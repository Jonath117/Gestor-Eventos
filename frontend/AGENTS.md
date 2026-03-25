# Gestor-Eventos Frontend - Agent & Contributor Guidelines

This document provides architectural context, tech stack usage, and coding guidelines for AI Agents and human contributors working on the **Campeando** frontend project.

## 1. Technology Stack
- **Core Framework**: React 19 via Vite
- **TypeScript**: Strict typing enabled
- **Routing**: React Router v7
- **Styling**: Tailwind CSS v4
- **State Management & Data Fetching**: TanStack React Query v5 & Axios
- **Linter & Formatter**: Biome (`npm run biome`)
- **API Mocking**: json-server (`npm run mock-api`)

## 2. Architectural Pattern: Vertical Slices / Feature-Based
The project follows a **Feature-Based (Vertical Slices)** folder structure to ensure long-term maintainability. Instead of grouping files purely by technical roles (e.g., separating all hooks, all components, all contexts globally), we group them by business domain (e.g., `events`, `auth`).

### Directory Layout (`src/`)

```text
src/
├── api/          # Global instances like Axios clients or shared API interceptors
├── assets/       # Global static assets (images, fonts, global CSS like index.css)
├── components/   # Application-wide UI components (e.g., Buttons, Loaders, ErrorBoundaries)
├── config/       # Environment variables, constants, and third-party setups
├── features/     # Isolated modules categorized by business domains
│   ├── auth/     # Authentication feature code
│   └── events/   # Event management feature code
├── hooks/        # Application-wide custom hooks (e.g., useLocalStorage)
├── pages/        # Route-level wrapper components that assemble different features
└── utils/        # Shared helper functions, formatters, and utilities
```

### Feature Module Structure
A typical feature inside `src/features/` should encapsulate its own concerns. For example, `src/features/events/`:
- `api/`: Specific HTTP calls (e.g., `getEvents`, `createEvent`).
- `components/`: UI pieces exclusively used by this domain (e.g., `EventList`, `EventSkeleton`).
- `hooks/`: Data-fetching hooks combining TanStack query with API calls (e.g., `useEvents`, `useDeleteEvent`).
- `types/`: Feature domain types and interfaces.
- `context/`: Module-specific React providers (if any).

*Rule of thumb:* If a component/hook is used across multiple features, move it to the global `components/` or `hooks/` directory.

## 3. Best Practices & Coding Standards

- **Component Organization**: Use arrow functions (`export const MyComponent = ...`) for components. Keep components focused on a single responsibility.
- **Routing**: `pages/` should primarily be responsible for mapping routes and composing logic/components imported from `features/`. Pages themselves should remain thinly composed layouts.
- **Data Fetching & HTTP Client**: Always use the configured Axios client from `src/api/client.ts` for HTTP requests. **DO NOT use the native `fetch` API** or raw axios instances. Wrap these API requests using **TanStack React Query** (`useQuery` / `useMutation`) inside the `features/<feature-name>/hooks/` folder to decouple the UI from the API.
- **Formatting**: Rely on **Biome** for all code formatting and linting. Do not use Prettier or ESLint as Biome replaces both.
- **Typing**: Avoid using `any`. Use descriptive interfaces and export them from the respective feature's `types/` folder.
