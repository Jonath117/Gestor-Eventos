const { EmailService } = require("./services/emailService");
const { DbService } = require("./services/dbService");

const emailService = new EmailService();
const dbService = new DbService();

/**
 * Cloud Function triggered by Pub/Sub to process OTP requests.
 * Expected payload (base64 encoded in message.data):
 * {
 *   "requestId": "uuid",
 *   "userId": "string",
 *   "tenantId": "string",
 *   "email": "string"
 * }
 *
 * This function:
 * 1. Generates a 6-digit OTP code
 * 2. Persists the code in the database (marks as 'procesado')
 * 3. Sends the OTP email to the user
 */
exports.sendOtp = async (message, context) => {
	if (!message || !message.data) {
		console.error("Error: Pub/Sub message or message.data is missing.");
		return;
	}

	let dataString;
	try {
		dataString = Buffer.from(message.data, "base64").toString("utf-8");
	} catch (error) {
		console.error(`Error decoding Pub/Sub message data: ${error.message}`);
		throw error;
	}

	let payload;
	try {
		payload = JSON.parse(dataString);
	} catch (error) {
		console.error(
			`Error parsing message JSON payload: ${error.message}. Raw data: ${dataString}`,
		);
		throw error;
	}

	const { requestId, userId, tenantId, email } = payload;

	if (!requestId || !email) {
		console.error(
			`Error: Missing required fields in payload (requestId: ${requestId}, email: ${email})`,
		);
		return;
	}

	console.log(
		`Processing OTP request: requestId=${requestId}, email=${email}, tenantId=${tenantId}`,
	);

	// 1. Generate a 6-digit OTP code
	const otpCode = String(Math.floor(100000 + Math.random() * 900000));

	try {
		// 2. Persist the OTP code in the database and mark as 'procesado'
		const updated = await dbService.saveOtpCode(requestId, otpCode);
		if (!updated) {
			console.error(
				`OTP request ${requestId} not found or already processed. Skipping.`,
			);
			return;
		}
		console.log(
			`OTP code persisted in DB for request ${requestId}. Status: procesado.`,
		);

		// 3. Send the OTP email
		await emailService.sendOtp({ userId, tenantId, email, otpCode });
		console.log(`Successfully sent OTP email to ${email}`);
	} catch (error) {
		console.error(`Failed to process OTP for ${email}: ${error.message}`);
		throw error;
	}
};
