.PHONY: infra-up infra-down db-migrate db-reset dev dev-backend dev-frontend dev-serverless

infra-up:
	docker compose up -d

infra-down:
	docker compose down

db-migrate:
	export PATH="$$PATH:$$HOME/.dotnet/tools" && dotnet ef database update --project backend/src/Modules/Core/Core.Infrastructure --startup-project backend/src/Web.API --context CoreDbContext
	export PATH="$$PATH:$$HOME/.dotnet/tools" && dotnet ef database update --project backend/src/Modules/Identity/Identity.Infrastructure --startup-project backend/src/Web.API --context IdentityDbContext
	export PATH="$$PATH:$$HOME/.dotnet/tools" && dotnet ef database update --project backend/src/Modules/Logistics/Logistics.Infrastructure --startup-project backend/src/Web.API --context LogisticsDbContext
	export PATH="$$PATH:$$HOME/.dotnet/tools" && dotnet ef database update --project backend/src/Modules/Payment/Payment.Infrastructure --startup-project backend/src/Web.API --context PaymentDbContext
	export PATH="$$PATH:$$HOME/.dotnet/tools" && dotnet ef database update --project backend/src/Modules/Registration/Registration.Infrastructure --startup-project backend/src/Web.API --context RegistrationDbContext

db-reset:
	docker compose down -v
	docker compose up -d db
	@echo "Base de datos reseteada. Las migraciones se aplicaran automaticamente al iniciar la API (make dev-backend)."

dev:
	pnpm dev

dev-backend:
	pnpm dev:backend

dev-frontend:
	pnpm dev:frontend

dev-serverless:
	pnpm dev:serverless
