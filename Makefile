BACKEND=backend
PROJECT_INFRA=BookShare.Infrastructure.Postgres
STARTUP_PROJECT=BookShare.Core

db-create-migration:
ifndef migration_name
	$(error migration_name is required)
endif
	@echo "Creating migration $(migration_name)..."
	cd $(BACKEND) && \
	dotnet ef migrations add $(migration_name) \
		--project $(PROJECT_INFRA) \
		--startup-project $(STARTUP_PROJECT)

db-update-migration:
	@echo "Updating database..."
	cd $(BACKEND) && \
	dotnet ef database update \
		--project $(PROJECT_INFRA) \
		--startup-project $(STARTUP_PROJECT)

back-build:
	@echo "Building solution..."
	cd $(BACKEND) && \
	dotnet clean && \
	dotnet build

back-run:
	@echo "Running backend..."
	cd $(BACKEND) && \
	dotnet run --project $(STARTUP_PROJECT)