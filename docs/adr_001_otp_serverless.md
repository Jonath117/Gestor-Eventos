# Architecture Decision Record (ADR) 001: Transición a Arquitectura OTP Asíncrona Serverless (FaaS)

## Estado
Aprobado

## Contexto
El flujo original de registro de participantes requería el uso de un código de un solo uso (OTP) enviado al correo del usuario. En la implementación inicial, la generación del OTP se realizaba de manera **síncrona** en el hilo de la petición HTTP principal del backend, persistiendo temporalmente el código en un caché en memoria (`IMemoryCache`) de la aplicación principal. 

Este enfoque presentaba varios inconvenientes críticos de arquitectura y de nube:
1.  **Bloqueo de Hilos (Resource Blocking):** La generación de códigos y el subsecuente envío de emails/SMS (incluso simulados) consume tiempo de CPU e introduce latencia en la API principal.
2.  **Acoplamiento y Falta de Escalabilidad:** Si el servicio de correos experimenta caídas o picos drásticos de tráfico (ej. apertura de inscripciones masivas), el backend principal puede colapsar por saturación.
3.  **Estado Volátil en Memoria:** El uso de `IMemoryCache` impide escalar horizontalmente el backend a múltiples instancias de contenedores, ya que el estado no está sincronizado entre ellos (falla en arquitecturas distribuidas multi-región).

## Decisión
Hemos decidido desacoplar el flujo de solicitud de OTP mediante un patrón **Asíncrono basado en Persistencia Serverless (FaaS)**:
1.  **Endpoint No Bloqueante (HTTP 202):** El endpoint `/api/registration/{eventId}/request-otp` en .NET ya no genera el código. En su lugar, registra la intención de solicitud en la tabla `otp_requests` de PostgreSQL con un estado `"pendiente"`, retornando inmediatamente un estado `HTTP 202 Accepted` al cliente.
2.  **Desacoplamiento con FaaS (Serverless OTP):** Creamos un servicio serverless aislado en Node.js (ubicado en `/serverless-otp`) que responde de manera asíncrona a eventos. Este servicio:
    *   Recibe un evento JSON conteniendo el `user_id` (email) y `tenant_id` (eventId).
    *   Genera de forma segura el código de 6 dígitos.
    *   Escribe el código en la base de datos externa de Neon y marca el estado del registro como `"procesado"`.
    *   Simula el envío físico del correo por consola de logs serverless (desacoplado del backend).
3.  **Verificación Consistente:** El endpoint de verificación (`verify-otp`) lee directamente de la base de datos PostgreSQL la solicitud en estado `"procesado"`, garantizando la consistencia del estado sin importar el contenedor de backend que atienda la petición.

## Consecuencias
*   **Positivas:**
    *   **Alta Disponibilidad:** La API responde en milisegundos (< 50ms) al delegar el procesamiento real a la nube.
    *   **Escalabilidad Elástica:** La generación de OTPs escala de manera independiente de la aplicación principal gracias a la naturaleza Serverless de las Cloud Functions.
    *   **Soporte Multi-región:** Al centralizar el estado del OTP en la base de datos distribuida de Neon PostgreSQL, eliminamos el acoplamiento al estado en memoria local, permitiendo escalar el backend horizontalmente sin perder consistencia.
*   **Negativas:**
    *   **Consistencia Eventual / Latencia Corta:** El frontend debe esperar unos milisegundos a que la función serverless finalice la inserción del código antes de que el usuario intente validarlo (habitualmente cubierto por el tiempo de lectura física del correo por parte del usuario).
