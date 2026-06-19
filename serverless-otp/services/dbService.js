const { Pool } = require("pg");

class DbService {
	constructor() {
		const databaseUrl = process.env.DATABASE_URL;

		if (!databaseUrl) {
			throw new Error(
				"Critical initialization failure: DATABASE_URL environment variable is required.",
			);
		}

		this.pool = new Pool({
			connectionString: databaseUrl,
			ssl: { rejectUnauthorized: false },
			max: 1,
		});
	}

	/**
	 * Saves the generated OTP code to the database and marks the request as 'procesado'.
	 * @param {string} requestId - The UUID of the OTP request.
	 * @param {string} otpCode - The generated 6-digit OTP code.
	 * @returns {Promise<boolean>} - True if the record was updated, false if not found.
	 */
	async saveOtpCode(requestId, otpCode) {
		const query = `
			UPDATE registration.otp_requests
			SET code = $1, status = 'procesado', processed_at = NOW()
			WHERE id = $2::uuid AND status = 'pendiente'
		`;

		const result = await this.pool.query(query, [otpCode, requestId]);
		return result.rowCount > 0;
	}
}

module.exports = { DbService };
