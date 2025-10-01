# IdentityAPI - Backend Installation Guide

API REST para gestión de identidad y autenticación construida con .NET 9, PostgreSQL y Docker.

## 🎯 Descripción del Proyecto

IdentityAPI es un sistema completo de autenticación y autorización que incluye:
- **Autenticación JWT** con refresh tokens
- **Gestión de usuarios** y roles
- **Base de datos PostgreSQL** con Entity Framework Core
- **Documentación automática** con Swagger/OpenAPI
- **Contenedorización** con Docker
- **Arquitectura limpia** (Clean Architecture)

## 📋 Tabla de Contenidos

- [Prerrequisitos](#prerrequisitos)
- [Instalación Rápida con Docker](#instalación-rápida-con-docker)
- [Instalación para Desarrollo](#instalación-para-desarrollo)
- [Configuración](#configuración)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Endpoints Principales](#endpoints-principales)
- [Usuario por Defecto](#usuario-por-defecto)
- [Desarrollo](#desarrollo)
- [Troubleshooting](#troubleshooting)

## 🚀 Prerrequisitos

### Opción 1: Solo Docker (Recomendado para pruebas rápidas)
- **Docker Desktop** 4.0+ 
- **Git** para clonar el repositorio

### Opción 2: Desarrollo local
- **.NET 9 SDK** - [Descargar aquí](https://dotnet.microsoft.com/download/dotnet/9.0)
- **PostgreSQL 16** - [Descargar aquí](https://www.postgresql.org/download/)
- **Visual Studio 2022** o **VS Code** (opcional)
- **Git** para clonar el repositorio

## 🐳 Instalación Rápida con Docker

### Paso 1: Clonar el repositorio
```bash
git clone https://github.com/nkonko/IdentityAPI.git
cd IdentityAPI
```

### Paso 2: Ejecutar con Docker Compose
```bash
# Levantar todos los servicios
docker compose up --build

# O en segundo plano
docker compose up -d --build
```

### Paso 3: Verificar instalación
Una vez iniciado, tendrás acceso a:

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **API Swagger** | http://localhost:5000/swagger | Documentación interactiva |
| **API Base** | http://localhost:5000 | Endpoint base de la API |
| **PgAdmin** | http://localhost:8085 | Administrador web de PostgreSQL |
| **PostgreSQL** | localhost:5432 | Base de datos (conexión directa) |

#### Credenciales por defecto:
- **PgAdmin**: admin@admin.com / admin
- **PostgreSQL**: postgres / postgres
- **Usuario Admin**: admin / Admin123!

## 💻 Instalación para Desarrollo

### Opción A: Solo Docker (Más Fácil - Recomendado)

**✅ Recomendado para desarrollo:** No necesitas instalar PostgreSQL ni configurar nada.

```bash
# Clonar repositorio
git clone https://github.com/nkonko/IdentityAPI.git
cd IdentityAPI

# Levantar todo con Docker
docker compose up --build
```

**¡Eso es todo!** Esto levanta automáticamente:
- ✅ **API** en http://localhost:5000 con hot-reload
- ✅ **PostgreSQL** con datos iniciales
- ✅ **PgAdmin** en http://localhost:8085
- ✅ **Volúmenes** para persistir datos

**Para desarrollo activo:**
```bash
# Ver logs en tiempo real
docker compose logs -f api

# Parar y limpiar (si necesitas)
docker compose down

# Reconstruir después de cambios importantes
docker compose up --build
```

---

### Opción B: Instalación Local Completa

**Para desarrolladores que prefieren todo local:**

#### Paso 1: Clonar y navegar
```bash
git clone https://github.com/nkonko/IdentityAPI.git
cd IdentityAPI
```

#### Paso 2: Instalar PostgreSQL
- **Windows**: [Descargar PostgreSQL](https://www.postgresql.org/download/windows/)
- **macOS**: `brew install postgresql && brew services start postgresql`
- **Linux**: `sudo apt install postgresql postgresql-contrib`

#### Paso 3: Configurar connection string (opcional)
Si quieres usar credenciales diferentes a `postgres/postgres`, edita `identityAPI.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=identityapidb;Username=postgres;Password=tu_password"
  }
}
```

#### Paso 4: Restaurar dependencias
```bash
dotnet restore
```

#### Paso 5: Ejecutar la aplicación
```bash
# Modo desarrollo con hot-reload (recomendado)
dotnet watch run --project identityAPI.Api

# O ejecución normal
dotnet run --project identityAPI.Api
```

**📝 Nota:** La aplicación creará automáticamente:
- ✅ **Base de datos** `identityapidb` (si no existe)
- ✅ **Tablas** mediante migraciones automáticas
- ✅ **Roles** `User` y `Admin`
- ✅ **Usuario administrador** por defecto

**Para administrar la base de datos localmente:**
- Instalar [pgAdmin](https://www.pgadmin.org/download/) por separado
- O usar línea de comandos: `psql -U postgres -d identityapidb`

## ⚙️ Configuración

### Variables de Entorno Importantes

| Variable | Descripción | Valor por Defecto |
|----------|-------------|-------------------|
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | `Development` |
| `ConnectionStrings__DefaultConnection` | Cadena de conexión a PostgreSQL | Ver appsettings.json |
| `Jwt__Key` | Clave secreta para JWT | **⚠️ CAMBIAR EN PRODUCCIÓN** |
| `Jwt__Issuer` | Emisor del token JWT | `identityAPI` |
| `Jwt__Audience` | Audiencia del token JWT | `identityAPIUsers` |

### Configuración de Producción

Para producción, **IMPORTANTE**:

1. **Cambiar la clave JWT** a una clave segura de 32+ caracteres
2. **Usar User Secrets** o variables de entorno para datos sensibles
3. **Configurar HTTPS** correctamente
4. **Revisar configuración de CORS** según tus necesidades

```bash
# Configurar User Secrets (desarrollo)
dotnet user-secrets init --project identityAPI.Api
dotnet user-secrets set "Jwt:Key" "tu_clave_super_secreta_de_32_caracteres_o_mas" --project identityAPI.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "tu_connection_string_real" --project identityAPI.Api
```

## 🏗️ Estructura del Proyecto

```
IdentityAPI/
├── identityAPI.Api/              # Capa de presentación (Controllers, Middleware)
│   ├── Controllers/              # Controladores REST
│   ├── Middleware/               # Middleware personalizado
│   ├── Program.cs                # Punto de entrada de la aplicación
│   ├── appsettings.json          # Configuración de producción
│   └── appsettings.Development.json # Configuración de desarrollo
├── identityAPI.Core/             # Capa de dominio (Entities, Models)
│   ├── Entities/                 # Entidades del dominio
│   ├── Models/                   # DTOs y modelos
│   └── ...
├── identityAPI.Infrastructure/   # Capa de infraestructura (Data, Services)
│   ├── Persistence/              # DbContext y configuraciones EF
│   ├── Services/                 # Implementaciones de servicios
│   ├── Migrations/               # Migraciones de Entity Framework
│   └── ...
├── docker-compose.yml            # Configuración Docker para producción
├── docker-compose.override.yml   # Configuración Docker para desarrollo
└── README.md                     # Este archivo
```

### Arquitectura
El proyecto sigue los principios de **Clean Architecture**:
- **Separación de responsabilidades** por capas
- **Inversión de dependencias** mediante interfaces
- **Testabilidad** y mantenibilidad
- **Independencia de frameworks** externos

## 📡 Endpoints Principales

### Autenticación
```http
POST /api/auth/register          # Registrar nuevo usuario
POST /api/auth/login             # Iniciar sesión
POST /api/auth/refresh           # Renovar token JWT
```

### Gestión de Usuarios
```http
GET    /api/users                # Listar usuarios
GET    /api/users/{id}           # Obtener usuario por ID
POST   /api/users                # Crear usuario
PUT    /api/users/{id}           # Actualizar usuario
DELETE /api/users/{id}           # Eliminar usuario
POST   /api/users/{id}/password  # Cambiar contraseña
```

### Gestión de Roles
```http
GET    /api/roles                # Listar roles
POST   /api/roles                # Crear rol
PUT    /api/roles/{id}           # Actualizar rol
DELETE /api/roles/{id}           # Eliminar rol
```

### Otros Endpoints
```http
GET /api/dashboard               # Dashboard con estadísticas
GET /api/settings                # Configuraciones del sistema
GET /api/subscriptions           # Gestión de suscripciones
GET /api/payments                # Gestión de pagos
GET /api/audit                   # Auditoría del sistema
```

### Documentación Completa
Una vez ejecutada la aplicación, visita **http://localhost:5000/swagger** para ver la documentación completa e interactiva de todos los endpoints.

## 👤 Usuario por Defecto

Al iniciar la aplicación por primera vez, se crea automáticamente:

**Usuario Administrador:**
- **Username:** `admin`
- **Email:** `admin@demo.com`
- **Password:** `Admin123!`
- **Rol:** `Admin`

**⚠️ IMPORTANTE:** Cambia esta contraseña inmediatamente en entornos de producción.

## 🛠️ Desarrollo

### Comandos Útiles

```bash
# Restaurar dependencias
dotnet restore

# Compilar proyecto
dotnet build

# Ejecutar tests (cuando los agregues)
dotnet test

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project identityAPI.Infrastructure --startup-project identityAPI.Api

# Aplicar migraciones
dotnet ef database update --project identityAPI.Api

# Limpiar y reconstruir
dotnet clean && dotnet build

# Publicar para producción
dotnet publish --configuration Release
```

### Hot Reload para Desarrollo
```bash
# Modo watch (recarga automática en cambios)
dotnet watch run --project identityAPI.Api
```

### Docker para Desarrollo
```bash
# Solo backend + base de datos
docker compose up --build


# Ver logs en tiempo real
docker compose logs -f

# Parar servicios
docker compose down

# Limpiar volúmenes y reconstruir
docker compose down -v && docker compose up --build
```

## 🐛 Troubleshooting

### Problema: Error de conexión a la base de datos
```
Npgsql.NpgsqlException: Connection refused
```

**Soluciones:**
1. Verificar que PostgreSQL esté ejecutándose
2. Comprobar la cadena de conexión en `appsettings.json`
3. Verificar que la base de datos y usuario existan
4. En Docker: esperar a que el contenedor de DB esté listo

### Problema: Error de migración
```
Unable to create an object of type 'ApplicationDbContext'
```

**Soluciones:**
1. Verificar que la cadena de conexión sea válida
2. Ejecutar desde la carpeta raíz: `dotnet ef database update --project identityAPI.Api`
3. Verificar que el proyecto de startup sea correcto

### Problema: JWT inválido
```
401 Unauthorized
```

**Soluciones:**
1. Verificar que la clave JWT sea la misma en configuración
2. Comprobar que el token no haya expirado
3. Validar formato del header: `Authorization: Bearer {token}`

### Problema: Puerto ocupado
```
EADDRINUSE: address already in use :::5000
```

**Soluciones:**
1. Cambiar puerto en `launchSettings.json`
2. Matar proceso que usa el puerto: `netstat -ano | findstr :5000`
3. En Docker: `docker compose down` antes de volver a ejecutar

### Problema: Permisos de Docker en Linux/Mac
```
Permission denied
```

**Soluciones:**
1. Agregar usuario al grupo docker: `sudo usermod -aG docker $USER`
2. Reiniciar sesión o ejecutar: `newgrp docker`
3. Verificar permisos: `docker ps`

## 📞 Soporte

### Para Problemas Técnicos
1. **Revisar logs:** `docker compose logs api` o logs de la aplicación
2. **Verificar configuración:** Comprobar `appsettings.json` y variables de entorno
3. **Consultar documentación:** Swagger UI en http://localhost:5000/swagger
4. **Issues en GitHub:** [Reportar problemas aquí](https://github.com/nkonko/IdentityAPI/issues)

### Para Desarrollo
- **Swagger UI:** Documentación interactiva de la API
- **PgAdmin:** Interfaz web para administrar PostgreSQL
- **Logs detallados:** Configurados para Development environment

---

## 🎉 ¡Todo Listo!

Si seguiste estos pasos correctamente, deberías tener:
- ✅ API ejecutándose en http://localhost:5000
- ✅ Base de datos PostgreSQL configurada
- ✅ Usuario administrador creado
- ✅ Documentación disponible en Swagger
- ✅ Entorno listo para desarrollo

**Próximos pasos sugeridos:**
1. Explorar la API usando Swagger UI
2. Probar endpoints de autenticación
3. Crear usuarios y roles adicionales
4. Configurar tu aplicación frontend para consumir la API

¡Happy coding! 🚀