# Campeando

**Campeando** es una plataforma SaaS (Software as a Service) **Multi-tenant** diseñada para la gestión logística integral de eventos cerrados, tales como retiros espirituales, campamentos universitarios y convenciones corporativas. 

A diferencia de las ticketeras convencionales, el foco de este sistema no es solo la venta de entradas, sino el control operativo post-registro: acreditación digital con QR vía WhatsApp, gestión estricta de raciones alimentarias y analítica de asistencia en tiempo real.

---

## Características Principales (Core Features)

* **Arquitectura Multi-tenant:** Aislamiento de datos mediante base de datos compartida y discriminador de `TenantId`.
* **Acreditación Digital con QR:** Generación de códigos únicos por asistente.
* **Logística y Catering Granular:** Validación en tiempo real del consumo de raciones (evitando fraudes y desperdicios).
* **Dashboard Analítico:** Visualización de KPIs (asistencia, consumos, distribución de grupos).
* **Modo Offline-First (Mobile):** App en Flutter capaz de hacer check-ins sin internet y sincronizar posteriormente.

---

## Stack Tecnológico

* **Backend:** .NET 10 (C# 14) / Web API.
* **Frontend:** React 19 (Vite) con TypeScript.
* **Mobile (Próximamente):** Flutter.
* **Persistencia:** PostgreSQL vía Entity Framework Core.
* **Gestor de Paquetes (Monorepo Node):** `pnpm` (Workspace).

---

## Estructura del Proyecto

El repositorio sigue un enfoque de Monorepo e incluye las siguientes carpetas principales:

```text
/
├── backend/                # Solución principal de .NET (Monolito Modular)
├── frontend/               # Aplicación React/Vite
├── scripts/                # Utilidades y scripts para hooks
├── .husky/                 # Configuración de Git Hooks (Linting/Formatting pre-commit)
├── package.json            # Root package.json (Configuración de Husky, pnpm)
└── pnpm-workspace.yaml     # Definición de workspaces
```

### Backend: Arquitectura (.NET 10)
Está diseñado como un **Monolito Modular** siguiendo **Clean Architecture y DDD**.

Cada módulo (ej. `Events`, `Identity`, `Logistics`) se divide en 4 proyectos físicos:
1. **Domain:** Entidades de negocio, excepciones y reglas puras (cero dependencias externas).
2. **Application:** CQRS + Vertical Slices. Organizado por *Feature* (ej. `/Features/Events/GetAllEvents/`), utilizando MediatR y FluentValidation.
3. **Infrastructure:** EF Core, bases de datos (DbContext por esquema) y servicios externos.
4. **Presentation:** Controladores HTTP (API RESTful). 

El Host principal se orquesta desde un único punto de entrada: `Web.API`.

> *Nota:* La gestión de paquetes NuGet es centralizada a través de `Directory.Packages.props`.

### Frontend: Arquitectura (React 19)
Sigue un patrón **Feature-Based (Vertical Slices)** para facilitar la escalabilidad.

```text
frontend/src/
├── api/          # Clientes Axios globales
├── components/   # UI components globales
├── features/     # Módulos por dominio de negocio (auth, events, etc.)
├── hooks/        # Hooks globales
├── pages/        # Composición de vistas (rutas)
└── utils/        # Funciones auxiliares
```
Cada feature encapsula sus propios `api`, `components`, `hooks` y `types`.
Se prioriza **TanStack Query (React Query v5)** con **Axios** para el estado del servidor, y **Context API** para estado del cliente (Redux o Zustand no están permitidos). **Biome** reemplaza a ESLint/Prettier para el linting y formato.

---

## Requisitos Previos

Asegúrate de tener instalado el siguiente software antes de levantar el proyecto:

* [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet)
* [Node.js](https://nodejs.org/) (v20 o superior recomendado)
* [pnpm](https://pnpm.io/es/) (v10+ recomendado)
* PostgreSQL (Instancia local o contenedor Docker)

---

## Configuración de Variables de Entorno

El proyecto requiere variables de entorno separadas para el frontend y el backend.

### Backend (`backend/Web.API/appsettings.Development.json`)
Crea o modifica el archivo `appsettings.Development.json` con tu cadena de conexión y credenciales:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EventCampDB;Username=postgres;Password=tu_password"
  },
  "JwtSettings": {
    "Secret": "TU_SECRETO_SUPER_SEGURO_PARA_JWT",
    "Issuer": "Campeando",
    "Audience": "CampeandoApp",
    "ExpiryMinutes": 60
  }
}
```

### Frontend (`frontend/.env`)
Crea un archivo `.env` en la ruta `frontend/`:
```env
VITE_API_URL=http://localhost:5000/api
```

---

## Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/Jonath117/Gestor-Eventos.git
   cd Gestor-Eventos
   ```

2. **Instalar dependencias del workspace (Frontend & Root Utilities):**
   ```bash
   pnpm install
   ```

3. **Restaurar dependencias del Backend:**
   ```bash
   cd backend
   dotnet restore
   ```

---

## Ejecución con Docker Compose

El proyecto está configurado para ejecutarse fácilmente mediante Docker Compose. Se utiliza **Neon** como base de datos externa, por lo que no es necesario levantar un contenedor de base de datos local.

### Requisitos Previos para Docker
* Docker y Docker Compose instalados.
* Archivo `.env` en la raíz del proyecto con las siguientes variables:
  ```env
  CONNECTION_STRING=tu_cadena_de_conexion_a_neon
  JWT_SECRET=tu_secreto_jwt
  JWT_ISSUER=Campeando
  JWT_AUDIENCE=CampeandoApp
  JWT_EXPIRY_MINUTES=60
  ASPNETCORE_ENVIRONMENT=Development
  ```

### 1. Entorno de Desarrollo (con Hot-Reload)
Este modo utiliza `docker-compose.yml` + `docker-compose.override.yml`. Permite que los cambios en el código se reflejen automáticamente (Backend usa `dotnet watch`, Frontend mapea volúmenes).

```bash
# Levantar el proyecto en desarrollo
docker compose up --build
```
*   **Backend:** http://localhost:5000
*   **Frontend:** http://localhost:5173

### 2. Entorno de Producción (Simulación)
Este modo utiliza `docker-compose.yml` + `docker-compose.prod.yml`. Compila las imágenes en modo `Release` y sirve el frontend mediante Nginx optimizado.

```bash
# Levantar el proyecto en modo producción
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --build
```
*   **Backend:** http://localhost:5000 (Mapeado a puerto 8080 interno)
*   **Frontend:** http://localhost:80 (Nginx)

---

## Guía de Uso básica para el desarrollo

### Ejecutar el Backend
Desde la raíz del backend (o en Visual Studio / Rider):
```bash
cd backend/src/Web.API # (Ubicación del entrypoint .NET)
dotnet run
```
*Esto levantará el servidor local y los controladores RESTful*

### Ejecutar el Frontend
Se puede utilizar Vite para iniciar el entorno de desarrollo:
```bash
cd frontend
pnpm dev
# O usar npx vite
```
*Se recomienda tener instalada la extensión Biome en tu IDE para formateo automático al guardar.*

### Migraciones de Base de Datos
Debido a la arquitectura Clean, las migraciones se corren hacia el proyecto principal desde Infrastructure:
```bash
cd backend
dotnet ef migrations add InitialCreate --project src/NOMBREDELMODULO.Infrastructure --startup-project src/Web.API
dotnet ef database update --project src/NOMBREDELMODULO.Infrastructure --startup-project src/Web.API
```
