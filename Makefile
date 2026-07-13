PROJECT_INFRA=BookShare.Infrastructure.Postgres
STARTUP_PROJECT=BookShare.Core

db-create-migration:
	@echo "Creating migration: $(migration_name)..."
	cd backend && dotnet ef migrations add $(migration_name) --project $(PROJECT_INFRA) --startup-project $(STARTUP_PROJECT)

db-update-migration:
	@echo "Updating database..."
	cd backend && dotnet ef database update --project $(PROJECT_INFRA) --startup-project $(STARTUP_PROJECT)