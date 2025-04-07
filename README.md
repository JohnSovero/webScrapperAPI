# webScrapperAPI
Backend of Provider-risk-screening-app

Este proyecto está compuesto por un **backend en .NET 9.0.202**. A continuación, se detallan los pasos para ejecutar ambos entornos en desarrollo y producción.

## Requisitos Previos

### Backend (.NET)

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio Code (opcional)

## Despliegue en Desarrollo
### 1. Clonar el repositorio
```bash
git clone https://github.com/JohnSovero/webScrapperAPI.git
cd src
```

### 2. Restaurar dependencias y compilar el proyecto

```bash
dotnet restore
dotnet build
```

### 3. Configurar la base de datos
- Crear base de datos llamada RiskScreeningDB en sql Express
- Configurar tu cadena de conexión en appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=RiskScreeningDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

### 4. Aplicar migraciones e iniciar el backend

```bash
dotnet ef database update
dotnet run
```

Por defecto, el backend se ejecuta en https://localhost:8080.

## Despliegue en Producción

### 1. Navegar al proyecto
```bash
cd webScrapperAPI/src
```

### 2. Publicar el proyecto en modo Release
```bash
dotnet publish -c Release -o ./publish
```
Esto generará los archivos necesarios para el despliegue en la carpeta publish.

## Despliegue en Azure con carpeta `publish`

### Requisitos

- Tener los archivos generados con `dotnet publish -c Release -o ./publish`
- Cuenta de Azure activa
- Crear una App Service en el portal de Azure

### Pasos

1. Ve al [Portal de Azure](https://portal.azure.com)
2. Crea un **App Service** con el stack `.NET 9 (LTS)` y sistema operativo Windows
3. En el App Service, ve a **Deployment Center**
4. Selecciona **Zip Deploy** o **Local Git** y elige la opción **Browse for a folder**
5. Selecciona la carpeta `publish` generada por el comando `dotnet publish`
6. Sube y despliega los archivos

### Configurar la conexión a base de datos

1. En el App Service, ve a **Configuration > Connection strings**
2. Agrega una nueva cadena llamada `DefaultConnection` con tu conexión a SQL Server
3. Marca el tipo como `SQL Server`
4. Guarda y reinicia la aplicación