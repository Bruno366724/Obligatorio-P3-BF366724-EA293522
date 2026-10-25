# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Obligatorio de Programación 3 (ORT) — "Escoge tu propia aventura": una plataforma de
lectura interactiva donde los Administradores arman historias compuestas por
capítulos encadenados (intermedios con opciones, finales con contador de alcances) y
los Lectores las leen hasta llegar a un final. El enunciado completo está en
`Obligatorio DW 082026.pdf`, pero **no vive en el working tree** (se borró en un
commit, ver "Otras notas sueltas" más abajo) — recuperalo del historial con
`git show 4154fbc:"Obligatorio DW 082026.pdf"` antes de citarlo, no asumas que
existe en disco. Léelo ahí para el detalle de cada RF, reglas de negocio y
entregables; no lo dupliques acá.

Fechas clave del enunciado: Entrega 1 (diagrama + solución sin código) 10/9 — ya
cumplida. Entrega 2 (RF01-RF03) 24/9 — cumplida. Entrega 3 (RF06-RF08) 15/10 —
cumplida. Entrega 4 (RF10-RF12) 21/10. Entrega final 12/11.

**`RedSocialM3C/`** es el ejercicio de clase del profe (otro tema, no de este
obligatorio). Se sincroniza de tanto en tanto desde
`https://github.com/Prog-DW-2026-2/GrupoM3CAg2026.git` (no es submódulo, es una copia
plana — ver "Otras notas sueltas") y se usa como referencia de convenciones (DTOs,
CasosDeUso, patrón repositorio, y ahora también Web API + JWT) para alinear la
estructura propia. No lo edites ni lo tomes como parte de este proyecto.

## Comandos

Todo se corre desde la raíz del repo, sobre `Obligatorio.slnx` (formato .slnx, no
.sln clásico):

```
dotnet build Obligatorio.slnx           # compila los 7 proyectos
dotnet run --project src/Presentacion   # app MVC de Administración (puertos 5241/7047)
dotnet run --project src/WebApiLectura  # Web API de Lectura, con Swagger (5242/7048)
dotnet run --project src/MvcLectura     # app MVC cliente de Lectura (5243/7049)
```

Para correr las tres juntas desde Visual Studio: click derecho en la solución →
"Configurar proyectos de inicio..." → "Varios proyectos de inicio" → Acción = Inicio
en `Presentacion`, `WebApiLectura` y `MvcLectura`.

No hay tests todavía. Requiere .NET SDK 10 (`dotnet --version` → 10.x).

Para cargar datos mínimos de prueba (un Administrador, un Lector, categorías, e
historias en Borrador/Publicada) en una base recién creada:
```
sqlcmd -S "(localdb)\MSSQLLocalDB" -d HistoriasDB -f 65001 -i scripts\datos-prueba-completo.sql
```
Es idempotente (se puede correr varias veces sin duplicar nada). Reemplaza a los dos
scripts sueltos anteriores (`datos-prueba-rf03.sql`, `datos-prueba-historia-completa.sql`,
que siguen en el repo pero ya no hace falta correrlos a mano).

## Arquitectura

Clean Architecture / DDD, en español, con 7 proyectos bajo `src/`. El lado de
Administración y el lado de Lectura comparten `Dominio`/`DTOs`/`AccesoDatos`/
`LogicaAplicacion`, pero tienen presentaciones separadas:

```
Dominio  <── AccesoDatos
   ↑             ↑
   └── DTOs  <── LogicaAplicacion
              ↑         ↑         ↑
              └── Presentacion   WebApiLectura ──┐
                  (Admin, MVC)   (Web API, JWT)  │
                                                  │
                                    DTOs  <── MvcLectura
                                              (cliente, consume
                                               WebApiLectura por
                                               HttpClient)
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
  `FromDTO`/`ToDTO` por entidad). `CapituloDTO` tiene `[JsonPolymorphic]`/
  `[JsonDerivedType]` (ver "Decisiones de dominio no obvias") para que `System.Text.Json`
  pueda serializar/deserializar `CapituloIntermedioDTO`/`CapituloFinalDTO` — lo necesita
  tanto `WebApiLectura` como `MvcLectura`, que referencia este proyecto directo.
- **AccesoDatos**: referencia solo a Dominio. Usa EF Core de verdad:
  `RepositorioEntityFramework/` (`HistoriasContext` + un `RepositorioXEF` por
  cada `IRepositorioX` de Dominio). `HistoriasContext` se registra con
  `AddDbContext` en el `Program.cs` de `Presentacion` y de `WebApiLectura` (cadena de
  conexión `"ConnectionStrings:Historias"` en su `appsettings.json`) y cada repo lo
  recibe por constructor — mismo patrón que `RedSocialM3C`. Al ser scoped, todos los
  repos de un mismo pedido HTTP comparten el contexto. `RepositorioEnMemoria/`
  quedó de una etapa anterior, sin usar pero sin borrar — `Program.cs` inyecta
  los `XEF`, no estos.
- **LogicaAplicacion**: referencia Dominio + DTOs. Por cada entidad con
  repositorio propio, carpetas `CasosDeUso/<Entidad>/` e
  `InterfacesDeCasoDeUso/<Entidad>/` con el patrón `Agregar<E>CU` /
  `Obtener<E>PorIdCU` / `EncontrarTodas<E>CU` (o `Todos` para Usuario, por
  género), cada uno inyectando el repositorio de Dominio correspondiente.
  Casos de uso fuera de ese patrón base se agregan igual (uno por acción,
  interfaz propia) cuando el RF lo pide. Un caso de uso puede inyectar más de un
  repositorio/caso de uso cuando coordina algo entre agregados (ej.
  `AgregarHistoriaCU` inyecta `IAgregarAuditoria` además de `IRepositorioHistoria`
  para auditar; `IniciarLecturaCU` inyecta `IRepositorioLectura` e
  `IRepositorioHistoria`).
- **Presentacion**: Web MVC de **Administración** (`Microsoft.NET.Sdk.Web`),
  referencia las 4 capas de abajo. Un controller por entidad con repositorio propio,
  inyectando sus casos de uso. RF01-RF05. `AuditoriaController` no tiene alta manual
  (la auditoría la genera el sistema automáticamente). `Program.cs` registra ahí
  todos los repositorios y casos de uso vía DI (`AddScoped`). Rutas protegidas con
  `Filtros/RequiereSesionAttribute` (chequea `Session["UsuarioId"]`), **no**
  `[Authorize]` (ver RF01 más abajo).
- **WebApiLectura**: Web API (`Microsoft.NET.Sdk.Web`), mismas 4 capas de abajo que
  Presentacion. RF06-RF08 (y, a futuro, RF09-RF11). Login con JWT
  (`Controllers/AuthController` + `TokenHandler/JWTHandler` estático, recibe
  `DTOs/LoginDTO`, devuelve `{ Token, Usuario }`), configurado en `Program.cs` con el
  paquete oficial `Microsoft.AspNetCore.Authentication.JwtBearer` — solo la clave en
  `appsettings.json` (`"Jwt:Key"`), sin validar emisor/audiencia, igual que
  `RedSocialM3C`. Swagger con soporte de
  Bearer token (botón "Authorize"). Controllers protegidos con `[Authorize]`
  (cualquier rol) o `[Authorize(Roles = "Administrador")]`/`[Authorize(Roles = "Lector")]`
  según corresponda — el `UsuarioId` siempre sale del claim del token
  (`ClaimTypes.NameIdentifier`), nunca de parámetros que mande el cliente.
- **MvcLectura**: Web MVC **cliente** (`dotnet new mvc`), referencia **solo `DTOs`**
  (no Dominio/AccesoDatos/LogicaAplicacion — "exclusivamente presentación", consume
  `WebApiLectura` por `HttpClient` con nombre, URL base en `appsettings.json`
  `"WebApiLectura:BaseUrl"`). El token JWT se guarda en `Session` (mismo mecanismo que
  RF01, pero acá "estar logueado" es tener token, no `UsuarioId`). Rutas protegidas
  con `Filtros/RequiereTokenAttribute`, puesto en `Controllers/ControladorBase`
  (clase abstracta que heredan `HistoriaController`/`LecturaController`, con un
  `CrearClienteAutenticado()` que arma el `HttpClient` con el header `Authorization`
  ya puesto). `AuthController` (Login con `LoginDTO` + vista tipada, Logout).
  RF06-RF08 del lado del cliente.

**Estado actual (actualizado 10/10): RF01-RF08 funcionalmente completos** (Entregas
2 y 3 cumplidas) **salvo el Delete de RF03** (salteado a pedido de la cátedra). Ver
sección "Estado de avance" más abajo para el resumen por RF, y
`notas-de-avance.md` (no commiteado, personal) para el detalle completo de bugs
encontrados y por qué se tomó cada decisión — ese documento es más granular que esta
sección y se actualiza a medida que se cierra cada RF. Categoria/Lectura siguen con
algunos stubs `NotImplementedException` fuera de lo que ya pide RF06-RF08 (ej.
`AgregarCategoriaCU`/`ObtenerCategoriaPorIdCU` no están pedidos por ningún RF
todavía). Al implementar cada RF, reemplazar el stub correspondiente — no rehacer la
estructura.

### Arquitectura de la solución final (del enunciado)

El enunciado pide **dos soluciones .NET independientes**: una app de Administración
(Web MVC, RF01-RF05, sin Web API) y una app de Lectura (Web API con JWT para
RF01+RF06-RF11, más un MVC cliente que la consume por HttpClient para RF12). Hoy
los tres proyectos (`Presentacion`, `WebApiLectura`, `MvcLectura`) ya existen y
funcionan con esa separación de responsabilidades, pero **todavía viven en una sola
solución** (`Obligatorio.slnx`) — separarlos en dos `.slnx`/carpetas distintas queda
pendiente, no es parte de ningún RF puntual, se puede dejar para más cerca de la
entrega final.

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
- `Lectura.CapituloActualId`/`CapituloActual` (agregado en RF07): no está en la
  letra del enunciado como campo de `Lectura`, pero hace falta — la Web API no
  tiene sesión del lado del servidor, así que entre un pedido HTTP y el siguiente
  ("avanzar, elegir una opción") no hay otro lugar donde guardar en qué capítulo
  va el lector. Nace en el capítulo inicial de la historia y se mueve con cada
  avance. Dos FKs a `Capitulo` desde `Lectura` (`CapituloActual` +
  `FinalAlcanzado`) están configuradas en `Restrict` en `HistoriasContext`, mismo
  motivo que el punto siguiente.
- `IRepositorio<T>` usa `void Add/Remove/Update` (no `bool`) y `T FindByID` sin
  `?` (no nullable) — así lo dio la cátedra en clase, distinto de lo que suele
  verse en el teórico. No revertir a `bool`/`T?` aunque parezca más "correcto".
- **Excepciones propias por entidad**, no una `DomainException` genérica para
  todo: `UsuarioException`, `CategoriaException`, `HistoriaException`,
  `LecturaException`, `AuditoriaException` (`Dominio/Excepciones/`), todas
  heredando de `DomainException` (que sigue existiendo como base común, ya no
  se instancia directo). `Capitulo`/`Opcion` usan `HistoriaException` (no tienen
  repo propio, son del agregado Historia).
- `Historia.CapituloInicialId` es `int?` (no `int`), a propósito: `Capitulo`
  necesita `HistoriaId` para insertarse y `Historia` necesita
  `CapituloInicialId` para insertarse — ciclo de FKs si las dos son `NOT NULL`
  y ambas filas son nuevas en el mismo `SaveChanges()`. Por eso
  `RepositorioHistoriaEF.Add` guarda en dos pasos (Historia+Capitulo sin el
  link, después completa `CapituloInicialId`). No volver a `int` sin resolver
  esto de otra forma.
- `RepositorioHistoriaEF` tiene un método privado `ResolverCategorias` que usa
  `contexto.Categorias.Where(c => ids.Contains(c.Id)).ToList()` en vez de
  `contexto.Attach(...)` antes de `Add`/`Update`. Es a propósito: `Attach` con
  una `Categoria` recién armada (solo con `Id`) pisaba el `Nombre` real o
  chocaba con una instancia ya trackeada por el mismo contexto (por el
  `Include` de `FindByID`). La consulta LINQ resuelve ambos casos sola.
- `RepositorioHistoriaEF.ConDetalle()` necesita `.Include("Capitulos.Opciones")`
  **por string** (no `.Include(h => h.Capitulos).ThenInclude(...)` tipado) porque
  `Opciones` solo existe en `CapituloIntermedio`, no en la `Capitulo` base (TPH).
  Sin esto, `Capitulos.Opciones` siempre volvía vacío en memoria aunque la base
  tuviera filas, y `Historia.ValidarParaPublicar()` fallaba siempre con "un
  capítulo intermedio debe tener al menos una opción" — hasta a historias que sí
  la tenían. Mismo patrón (`.Include("CapituloActual.Opciones")` +
  `.Include("CapituloActual.Opciones.CapituloDestino")`) en
  `RepositorioLecturaEF.ConDetalle()`, necesario para que `AvanzarLecturaCU` pueda
  validar la opción elegida y saber a qué capítulo lleva sin una consulta aparte.
- `CapituloDTO` tiene `[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipo")]` +
  `[JsonDerivedType(...)]` (`DTOs/DTOs/CapituloDTO.cs`). Sin esto, `System.Text.Json`
  no solo serializa mal un tipo abstracto (pierde `Opciones`/`ContadorAlcances`),
  directamente **no puede deserializarlo** — tira `NotSupportedException` apenas un
  cliente (como `MvcLectura`) intenta leer una respuesta de la Web API que incluya
  capítulos. El campo `"tipo"` nuevo en el JSON (`"intermedio"`/`"final"`) le dice
  al deserializador qué subtipo armar.
- **Convención de código: evitar `var`**, usar el tipo explícito salvo que no
  haya otra opción (tipos anónimos, etc.) — pedido explícito del usuario,
  aplicado en todo el proyecto (no solo en código nuevo).
- **Comentarios en el código: una línea, qué hace, no por qué** — no reproducir
  ahí el razonamiento/la historia de cómo se llegó a una decisión (eso va en
  `notas-de-avance.md`, no en el código fuente). Aplica a todo el proyecto.

## Estado de avance (actualizado 10/10/2026)

Contexto para retomar: esta sección se actualiza a medida que se avanza, para que
una sesión nueva sepa exactamente dónde se quedó sin tener que releer todo el
historial de git ni el chat. Para el detalle fino de cada bug encontrado y cada
decisión de diseño (con el *por qué*, no solo el *qué*), ver `notas-de-avance.md`
(no commiteado — documento personal para repasar antes de la defensa, pero vale la
pena abrirlo si hace falta contexto que esta sección resume de más).

### RF01 — Login de administradores: completo

Session (`HttpContext.Session`), no Cookie Authentication ni `[Authorize]` — la
letra no impone tecnología y JWT está reservado para RF06+. Login/Logout viven en
`UsuarioController` (no un `LoginController` separado). Protección de rutas:
`Filtros/RequiereSesionAttribute` (`ActionFilterAttribute`, chequea
`Session["UsuarioId"]`, redirige a `Usuario/Login` si no hay), puesto a nivel de
clase en `Historia`/`Categoria`/`Lectura`/`AuditoriaController`, y método por método
en `UsuarioController` (menos `Login`/`Logout`, para no bloquear el login mismo).

### RF02 — Alta de usuarios: completo

`AgregarUsuarioCU` valida nombre de usuario duplicado (no explícito en la letra,
pero necesario para que el login identifique un único usuario). Falta
Editar/Eliminar/Detalles — no pedidos por RF02. Usuario de prueba real:
`admin1` / `Admin123!` (Administrador). También hay `lector1` / `Lector123!`
(Lector) desde el script de datos mínimos.

### RF03 — Gestión de historias: completo (menos Delete)

CRUD sin Delete (salteado a pedido de la cátedra). Ver `notas-rf03-para-defensa.md`
para el detalle de mappers/excepciones/caso de uso de creación.

### RF04 — Alta de capítulos: completo

`AgregarCapituloCU` (`LogicaAplicacion/CasosDeUso/Historias/`, no tiene carpeta
propia porque Capítulo no tiene repo). Recibe parámetros sueltos (historiaId,
capituloAntecedenteId, esFinal, título, texto, textoOpcion), valida que el
antecedente sea intermedio y pertenezca a la misma historia, crea el capítulo nuevo
y la opción que lo enlaza desde el antecedente. El capítulo intermedio recién creado
no se valida completo (nace con 0 opciones) — mismo criterio que el capítulo inicial
de `AgregarHistoriaCU`. UI: botón "Agregar capítulo siguiente" en
`Historia/Details.cshtml` (antes placeholder deshabilitado) → `HistoriaController.AgregarCapitulo`.

### RF05 — Auditoría de historias: completo

Automática: `AgregarHistoriaCU`, `EditarHistoriaCU` y `AgregarCapituloCU` reciben
`administradorId` (sacado de `Session` en `HistoriaController`) e inyectan
`IAgregarAuditoria` además de su repo de siempre, registrando la auditoría al
terminar su trabajo. Acciones etiquetadas: `"Creación de historia"`, `"Edición de
historia"`, `"Publicación de historia"` (distinta de edición: solo si el estado pasa
a Publicada *en esa llamada*), `"Alta de capítulo"` (el único caso que completa
`CapituloId`). `AuditoriaController` sin alta manual ni Edit/Delete; `Index` filtra
opcionalmente por historia o administrador (`EncontrarAuditoriasPorHistoriaCU`/
`PorAdministradorCU`).

### RF06 — Login de usuarios (Web API + JWT): completo

`IniciarSesionLecturaCU` (separado de `IniciarSesionCU` de RF01: sin restricción de
rol, entran Administrador y Lector). JWT con el paquete oficial
`Microsoft.AspNetCore.Authentication.JwtBearer`, generado en
`WebApiLectura/TokenHandler/JWTHandler.cs` (3 claims: `NameIdentifier`,
`Name`, `Role`) y expuesto en `POST /api/Auth/login`. Swagger configurado
con `Type = Http, Scheme = "bearer"` (no `ApiKey`) para que el botón "Authorize"
agregue el prefijo `Bearer ` solo — con `ApiKey` hay que escribirlo a mano y es
fácil de pifiar (401 sin explicación aparente).

### RF07 — Lectura de historia: completo (API + cliente MVC)

- `EncontrarHistoriasPublicadasCU`: filtra `Estado == Publicada` + categoría
  opcional.
- `IniciarLecturaCU`: valida que la historia esté Publicada, arranca la lectura en
  su capítulo inicial.
- `AvanzarLecturaCU`: valida pertenencia (la lectura tiene que ser del usuario que
  la pide), que no haya terminado, que la opción elegida sea del capítulo actual;
  si el destino es un final, cierra la lectura y llama `RegistrarAlcance()` sobre
  el capítulo (un solo `Update(lectura)`/`SaveChanges()` persiste los dos cambios,
  porque el capítulo final ya viene trackeado por el mismo `DbContext`).
- Endpoints en `WebApiLectura/Controllers/LecturaController`
  (`[Authorize(Roles = "Lector")]`): `Iniciar`, `{id}/Avanzar`, `{id}` (GET, con
  chequeo de pertenencia en el controller — el CU `IObtenerLecturaPorId` es
  genérico y también lo usa `Presentacion` sin esa noción).
- Cliente MVC (`MvcLectura`): `HistoriaController` (listado + detalle, con botón
  "Comenzar a leer") y `LecturaController` (`Iniciar`/`Leer`/`Avanzar`). Como
  `LecturaDTO` solo trae `CapituloActualId`, la pantalla de lectura pide también la
  `Historia` completa y busca el capítulo por Id (`LeerViewModel`). Vista
  `Leer.cshtml`: texto del capítulo + un botón por opción si es intermedio, mensaje
  de final alcanzado si la lectura terminó.

### RF08 — Historial de lecturas: completo (API + cliente MVC)

`EncontrarHistorialLecturasCU` usa `FindByUsuarioYPeriodo` (ya existía en el repo).
Endpoint `GET /api/Lectura/Historial?mes=&anio=` — el `UsuarioId` sale del token,
nunca de la URL. Lista vacía con 200 si no hay lecturas ese mes (el "mensaje
descriptivo" que pide el enunciado es cosa de la UI, no de la API). Cliente MVC:
`LecturaController.Historial` arma un `HistorialItemViewModel` por lectura
(resolviendo título de historia y de final alcanzado, pidiendo cada `Historia`),
con selector de mes/año y el mensaje cuando no hay resultados.

### Pendiente

- RF09-RF11 (rankings/métricas, lado Administrador — van también en `WebApiLectura`
  + `MvcLectura`, no en `Presentacion`, según el enunciado).
- RF12 (estilos de lectura) y RF05 de la segunda tanda del enunciado (investigación
  con IA, documentar aparte) — necesitan que `MvcLectura` ya tenga pantallas para
  aplicarles el tema.
- Separar la solución en dos `.slnx` (Administración / Lectura), si se decide
  hacerlo antes de la entrega final.
- Categoria sigue con stubs fuera de `EncontrarTodasCategoriasCU`.

## EF Core / base de datos

Cadena de conexión en `appsettings.json` de `Presentacion` y de `WebApiLectura`
(`"ConnectionStrings:Historias"`, `(localdb)\MsSqlLocalDb`, base `HistoriasDB`),
registrada con `AddDbContext` (ver AccesoDatos arriba). `dotnet-ef` como herramienta local
(`dotnet-tools.json`, generado con `dotnet new tool-manifest` — **no commiteado
todavía a propósito**, el usuario lo quiere probar primero en su PC).

Migraciones aplicadas (orden): `Init`, `CapituloInicialIdNullable`,
`AgregarCapituloActualALectura` (esta última agrega `Lectura.CapituloActualId`,
RF07). Para levantar la base desde cero en una PC nueva:
```
dotnet tool restore
dotnet ef database update --project src/AccesoDatos/AccesoDatos.csproj --startup-project src/Presentacion/Presentacion.csproj
```
El `--startup-project` es obligatorio desde que el contexto se configura por DI: sin
él, `dotnet ef` no encuentra la cadena de conexión ("Unable to create a 'DbContext'").
Mismo flag para `dotnet ef migrations add <Nombre>`.
Y después cargar datos de prueba con `scripts/datos-prueba-completo.sql` (ver
"Comandos" arriba).

Dos bugs de mapeo relevantes, con el detalle completo en "Decisiones de dominio no
obvias": el ciclo de FKs `Historia`↔`CapituloInicial` (resuelto con guardado en dos
pasos) y el `Include` faltante de `Capitulos.Opciones` (rompía la publicación de
historias, encontrado recién en RF05 al probar un flujo completo contra la base
real — moraleja que se repitió varias veces esta sesión: conviene probar contra
LocalDB de verdad, no solo que compile, ni bien algo encadena varias entidades).

## Convenciones de Git

- **Nunca agregar `Co-Authored-By: Claude` ni `Claude-Session` en los mensajes
  de commit.** Es una entrega académica y el usuario pidió explícitamente que
  no quede rastro de Claude en la autoría de los commits — solo debe figurar
  el usuario (Vorhaan) como author/committer.
- **`CLAUDE.md` SÍ se sube al repo** (decisión del 17/9: se commitea a propósito
  para que el compañero lo tenga como contexto y se lo pueda pasar a su propia
  IA).
- **Notas personales sueltas, en la raíz del repo, nunca commiteadas**:
  `notas-rf03-para-defensa.md`, `notas-de-avance.md` (documento vivo, se actualiza
  al cerrar cada RF — bugs encontrados, decisiones, con el *por qué*),
  `preguntas-para-el-profe.md` (dudas para la próxima clase, ej. la de
  polimorfismo en JSON, ya resuelta por nuestra cuenta pero documentada ahí por si
  el profe muestra otra forma). Mismo criterio para cualquier nota nueva de este
  tipo: se crea suelta en la raíz, no se agrega al commit salvo que se pida
  explícitamente.
- Hay compañero (Bruno) trabajando en el mismo repo, directo a `master`. El
  trabajo de RF04-RF08 de esta sesión (y el de RF03 de una sesión anterior) se
  pusheó a **`feature/estructura-proyecto`** (rama del usuario), no a `master` —
  Bruno iba a intentar RF03 por su cuenta para comparar, y después `master` y
  `feature/estructura-proyecto` se reconciliaron solos (Bruno mergeó `feature`
  a `master` en algún punto, confirmado viendo que `1872d8c` — el último commit
  de aquella sesión de RF03 — es ancestro de `origin/master`). Antes de pushear
  a `master` directo, confirmar con el usuario; a `feature/estructura-proyecto`
  se puede pushear sin preguntar, es la rama de trabajo del usuario.
- `git clone` de este repo deja parado en `master` (rama default), aunque
  `feature/estructura-proyecto` y otras ramas ya estén disponibles como
  `origin/<rama>` — hace falta `git checkout feature/estructura-proyecto`
  después de clonar. No es un problema del repo, es el comportamiento normal de
  `git clone`.
- `RedSocialM3C/` y cualquier archivo suelto ajeno al obligatorio (fotos,
  material de teórico) quedan fuera de los commits de este repo.
- `Obligatorio-P3-BF366724-EA293522.zip` (42MB) quedó commiteado por error hace
  tiempo, el usuario pidió dejarlo así por ahora, no tocarlo.

## Otras notas sueltas

- `Obligatorio DW 082026.pdf` fue borrado del repo por un commit del compañero
  (`8d4f342`). Sigue recuperable del historial
  (`git show 4154fbc:"Obligatorio DW 082026.pdf"`) pero **no se restauró** — el
  usuario todavía no decidió si quiere reagregarlo.
- LocalDB: la instancia `MSSQLLocalDB` puede no estar iniciada en una sesión
  nueva de Windows. Si algo falla al conectar, correr
  `sqllocaldb start MSSQLLocalDB` primero.
- Si Visual Studio tira un error raro de arranque tipo "An element with the same
  key but a different value already exists. Key:
  Microsoft.WebTools.ProjectSystem.WebServer.SelfHostWebServer" — es un bug
  conocido del caché de componentes (MEF) de VS, no del código. Se arregla
  cerrando VS del todo y borrando
  `%LOCALAPPDATA%\Microsoft\VisualStudio\<version>\ComponentModelCache` (+
  `MEFCacheBackup`) y la carpeta `.vs\` de la solución; al reabrir, VS
  reconstruye el caché solo. Pasa más seguido después de editar muchos archivos
  por fuera del IDE en una misma sesión.
