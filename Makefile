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

front-build:
	@echo "Building frontend..."
	cd frontend && \
	npm install && \
	npm run build

front-run:
	@echo "Running frontend..."
	cd frontend && \
	npm install && \
	npm run start

docker-compose-up:
	@echo "Starting Docker Compose..."
	docker-compose up -d

docker-compose-down:
	@echo "Stopping Docker Compose..."
	docker-compose down

docker-compose-restart:
	@echo "Restarting Docker Compose..."
	docker-compose down && docker-compose up -d	

docker-compose-logs:
	@echo "Showing Docker Compose logs..."
	docker-compose logs -f

docker-compose-build:
	@echo "Building Docker Compose services..."
	docker-compose build

docker-compose-up-db:
	@echo "Starting Docker Compose with database..."
	docker-compose up -d db
