# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Obligatorio de Programación 3 (ORT) — "Escoge tu propia aventura": una plataforma de
lectura interactiva donde los Administradores arman historias compuestas por
capítulos encadenados (intermedios con opciones, finales con contador de alcances) y
los Lectores las leen hasta llegar a un final. El enunciado completo está en
`Obligatorio DW 082026.pdf` (raíz del repo) — léelo ahí para el detalle de cada RF,
reglas de negocio y entregables; no lo dupliques acá.

Fechas clave del enunciado: Entrega 1 (diagrama + solución sin código) 10/9 — ya
cumplida. Entrega 2 (RF01-RF03) 24/9. Entrega 3 (RF06-RF08) 15/10. Entrega 4
(RF10-RF12) 21/10. Entrega final 12/11.

**`RedSocialM3C/`** es un ejercicio de clase de otro tema (no de este obligatorio).
Se usó una sola vez como referencia de convenciones de carpetas/nombres (DTOs,
CasosDeUso, InterfacesDeCasoDeUso, etc.) para levantar la estructura propia. No lo
edites ni lo tomes como parte de este proyecto.

## Comandos

Todo se corre desde la raíz del repo, sobre `Obligatorio.slnx` (formato .slnx, no
.sln clásico):

```
dotnet build Obligatorio.slnx      # compila los 5 proyectos
dotnet run --project src/Presentacion   # levanta la app MVC de Administración
```

No hay tests todavía. Requiere .NET SDK 10 (`dotnet --version` → 10.x).

## Arquitectura

Clean Architecture / DDD, en español, con 5 proyectos bajo `src/` y esta cadena de
dependencias:

```
Dominio  <── AccesoDatos
   ↑             ↑
   └── DTOs  <── LogicaAplicacion
              ↑         ↑
              └── Presentacion ──┘
```

- **Dominio**: sin dependencias. `Entidades/` (Usuario, Categoria, Historia,
  Capitulo abstracta con CapituloIntermedio/CapituloFinal, Opcion, Lectura,
  Auditoria), `ValueObjects/` (Email, Password — password exige 8+ caracteres con
  mayúscula/minúscula/número/especial), `Enumerados/` (EstadoHistoria, RolUsuario),
  `Excepciones/DomainException`, `InterfacesDominio/IValidable` (todas las
  entidades y VOs se autovalidan con `Validar()`), `InterfacesRepositorios/` (una
  interfaz por raíz de agregado: Usuario, Categoria, Historia, Lectura, Auditoria).
  **Capitulo y Opcion no tienen repositorio propio** — se gestionan a través del
  agregado Historia (regla de negocio: no se comparten capítulos entre historias).
- **DTOs**: referencia solo a Dominio. `DTOs/DTOs/` (una clase plana por entidad,
  enums de Dominio expuestos directo) y `DTOs/Mappers/` (una clase estática
  `FromDTO`/`ToDTO` por entidad).
- **AccesoDatos**: referencia solo a Dominio. `Repositorios/` con una clase por
  cada `IRepositorioX` de Dominio. Todavía no usa EF Core — son stubs.
- **LogicaAplicacion**: referencia Dominio + DTOs. Por cada entidad con
  repositorio propio, carpetas `CasosDeUso/<Entidad>/` e
  `InterfacesDeCasoDeUso/<Entidad>/` con el patrón `Agregar<E>CU` /
  `Obtener<E>PorIdCU` / `EncontrarTodas<E>CU` (o `Todos` para Usuario, por
  género), cada uno inyectando el repositorio de Dominio correspondiente.
- **Presentacion**: Web MVC (`Microsoft.NET.Sdk.Web`), referencia las 4 capas.
  Un controller por entidad con repositorio propio, inyectando sus 3 casos de uso.
  `AuditoriaController` no tiene alta manual (RF05: la auditoría la genera el
  sistema automáticamente, no el administrador). `Program.cs` registra ahí todos
  los repositorios y casos de uso vía DI (`AddScoped`).

**Estado actual (actualizado 17/9): en desarrollo activo de la Entrega 2
(RF01-RF03, vence 24/9).** Ya no es un esqueleto vacío — ver sección
"Estado de avance" más abajo para el detalle de qué está hecho, qué falta y qué
decisiones se tomaron. Sigue habiendo stubs `throw new NotImplementedException()`
en las piezas de Historia/Categoria/Lectura/Auditoria que todavía no se
implementaron (RF03). Al implementar cada RF, reemplazar el stub
correspondiente — no rehacer la estructura.

### Arquitectura de la solución final (del enunciado, todavía no separado así)

El enunciado pide **dos soluciones .NET independientes**: una app de
Administración (Web MVC, RF01-RF05, sin Web API) y una app de Lectura (Web API con
JWT para RF01+RF06-RF11, más un MVC cliente que la consume por HttpClient para
RF12). Hoy todo vive en una sola solución (`Obligatorio.slnx`) porque solo se
implementó el lado de Administración. Cuando se ataque RF06+, evaluar si
`Presentacion` pasa a ser específicamente la app de Administración y se crean los
proyectos de Web API + MVC cliente de Lectura (misma arquitectura, otra solución).

### Decisiones de dominio no obvias

- `Historia.CapituloInicial` tipa `CapituloIntermedio` (no la `Capitulo` abstracta)
  — por regla de negocio el inicial nunca puede ser un final; el compilador lo
  garantiza en vez de una validación en runtime.
- `Historia` mantiene `List<Capitulo> Capitulos` — la relación con `Capitulo` es
  agregación (no composición); esto es una distinción de diagrama/UML, no cambia
  el código.
- `Auditoria` referencia opcionalmente `Capitulo` (`CapituloId` nullable) además
  de `Historia` y `Usuario` (administrador), para poder auditar altas de
  capítulos y no solo acciones a nivel Historia.
- `IRepositorio<T>` usa `void Add/Remove/Update` (no `bool`) y `T FindByID` sin
  `?` (no nullable) — así lo dio la cátedra en clase, distinto de lo que suele
  verse en el teórico. No revertir a `bool`/`T?` aunque parezca más "correcto".

## Estado de avance (actualizado 17/9/2026)

Contexto para retomar: esta sección se actualiza a medida que se avanza, para que
una sesión nueva sepa exactamente dónde se quedó sin tener que releer todo el
historial de git ni el chat.

### Entity Framework Core (base para todo lo demás)

Un compañero (Bruno) agregó en clase la base de EF Core: `HistoriasContext`
(en `src/AccesoDatos/RepositorioEntityFramework/`), la primera migración, y los
esqueletos de `RepositorioXEF`. Quedó "a medio conectar" (commit
"Tabla y vista creada, falta conexión con BD"). En esta sesión se terminó de
conectar y se corrigieron varios bugs de mapeo que tenía el modelo:

- `Email` y `Password` (`src/Dominio/ValueObjects/`) pasaron de `class` con
  propiedad `get`-only a `record` con `init`. Con `get`-only, EF Core no lograba
  mapear esas propiedades y la tabla `Usuarios` quedaba **sin columnas** de email
  ni contraseña (bug real, verificado con `sqlcmd`). El patrón `record` + `init`
  es el mismo que usa el profe en el proyecto de referencia `RedSocialM3C`
  (ver más abajo).
- `HistoriasContext.OnModelCreating` (nuevo, no existía) configura a mano: la FK
  de `Historia.CapituloInicial` (por defecto EF generaba una columna fantasma
  duplicada), la relación `Historia`↔`Categoria` como muchos-a-muchos (por
  defecto EF la armaba uno-a-muchos, lo cual rompía la posibilidad de reusar una
  categoría entre varias historias), y la obligatoriedad de
  `CapituloIntermedio.Opciones`. También fija `OnDelete(Restrict)` en un par de
  relaciones para evitar el error de SQL Server por "multiple cascade paths"
  hacia la tabla `Capitulos` (TPH compartida por `CapituloIntermedio`/`CapituloFinal`).
- Los 5 `RepositorioXEF` (`Usuario`, `Categoria`, `Historia`, `Lectura`,
  `Auditoria`) están implementados con LINQ método (`Where`/`FirstOrDefault`/
  `ToList`), instanciando su propio `HistoriasContext` en el constructor (mismo
  patrón que el profe, sin inyectar el `DbContext` por DI).
- `Program.cs` ya inyecta los repos EF (`RepositorioXEF`), no los de memoria
  (`RepositorioEnMemoria/`, que quedaron sin usar pero no se borraron).
- La migración vieja (`20260910115250_init`, la del modelo roto) se borró y se
  regeneró limpia como `Init`. Se aplicó a LocalDB (`(localdb)\MSSQLLocalDB`,
  base `HistoriasDB`) con `dotnet ef database update`.
- Se instaló `dotnet-ef` como herramienta local (`dotnet-tools.json` en la raíz,
  generado con `dotnet new tool-manifest` + `dotnet tool install dotnet-ef`).
  **Ojo: el usuario decidió no commitear esto todavía**, lo quiere probar en su
  PC primero. Si en una sesión futura `dotnet-tools.json` sigue sin trackear en
  git, es a propósito, no un olvido — no lo agregues a un commit sin que lo pida.

Para levantar la base de datos desde cero en una PC nueva:
```
dotnet tool restore
dotnet ef database update --project src/AccesoDatos/AccesoDatos.csproj
```

### RF02 — Alta de usuarios: hecho y probado

`UsuarioMapper`, `AgregarUsuarioCU` (valida nombre de usuario duplicado antes de
guardar — no está explícito en la letra pero es necesario para que el login
identifique un único usuario) y `ObtenerUsuarioPorIdCU` están implementados.
`UsuarioController.Create` ahora muestra el mensaje de `DomainException` en vez
de tragárselo en un `catch` vacío. Se agregó la vista `Create.cshtml` que no
existía (el botón "Create New" del scaffold rompía). Se sacó la columna de
contraseña en texto plano de `Index.cshtml`. Falta Editar/Eliminar/Detalles de
Usuario — no están pedidos por RF02 (que es solo alta), quedan fuera de alcance
por ahora.

Hay un usuario real cargado en la base para pruebas: `admin1` / `Admin123!`,
rol Administrador.

### RF01 — Login de administradores: funcional, falta la protección de rutas

**Decisión importante:** se usa **Session** (`HttpContext.Session`), NO Cookie
Authentication ni `[Authorize]`. La letra de RF01 no impone tecnología (dice
"la autenticación se resuelve dentro de la misma aplicación MVC"; JWT está
explícitamente reservado para la app de Lectura, RF06+). El usuario ya usó
Session el semestre pasado y todavía no se vio Cookie Authentication en el
material de la cátedra (`RedSocialM3C`), así que se optó por lo conocido.
Implicancia: **no se puede usar `[Authorize]`** en ningún controller — cualquier
protección de rutas tiene que ser manual.

**Cambio de enfoque (17/9, hecho en clase):** el login ya NO vive en un
`LoginController` separado — quedó fusionado directo en `UsuarioController`
(`Login` GET/POST, `Logout`) + `Views/Usuario/Login.cshtml`. La versión anterior
con `LoginController`/`Views/Login/` (de una sesión previa en esta PC) se
descartó sin commitear al pullear estos cambios — nunca llegó a subirse, así que
no hay que buscarla en el historial.

Lo que ya está:
- `Program.cs`: `AddDistributedMemoryCache()` + `AddSession()` + `app.UseSession()`.
  La ruta default cambió de `Usuario/Index` a **`Usuario/Login`** — la app ahora
  arranca en el login en vez del listado de usuarios.
- `IIniciarSesion` / `IniciarSesionCU` (`LogicaAplicacion/CasosDeUso/Usuarios/`):
  busca por `NombreUsuario`, compara `Password.Valor` tal cual (sin hash — no
  está pedido, y el proyecto de referencia tampoco lo hace), y rechaza si el rol
  no es Administrador.
- `UsuarioController.Login` (GET/POST) + `Logout`, con `Views/Usuario/Login.cshtml`.
  Guarda en Session: `UsuarioId`, `NombreUsuario`, `Rol`. Probado por HTTP y en
  el navegador: credenciales malas → mensaje de error; buenas → redirige a
  `Usuario/Index`; logout → limpia Session y vuelve a `Usuario/Login`.

**Lo que falta, y quedó pausado a pedido del usuario** (para ver primero cómo lo
dan en clase, puede cambiar el enfoque): proteger el resto de los controllers
para que no se pueda entrar sin loguearse. Se propuso un Filtro de Acción
(`IActionFilter`/`ActionFilterAttribute`) tipo `[RequiereSesion]` que se pondría
sobre `UsuarioController` (menos `Login`/`Logout`), `HistoriaController`, etc.
Todavía **no se implementó nada de esto** — es la próxima pieza cuando se retome.
Hoy en día, sin este filtro, entrar a `/Usuario/Index` o cualquier otra ruta sin
loguearse funciona igual (no está protegido).

### RF03 — Gestión de historias: no arrancado

Sigue con los stubs `NotImplementedException` de la Entrega 1. No se tocó nada
todavía.

### Otras notas sueltas de esta sesión

- El repo de clase con los cambios de EF del profesor está en
  `https://github.com/Prog-DW-2026-2/GrupoM3CAg2026.git`. La copia local en
  `RedSocialM3C/` (que **no se commitea a este repo**, ver más abajo) se
  sincronizó manualmente desde ahí una vez (13/9). Si hace falta volver a
  actualizarla, clonar ese repo aparte y copiar la carpeta
  `RedSocialM3C/RedSocialM3C` (excluyendo `obj/`, `bin/`, `.vs/`) — no es un
  submódulo git, es una copia plana.
- `Obligatorio DW 082026.pdf` fue borrado del repo por un commit del compañero
  (`8d4f342`). Sigue recuperable del historial
  (`git show 4154fbc:"Obligatorio DW 082026.pdf"`) pero **no se restauró** — el
  usuario todavía no decidió si quiere reagregarlo. No asumir que existe en el
  working tree sin comprobarlo.
- `Obligatorio-P3-BF366724-EA293522.zip` (42MB) quedó commiteado por error en
  `8d4f342`. El usuario pidió explícitamente dejarlo así por ahora, no tocarlo.
- LocalDB: la instancia `MSSQLLocalDB` puede no estar iniciada en una sesión
  nueva de Windows. Si algo falla al conectar, correr
  `sqllocaldb start MSSQLLocalDB` primero.

## Convenciones de Git

- **Nunca agregar `Co-Authored-By: Claude` ni `Claude-Session` en los mensajes
  de commit.** Es una entrega académica y el usuario pidió explícitamente que
  no quede rastro de Claude en la autoría de los commits — solo debe figurar
  el usuario (Vorhaan) como author/committer.
- **`CLAUDE.md` SÍ se sube al repo** (decisión del 17/9: se commitea a propósito
  para que el compañero lo tenga como contexto y se lo pueda pasar a su propia
  IA al arrancar RF03). Antes estaba en `.gitignore` porque se pensaba que el
  profesor no debía verlo — eso se descartó. Si en el futuro se quiere volver a
  ocultarlo, hay que agregarlo de nuevo a `.gitignore` y sacarlo con
  `git rm --cached CLAUDE.md`.
- Se trabaja solo (sin compañeros todavía), así que se empuja directo a
  `master` sin flujo de PR — no crear ramas/PRs de más por costumbre.
- `RedSocialM3C/` y cualquier archivo suelto ajeno al obligatorio (fotos,
  material de teórico) quedan fuera de los commits de este repo.
