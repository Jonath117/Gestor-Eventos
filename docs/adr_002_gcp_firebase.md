# Architecture Decision Record (ADR) 002: Elección de GCP Cloud Run y Firebase Hosting sobre Kubernetes (Ruta B)

## Estado
Aprobado

## Contexto
Para el despliegue del backend (.NET 10 conteneirizado) y del frontend (React / Vite) del sistema **Gestor-Eventos**, se evaluaron dos estrategias principales de despliegue en la nube pública:
1.  **Kubernetes (GKE - Google Kubernetes Engine):** Orquestación mediante pods, servicios, ingresses, balanceadores de carga y autoescaladores de pods (HPA).
2.  **Arquitectura Serverless/PaaS (GCP Cloud Run + Firebase Hosting):** Despliegue de contenedores administrados para el backend y distribución estática mediante CDN global para el frontend.

## Decisión
Hemos decidido implementar el despliegue utilizando la **Arquitectura Serverless / PaaS** mediante **GCP Cloud Run** (para el backend .NET 10) y **Firebase Hosting** (para el frontend de React), alineándonos con la **Ruta B** de modernización y reducción de complejidad operativa.

## Justificación

1.  **Complejidad Operativa Extremadamente Menor:**
    *   *Kubernetes:* Requiere la creación de clústeres, administración de grupos de nodos (Node Pools), configuración de manifiestos YAML complejos para Ingress, Services, Deployment, Secrets, Cert-Manager y políticas de red. Esto exige conocimientos de DevOps avanzados y mantenimiento constante.
    *   *Cloud Run y Firebase Hosting:* Son servicios completamente administrados. No requiere configurar servidores ni redes complejas. Google administra la infraestructura subyacente, balanceo de carga, enrutamiento y certificados SSL automáticos.
2.  **Autoescalado a Cero (Cost-Efficiency):**
    *   *Kubernetes:* Los nodos virtuales (VMs) del clúster deben estar encendidos de manera permanente para garantizar la disponibilidad mínima del plano de control y de los pods de sistema, incurriendo en un costo mensual fijo elevado.
    *   *Cloud Run:* Escala a **0 instancias** de manera automática cuando no hay peticiones activas, eliminando los costos de facturación por inactividad. Esto es ideal para entornos de desarrollo, pruebas y presentaciones académicas.
3.  **Rendimiento y CDN Global en Frontend:**
    *   *Firebase Hosting:* Distribuye los assets compilados de React directamente desde servidores perimetrales (CDN de Google), reduciendo la latencia de carga del sitio a milisegundos a nivel mundial, sin cargar el backend con peticiones estáticas.
4.  **Integración Directa de Pipelines (CI/CD Simplificado):**
    *   El despliegue a Cloud Run e Firebase Hosting se integra de forma directa mediante GitHub Actions nativos de Google, evitando la necesidad de lidiar con herramientas adicionales como Helm o kubectl en los agentes del runner.

## Consecuencias
*   **Positivas:**
    *   Despliegue rápido, seguro y económico.
    *   Cero costos de infraestructura en periodos de inactividad de la aplicación.
    *   Certificados SSL HTTPS autogestionados y aprovisionados de forma automática por Google.
    *   Curva de aprendizaje baja para los desarrolladores.
*   **Negativas:**
    *   *Cold Starts (Arranque en Frío):* Tras periodos prolongados de inactividad, la primera solicitud HTTP al backend puede experimentar una latencia de 1 a 2 segundos mientras se inicializa el contenedor .NET. Este efecto puede mitigarse configurando un número mínimo de instancias a 1 en producción, si los costos lo permiten.
