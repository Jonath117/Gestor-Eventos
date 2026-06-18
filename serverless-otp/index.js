const { Client } = require('pg');
const crypto = require('crypto');

/**
 * Serverless function to process an OTP request.
 * Expected event payload:
 * {
 *   "user_id": "user@example.com",
 *   "tenant_id": "event-guid"
 * }
 */
exports.generateOtp = async (req, res) => {
  // Manejo de Cloud Functions de GCP (req y res) o ejecución directa
  let userId, tenantId;
  
  if (req && req.body) {
    userId = req.body.user_id;
    tenantId = req.body.tenant_id;
  } else {
    // Si se invoca con payload directamente
    userId = req.user_id;
    tenantId = req.tenant_id;
  }

  if (!userId || !tenantId) {
    const errorMsg = 'Error: Faltan los parámetros obligatorios user_id o tenant_id.';
    console.error(errorMsg);
    if (res && res.status) {
      return res.status(400).json({ error: errorMsg });
    }
    throw new Error(errorMsg);
  }

  // Generar un código de 6 dígitos aleatorio
  const otpCode = Math.floor(100000 + Math.random() * 900000).toString();
  console.log(`Generando OTP para Usuario: ${userId}, Evento: ${tenantId}`);

  // Configuración del cliente PostgreSQL a partir del entorno
  const client = new Client({
    connectionString: process.env.DATABASE_URL || process.env.ConnectionStrings__NeonPostgres || process.env.ConnectionStrings__DefaultConnection,
    ssl: {
      rejectUnauthorized: false // Para Neon.tech y Supabase que requieren SSL
    }
  });

  try {
    await client.connect();

    // 1. Buscar la solicitud pendiente más reciente de este usuario/tenant
    const selectQuery = `
      SELECT id FROM registration.otp_requests 
      WHERE user_id = $1 AND tenant_id = $2 AND status = 'pendiente' 
      ORDER BY created_at DESC 
      LIMIT 1
    `;
    const selectRes = await client.query(selectQuery, [userId, tenantId]);

    if (selectRes.rows.length > 0) {
      const requestId = selectRes.rows[0].id;
      
      // 2. Actualizar el código y marcar como 'procesado'
      const updateQuery = `
        UPDATE registration.otp_requests 
        SET code = $1, status = 'procesado', processed_at = NOW() 
        WHERE id = $2
      `;
      await client.query(updateQuery, [otpCode, requestId]);
      console.log(`Solicitud de OTP ${requestId} actualizada correctamente con el código generado.`);
    } else {
      // 3. Failsafe: Si no existe una solicitud pendiente, la creamos directamente procesada
      const newId = crypto.randomUUID();
      const insertQuery = `
        INSERT INTO registration.otp_requests (id, user_id, tenant_id, code, status, created_at, processed_at) 
        VALUES ($1, $2, $3, $4, 'procesado', NOW(), NOW())
      `;
      await client.query(insertQuery, [newId, userId, tenantId, otpCode]);
      console.log(`Failsafe: No se encontró solicitud pendiente. Se creó una nueva con ID: ${newId}.`);
    }

    // Simular el envío enviándolo a la consola (en producción se usaría Twilio, SendGrid, etc.)
    console.log(`========================================`);
    console.log(`[EMAIL SEND SIMULATION]`);
    console.log(`Para: ${userId}`);
    console.log(`Asunto: Tu código de verificación de evento`);
    console.log(`Tu código es: ${otpCode}`);
    console.log(`========================================`);

    const responsePayload = {
      message: 'OTP procesado exitosamente por la función Serverless FaaS.',
      user_id: userId,
      tenant_id: tenantId,
      status: 'procesado'
    };

    if (res && res.status) {
      return res.status(200).json(responsePayload);
    }
    return responsePayload;

  } catch (error) {
    console.error('Error procesando OTP en la función Serverless:', error);
    if (res && res.status) {
      return res.status(500).json({ error: 'Internal Server Error', details: error.message });
    }
    throw error;
  } finally {
    await client.end();
  }
};
