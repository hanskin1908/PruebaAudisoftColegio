# PruebaAudisoftColegio

## Descripcion general del proyecto

Aplicacion web en .NET con API REST y frontend MVC para administrar Estudiantes, Profesores y Notas.

**Objetivo**
Brindar un CRUD completo con reglas de negocio y paginacion, separando responsabilidades con Clean Architecture.

**Tecnologias utilizadas**
- .NET 6
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server

**Arquitectura aplicada (Clean Architecture)**
La solucion se organiza en capas con dependencias hacia adentro: Dominio -> Aplicacion -> Infraestructura -> API/Presentacion.

**Principios SOLID**
Se aplican SRP, OCP, LSP, ISP y DIP mediante interfaces, servicios y repositorios con responsabilidades acotadas.

## Requisitos del sistema

- .NET SDK 6.0 o superior
- SQL Server (LocalDB, Express o una instancia accesible)
- Herramienta `dotnet-ef`:
  - `dotnet tool install --global dotnet-ef`

## Instalacion paso a paso

1) Clonar el repositorio:

```powershell
git clone <URL_DEL_REPOSITORIO>
cd PruebaAudisoftColegio
```

2) Restaurar dependencias:

```powershell
dotnet restore
```

3) Configurar la cadena de conexion:
- Archivo: `Api/appsettings.json` y `Api/appsettings.Development.json`
- Clave: `ConnectionStrings:BaseDatosColegio`
- Ejemplo (LocalDB):

```json
"BaseDatosColegio": "Server=(localdb)\\MSSQLLocalDB;Database=BaseDatosColegio;Trusted_Connection=True;MultipleActiveResultSets=true"
```

4) Configurar appsettings del frontend:
- Archivo: `Web/appsettings.json` y `Web/appsettings.Development.json`
- Clave: `ConfiguracionApi:direccionApiBase`
- Valor esperado: `https://localhost:7278/`

5) Ejecutar migraciones de Entity Framework Core:

```powershell
dotnet ef database update --project Infraestructura --startup-project Api
```

6) Crear la base de datos:
El comando anterior crea la base de datos y aplica las migraciones en SQL Server.

7) Ejecutar la aplicacion (API):

```powershell
dotnet run --project Api
```

8) Ejecutar la aplicacion (Frontend MVC):

```powershell
dotnet run --project Web
```

9) Verificar que la API y el frontend estan activos:
- API: `https://localhost:7278/swagger`
- Frontend: `https://localhost:7286`

## Estructura del proyecto

- **Dominio**: Entidades puras y contratos (interfaces) de repositorio.
- **Aplicacion**: Servicios, DTOs, reglas de negocio y orquestacion.
- **Infraestructura**: EF Core, DbContext, repositorios y migraciones.
- **API**: Controladores REST y manejo HTTP.
- **Presentacion (Web)**: MVC que consume el API via HTTP.

**Relacion entre capas**
Dominio no depende de ninguna capa externa. Aplicacion depende solo de Dominio. Infraestructura implementa los contratos del Dominio. API orquesta servicios de Aplicacion. Web consume el API.

## Uso de la aplicacion

1) Acceder al sistema
- Abrir `https://localhost:7286` en el navegador.

2) Navegar entre modulos
- Usar el menu para Estudiantes, Profesores y Notas.

3) Crear, editar y eliminar registros
- Cada modulo incluye acciones de Crear, Editar y Eliminar.

4) Visualizar paginacion
- Los listados exponen `pageNumber` y `pageSize`.
- Respuesta con `totalRecords`, `totalPages`, `currentPage` y `data`.

5) Validaciones de eliminacion
- No se permite eliminar Estudiantes o Profesores con Notas asociadas.

## Capturas de la aplicacion

<img width="1918" height="382" alt="image" src="https://github.com/user-attachments/assets/8ae90ce6-c6cb-4ceb-b34b-a0c094e32de8" />


[AQUI CAPTURA - Listado de estudiantes con paginacion]

[AQUI CAPTURA - Formulario de creacion de estudiante]

[AQUI CAPTURA - Validacion al eliminar estudiante con notas]

[AQUI CAPTURA - Listado de profesores]

[AQUI CAPTURA - Listado de notas]

## Consideraciones tecnicas

- **Manejo de errores**: Las reglas de negocio lanzan excepciones controladas y el API responde con codigos HTTP adecuados.
- **Reglas de negocio**: Bloqueo de eliminacion si existen notas relacionadas.
- **Entity Framework Core**: DbContext centralizado y migraciones en `Infraestructura/Migrations`.
- **Inyeccion de dependencias**: Servicios y repositorios se registran por interfaces en el contenedor de DI.
