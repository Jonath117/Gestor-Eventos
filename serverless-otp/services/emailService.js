class EmailService {
	constructor() {
		this.provider = process.env.EMAIL_PROVIDER;

		// Fail-Fast: critical validation during initialization/cold start
		if (this.provider === "brevo") {
			const missing = [];
			if (!process.env.BREVO_API_KEY) missing.push("BREVO_API_KEY");
			if (!process.env.EMAIL_SENDER) missing.push("EMAIL_SENDER");
			if (!process.env.EMAIL_NAME) missing.push("EMAIL_NAME");

			if (missing.length > 0) {
				throw new Error(
					`Critical initialization failure: Missing required Brevo environment variables: ${missing.join(", ")}`,
				);
			}
		} else if (this.provider !== "mailpit") {
			// Throw if provider is not defined or is unsupported
			throw new Error(
				`Critical initialization failure: EMAIL_PROVIDER must be 'brevo' or 'mailpit'. Current value: '${this.provider}'`,
			);
		}

		this.initProvider();
	}

	initProvider() {
		if (this.provider === "mailpit") {
			const nodemailer = require("nodemailer");
			const host = process.env.SMTP_HOST || "127.0.0.1";
			const port = parseInt(process.env.SMTP_PORT || "1025", 10);

			this.transporter = nodemailer.createTransport({
				host: host,
				port: port,
				secure: false,
			});
		}
	}

	async sendOtp({ userId, tenantId, email, otpCode }) {
		const subject = "Tu código de verificación OTP";
		const senderName = process.env.EMAIL_NAME;
		const senderEmail = process.env.EMAIL_SENDER;
		const htmlContent = `
      <html>
        <body>
          <h2>Código de Verificación</h2>
          <p>Tu código de verificación OTP es: <strong>${otpCode}</strong></p>
          <p>ID de inquilino: ${tenantId}</p>
          <p>ID de usuario: ${userId}</p>
        </body>
      </html>
    `;

		if (this.provider === "brevo") {
			const payload = {
				sender: {
					name: senderName,
					email: senderEmail,
				},
				to: [{ email: email }],
				subject: subject,
				htmlContent: htmlContent,
			};

			const response = await fetch("https://api.brevo.com/v3/smtp/email", {
				method: "POST",
				headers: {
					"api-key": process.env.BREVO_API_KEY,
					"Content-Type": "application/json",
				},
				body: JSON.stringify(payload),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Brevo HTTP error ${response.status}: ${errorText}`);
			}

			return await response.json();
		} else if (this.provider === "mailpit") {
			const info = await this.transporter.sendMail({
				from: `"${senderName}" <${senderEmail}>`,
				to: email,
				subject: subject,
				html: htmlContent,
			});
			return info;
		}
	}
}

module.exports = { EmailService };
