const { EmailService } = require("./services/emailService");

// Instantiating here at global scope ensures Fail-Fast on Cold Start if configuration is invalid.
const emailService = new EmailService();

/**
 * Cloud Function to process an OTP send request via Pub/Sub.
 * Expected event payload (base64 encoded in message.data):
 * {
 *   "userId": "string",
 *   "tenantId": "string",
 *   "email": "string",
 *   "otpCode": "string"
 * }
 */
exports.sendOtp = async (message, context) => {
	if (!message || !message.data) {
		const errorMsg = "Error: Pub/Sub message or message.data is missing.";
		console.error(errorMsg);
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

	const { userId, tenantId, email, otpCode } = payload;

	if (!email || !otpCode) {
		const errorMsg = `Error: Missing required fields in payload (email: ${email}, otpCode: ${otpCode})`;
		console.error(errorMsg);
		return;
	}

	try {
		await emailService.sendOtp({ userId, tenantId, email, otpCode });
		console.log(`Successfully sent OTP email to ${email}`);
	} catch (error) {
		console.error(`Failed to send OTP email to ${email}: ${error.message}`);
		throw error;
	}
};
