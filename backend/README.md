# Vehicles API

This is a REST API for managing vehicles.

## Setup database
To manage migrations using code first, open a terminal and navigate
to src/VehiclesApi. Install EF as a global tool:

`dotnet tool install --global dotnet-ef`

Create your local development database and apply all pending migrations:

`dotnet ef database update`
