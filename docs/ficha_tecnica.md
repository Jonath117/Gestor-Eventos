# Ficha Técnica: Arquitectura de Software y Estrategia de Multi-tenancy

Esta ficha técnica describe las decisiones de arquitectura de software implementadas para el proyecto **Gestor-Eventos (Campeando)**, alineado con las mejores prácticas de **Cloud Computing y Arquitecturas Modernas**.

---

## 1. Arquitectura de Software: Monolito Modular con Clean Architecture

El backend está diseñado como un **Monolito Modular** estructurado bajo los principios de **Clean Architecture** (Arquitectura Limpia). Esta estrategia combina la sencillez de despliegue de un único servicio con la independencia y bajo acoplamiento propios de los microservicios.

### Capas del Proyecto (Clean Architecture)

Cada módulo en la carpeta `/src/Modules/` está estrictamente dividido en las siguientes capas de responsabilidad:

1.  **Domain (Dominio):**
    *   **Función:** Contiene la lógica de negocio central, entidades puras, excepciones de dominio, interfaces de repositorios y enums.
    *   **Dependencias:** Ninguna (Capa más interna y pura).
2.  **Application (Aplicación):**
    *   **Función:** Define los casos de uso del sistema. Implementa el patrón **CQRS** (Command Query Responsibility Segregation) utilizando **MediatR**. Contiene los comandos, consultas, validadores de datos (FluentValidation) y manejadores.
    *   **Dependencias:** Solo depende de la capa de *Domain*.
3.  **Infrastructure (Infraestructura):**
    *   **Función:** Provee implementaciones técnicas concretas. Configura el acceso a datos mediante **Entity Framework Core**, mapeo de entidades (Configurations), migraciones de base de datos de PostgreSQL y clientes externos.
    *   **Dependencias:** Depende de las capas de *Domain* y *Application*.
4.  **Presentation (Presentación):**
    *   **Función:** Expone los controladores REST que sirven como puntos de entrada HTTP (Endpoints) del módulo.
    *   **Dependencias:** Depende de las capas de *Application* e *Infrastructure*.

### Composition Root (Punto de Composición)
*   **Web.API:** El proyecto principal de ejecución actúa como la raíz de composición. Registra secuencialmente los controladores y las dependencias de infraestructura de cada uno de los módulos mediante métodos de extensión `.AddModuleInfrastructure()`, cargando configuraciones dinámicas de entorno.

---

## 2. Estrategia de Multi-tenancy (Multi-inquilino)

La aplicación implementa un modelo de **Multi-tenancy** diseñado para soportar múltiples organizaciones organizadoras de eventos de manera aislada y segura en una infraestructura compartida.

### Modelo Físico de Aislamiento
*   **Esquemas Compartidos en Base de Datos Única (Shared Database, Separate Schemas):**
    *   Cada módulo define su propio esquema lógico en PostgreSQL mediante Entity Framework Core (ej. esquema `core`, `registration`, `identity`, `logistics`, `payment`).
    *   El aislamiento a nivel de inquilino (Tenant) se realiza mediante un identificador único global `TenantId` / `OrganizationId` persistido en las entidades clave.
*   **Inyección Dinámica de Contexto:**
    *   Un `ITenantProvider` intercepta el token JWT o las cabeceras HTTP de cada petición para identificar el inquilino correspondiente de forma transparente.
    *   Se aplican **Query Filters** globales en Entity Framework para filtrar automáticamente las consultas por el `TenantId` activo de la sesión del usuario, mitigando riesgos de fuga de datos entre inquilinos (*Data Leaks*).
