
terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0.1"
    }
  }
}

provider "docker" {
  host = var.docker_host
} 

resource "docker_image" "api_image" {
  name = "app-api:10"
  build {
    context    = abspath("${path.module}/../")   
    dockerfile = "Dockerfile"  
  }
}

resource "docker_image" "react_app" {
  name = "vite-react-app:1"
  build {
    context    = "${path.module}/../ux/"   
    dockerfile = "Dockerfile"   
  }   
} 
 
# Define Docker network
resource "docker_network" "app_network" {
  name = var.network_name
  depends_on   = [docker_image.api_image]
}

# Define persistent volume for SQL Server data
resource "docker_volume" "sqlserver_data" {
  name = var.db_volume_name
  depends_on   = [docker_image.api_image]
}

# Define SQL Server container with persistent storage
resource "docker_container" "sql_server_db" {
  name         = var.db_container_name
  image        = var.db_image_name
  ports {
    internal = 1433
    external = 8002
  }
  env = [
    "ACCEPT_EULA=Y",
    "MSSQL_SA_PASSWORD=myStong_Password123#"
  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_image.api_image]
  # Mount the persistent volume for SQL Server data
  mounts {
    target = "/var/opt/mssql"
    source = docker_volume.sqlserver_data.name
    type   = "volume"
  }
}

# Define API container
resource "docker_container" "api_container" {
  name         = var.api_container_name
  image        = docker_image.api_image.name
  ports {
    internal = 80
    external = 8001
  }
  env = [
    "ConnectionStrings__dbConnectionString=Server=${docker_container.sql_server_db.name};Database=SqlDemoDb;User Id=SA;Password=myStong_Password123#;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
    "ASPNETCORE_ENVIRONMENT=Development",
    "ASPNETCORE_URLS=http://+:80"
  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.sql_server_db, docker_image.api_image]
  
  # Define volumes for API container
  volumes {
    host_path      = "${pathexpand("~")}/.microsoft/usersecrets"
    container_path = "/root/.microsoft/usersecrets"
    read_only      = true
  }

  volumes {
    host_path      = "${pathexpand("~")}/.aspnet/https"
    container_path = "/root/.aspnet/https"
    read_only      = true
  }
} 
 

# React App Container
resource "docker_container" "react_app_container" {
  name         = "react_app_container"
  image        = docker_image.react_app.name   
  ports {
    internal = 80
    external = 3000
  }
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.api_container, docker_image.react_app]
}