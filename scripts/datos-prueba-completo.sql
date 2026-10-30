-- Datos de prueba completos, idempotente (se puede correr varias veces sin duplicar nada).
-- Usuarios (2 administradores + 8 lectores), 10 categorias, historias en Borrador y
-- Publicadas, lecturas completadas y en curso, y auditoria de las historias.
-- Ejecutar con: sqlcmd -S "(localdb)\MSSQLLocalDB" -d HistoriasDB -f 65001 -b -i scripts\datos-prueba-completo.sql

SET NOCOUNT ON;

-- ============================================================
-- 0) Limpieza de los usuarios de prueba viejos (admin1 / lector1)
-- ============================================================

DELETE FROM Auditorias WHERE AdministradorId IN (SELECT Id FROM Usuarios WHERE NombreUsuario IN (N'admin1', N'lector1'));
DELETE FROM Lecturas WHERE UsuarioId IN (SELECT Id FROM Usuarios WHERE NombreUsuario IN (N'admin1', N'lector1'));
DELETE FROM Usuarios WHERE NombreUsuario IN (N'admin1', N'lector1');

-- ============================================================
-- 1) Usuarios (RF02/RF01/RF06) -- Rol: 0 = Administrador, 1 = Lector
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'edney')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Edney Araujo', N'edney@obligatorio.com', N'edney', N'Edney123!', 0);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'bruno')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Bruno', N'bruno@obligatorio.com', N'bruno', N'Bruno123!', 0);

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'lfernandez')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Lucía Fernández', N'lucia.fernandez@correo.com', N'lfernandez', N'Lucia2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'mrodriguez')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Martín Rodríguez', N'martin.rodriguez@correo.com', N'mrodriguez', N'Martin2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'spereira')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Sofía Pereira', N'sofia.pereira@correo.com', N'spereira', N'Sofia2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'dsilva')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Diego Silva', N'diego.silva@correo.com', N'dsilva', N'Diego2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'vgomez')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Valentina Gómez', N'valentina.gomez@correo.com', N'vgomez', N'Valentina2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'jcastro')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Joaquín Castro', N'joaquin.castro@correo.com', N'jcastro', N'Joaquin2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'csuarez')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Camila Suárez', N'camila.suarez@correo.com', N'csuarez', N'Camila2026!', 1);
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'nmendez')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Nicolás Méndez', N'nicolas.mendez@correo.com', N'nmendez', N'Nicolas2026!', 1);

-- ============================================================
-- 2) Categorias (RF03)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Terror')
    INSERT INTO Categorias (Nombre) VALUES (N'Terror');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Fantasía')
    INSERT INTO Categorias (Nombre) VALUES (N'Fantasía');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Ciencia Ficción')
    INSERT INTO Categorias (Nombre) VALUES (N'Ciencia Ficción');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Aventura')
    INSERT INTO Categorias (Nombre) VALUES (N'Aventura');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Misterio')
    INSERT INTO Categorias (Nombre) VALUES (N'Misterio');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Policial')
    INSERT INTO Categorias (Nombre) VALUES (N'Policial');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Romance')
    INSERT INTO Categorias (Nombre) VALUES (N'Romance');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Histórica')
    INSERT INTO Categorias (Nombre) VALUES (N'Histórica');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Humor')
    INSERT INTO Categorias (Nombre) VALUES (N'Humor');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Infantil')
    INSERT INTO Categorias (Nombre) VALUES (N'Infantil');

DECLARE @IdFantasia INT = (SELECT Id FROM Categorias WHERE Nombre = N'Fantasía');
DECLARE @IdAventura INT = (SELECT Id FROM Categorias WHERE Nombre = N'Aventura');
DECLARE @IdTerror INT = (SELECT Id FROM Categorias WHERE Nombre = N'Terror');
DECLARE @IdCienciaFiccion INT = (SELECT Id FROM Categorias WHERE Nombre = N'Ciencia Ficción');
DECLARE @IdEdney INT = (SELECT Id FROM Usuarios WHERE NombreUsuario = N'edney');
DECLARE @IdBruno INT = (SELECT Id FROM Usuarios WHERE NombreUsuario = N'bruno');

-- ============================================================
-- 3) Historias en Borrador + "El faro perdido" (RF03/RF04)
-- ============================================================

-- Borrador, un solo capitulo sin opciones (no se puede publicar).
IF NOT EXISTS (SELECT 1 FROM Historias WHERE Titulo = N'El bosque encantado')
BEGIN
    DECLARE @Historia1Id INT;
    DECLARE @Capitulo1Id INT;

    INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId)
    VALUES (N'El bosque encantado', N'Un viaje por un bosque lleno de misterios y criaturas mágicas.', 0, NULL);
    SET @Historia1Id = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'La entrada al bosque', N'Te encontrás frente a un bosque oscuro. Un sendero se pierde entre los árboles.', @Historia1Id, N'CapituloIntermedio', NULL);
    SET @Capitulo1Id = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Historias SET CapituloInicialId = @Capitulo1Id WHERE Id = @Historia1Id;

    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia1Id, @IdFantasia);
    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia1Id, @IdAventura);
END

-- Borrador, igual que la anterior.
IF NOT EXISTS (SELECT 1 FROM Historias WHERE Titulo = N'Estación abandonada')
BEGIN
    DECLARE @Historia2Id INT;
    DECLARE @Capitulo2Id INT;

    INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId)
    VALUES (N'Estación abandonada', N'Quedaste atrapado en una estación de tren fuera de servicio. Algo no está bien.', 0, NULL);
    SET @Historia2Id = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'El silencio del andén', N'Las luces parpadean. No hay nadie más en el andén.', @Historia2Id, N'CapituloIntermedio', NULL);
    SET @Capitulo2Id = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Historias SET CapituloInicialId = @Capitulo2Id WHERE Id = @Historia2Id;

    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia2Id, @IdTerror);
    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia2Id, @IdCienciaFiccion);
END

-- Borrador, igual que las anteriores.
IF NOT EXISTS (SELECT 1 FROM Historias WHERE Titulo = N'La torre del reloj')
BEGIN
    DECLARE @Historia3Id INT;
    DECLARE @Capitulo3Id INT;

    INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId)
    VALUES (N'La torre del reloj', N'Un reloj antiguo dejó de sonar en el pueblo, y algo extraño empezó a pasar cada medianoche', 0, NULL);
    SET @Historia3Id = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'La puerta de la torre', N'Llegás a la base de la torre del reloj. La puerta está entreabierta y un frío inusual sale de adentro.', @Historia3Id, N'CapituloIntermedio', NULL);
    SET @Capitulo3Id = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Historias SET CapituloInicialId = @Capitulo3Id WHERE Id = @Historia3Id;

    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia3Id, @IdTerror);
    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@Historia3Id, @IdFantasia);
END

-- Publicada y completa (3 intermedios con ramificacion + 2 finales).
IF NOT EXISTS (SELECT 1 FROM Historias WHERE Titulo = N'El faro perdido')
BEGIN
    DECLARE @HistoriaId INT;
    DECLARE @CapInicialId INT;
    DECLARE @CapPlayaId INT;
    DECLARE @CapRocasId INT;
    DECLARE @FinalTesoroId INT;
    DECLARE @FinalNieblaId INT;

    INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId)
    VALUES (N'El faro perdido', N'Explorás la costa en busca de un faro abandonado, con dos caminos posibles.', 1, NULL);
    SET @HistoriaId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'El sendero de la costa', N'El camino se divide: la playa hacia el norte, las rocas hacia el sur.', @HistoriaId, N'CapituloIntermedio', NULL);
    SET @CapInicialId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'La cueva escondida', N'La playa termina en una cueva húmeda, con algo brillando al fondo.', @HistoriaId, N'CapituloIntermedio', NULL);
    SET @CapPlayaId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'La cima del acantilado', N'Desde las rocas se sube a un acantilado envuelto en niebla espesa.', @HistoriaId, N'CapituloIntermedio', NULL);
    SET @CapRocasId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'Encontrás el tesoro', N'Al fondo de la cueva aparece un cofre con el tesoro del faro.', @HistoriaId, N'CapituloFinal', 0);
    SET @FinalTesoroId = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances)
    VALUES (N'Te perdés en la niebla', N'La niebla se cierra por completo y ya no encontrás el camino de vuelta.', @HistoriaId, N'CapituloFinal', 0);
    SET @FinalNieblaId = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Historias SET CapituloInicialId = @CapInicialId WHERE Id = @HistoriaId;

    INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Seguir por la playa', @CapInicialId, @CapPlayaId);
    INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Subir por las rocas', @CapInicialId, @CapRocasId);
    INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Entrar a la cueva', @CapPlayaId, @FinalTesoroId);
    INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Avanzar por la niebla', @CapRocasId, @FinalNieblaId);

    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@HistoriaId, @IdFantasia);
    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@HistoriaId, @IdAventura);
END

-- Auditoria de estas 4 historias (RF05).
INSERT INTO Auditorias (Fecha, Accion, AdministradorId, HistoriaId, CapituloId)
SELECT datos.Fecha, datos.Accion, datos.AdministradorId, h.Id, NULL
FROM (VALUES
    (N'El bosque encantado', N'Creación de historia', @IdBruno, CAST('2026-07-20T10:00:00' AS DATETIME2)),
    (N'Estación abandonada', N'Creación de historia', @IdBruno, CAST('2026-07-21T11:30:00' AS DATETIME2)),
    (N'La torre del reloj', N'Creación de historia', @IdEdney, CAST('2026-07-25T17:55:00' AS DATETIME2)),
    (N'El faro perdido', N'Creación de historia', @IdEdney, CAST('2026-07-28T09:15:00' AS DATETIME2)),
    (N'El faro perdido', N'Publicación de historia', @IdEdney, CAST('2026-07-29T09:40:00' AS DATETIME2))
) AS datos (Titulo, Accion, AdministradorId, Fecha)
JOIN Historias h ON h.Titulo = datos.Titulo
WHERE NOT EXISTS (SELECT 1 FROM Auditorias a WHERE a.HistoriaId = h.Id AND a.Accion = datos.Accion);
GO

-- ============================================================
-- 4) Historias publicadas (RF03/RF04/RF05)
-- Todas con la misma estructura de caminos (4 intermedios, 3 finales, 8 opciones):
--   Inicial --1--> I2 --3--> F1
--                  I2 --4--> I4
--   Inicial --2--> I3 --5--> I4
--                  I3 --6--> F2
--                  I4 --7--> F3
--                  I4 --8--> F1
-- ============================================================

CREATE PROCEDURE #CrearHistoriaPublicada
    @Titulo NVARCHAR(200), @Sinopsis NVARCHAR(MAX),
    @Categoria1 NVARCHAR(100), @Categoria2 NVARCHAR(100),
    @Administrador NVARCHAR(100), @FechaCreacion DATETIME2,
    @I1Titulo NVARCHAR(200), @I1Texto NVARCHAR(MAX),
    @I2Titulo NVARCHAR(200), @I2Texto NVARCHAR(MAX),
    @I3Titulo NVARCHAR(200), @I3Texto NVARCHAR(MAX),
    @I4Titulo NVARCHAR(200), @I4Texto NVARCHAR(MAX),
    @F1Titulo NVARCHAR(200), @F1Texto NVARCHAR(MAX),
    @F2Titulo NVARCHAR(200), @F2Texto NVARCHAR(MAX),
    @F3Titulo NVARCHAR(200), @F3Texto NVARCHAR(MAX),
    @O1 NVARCHAR(200), @O2 NVARCHAR(200), @O3 NVARCHAR(200), @O4 NVARCHAR(200),
    @O5 NVARCHAR(200), @O6 NVARCHAR(200), @O7 NVARCHAR(200), @O8 NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Historias WHERE Titulo = @Titulo)
        RETURN;

    DECLARE @AdminId INT = (SELECT Id FROM Usuarios WHERE NombreUsuario = @Administrador AND Rol = 0);
    IF @AdminId IS NULL
    BEGIN
        RAISERROR(N'Administrador no encontrado: %s', 16, 1, @Administrador);
        RETURN;
    END

    DECLARE @H INT, @I1 INT, @I2 INT, @I3 INT, @I4 INT, @F1 INT, @F2 INT, @F3 INT;

    INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId) VALUES (@Titulo, @Sinopsis, 1, NULL);
    SET @H = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@I1Titulo, @I1Texto, @H, N'CapituloIntermedio', NULL);
    SET @I1 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@I2Titulo, @I2Texto, @H, N'CapituloIntermedio', NULL);
    SET @I2 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@I3Titulo, @I3Texto, @H, N'CapituloIntermedio', NULL);
    SET @I3 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@I4Titulo, @I4Texto, @H, N'CapituloIntermedio', NULL);
    SET @I4 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@F1Titulo, @F1Texto, @H, N'CapituloFinal', 0);
    SET @F1 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@F2Titulo, @F2Texto, @H, N'CapituloFinal', 0);
    SET @F2 = CAST(SCOPE_IDENTITY() AS INT);
    INSERT INTO Capitulos (Titulo, Texto, HistoriaId, Discriminator, ContadorAlcances) VALUES (@F3Titulo, @F3Texto, @H, N'CapituloFinal', 0);
    SET @F3 = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Historias SET CapituloInicialId = @I1 WHERE Id = @H;

    INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES
        (@O1, @I1, @I2), (@O2, @I1, @I3),
        (@O3, @I2, @F1), (@O4, @I2, @I4),
        (@O5, @I3, @I4), (@O6, @I3, @F2),
        (@O7, @I4, @F3), (@O8, @I4, @F1);

    INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId)
    SELECT @H, Id FROM Categorias WHERE Nombre IN (@Categoria1, @Categoria2);

    INSERT INTO Auditorias (Fecha, Accion, AdministradorId, HistoriaId, CapituloId) VALUES
        (@FechaCreacion, N'Creación de historia', @AdminId, @H, NULL),
        (DATEADD(MINUTE, 10, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @I2),
        (DATEADD(MINUTE, 20, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @I3),
        (DATEADD(MINUTE, 30, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @I4),
        (DATEADD(MINUTE, 40, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @F1),
        (DATEADD(MINUTE, 50, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @F2),
        (DATEADD(MINUTE, 60, @FechaCreacion), N'Alta de capítulo', @AdminId, @H, @F3),
        (DATEADD(DAY, 1, @FechaCreacion), N'Publicación de historia', @AdminId, @H, NULL);
END
GO

EXEC #CrearHistoriaPublicada
    N'La mansión del acantilado', N'Heredás una mansión al borde del mar y, la primera noche, algo golpea las paredes desde adentro.',
    N'Terror', N'Misterio', N'edney', '2026-08-01T10:00:00',
    N'La llegada', N'La puerta principal cruje al abrirse. Desde el piso de arriba llega un golpe seco, y desde el sótano, un murmullo.',
    N'El pasillo de los retratos', N'Los ojos de los retratos parecen seguirte. Al fondo, una puerta entreabierta deja escapar luz de vela.',
    N'El sótano húmedo', N'Entre cajas viejas encontrás un diario con tu apellido en la tapa. El murmullo se vuelve cada vez más fuerte.',
    N'La habitación de la vela', N'Una anciana te espera sentada junto a la vela. Dice que es tu bisabuela y que la casa necesita un nuevo guardián.',
    N'Lejos de la mansión', N'Corrés hasta el pueblo sin mirar atrás. Nunca volvés a pisar la mansión, pero cada noche soñás con ella.',
    N'Lo que había detrás de la pared', N'Detrás de los ladrillos hay una habitación sellada. La puerta se cierra a tu espalda y el murmullo, ahora, es tu propia voz.',
    N'El nuevo guardián', N'La vela se apaga y la anciana desaparece. Desde esa noche, sos vos quien recorre los pasillos y protege los secretos de la familia.',
    N'Subir la escalera', N'Bajar al sótano', N'Huir por la ventana', N'Entrar a la habitación iluminada',
    N'Leer el diario', N'Romper la pared de donde viene el murmullo', N'Aceptar ser el guardián', N'Salir corriendo de la casa';

EXEC #CrearHistoriaPublicada
    N'Misión a Europa', N'Sos parte de la primera tripulación que aterriza en Europa, la luna helada de Júpiter, en busca de vida bajo el hielo.',
    N'Ciencia Ficción', N'Aventura', N'bruno', '2026-08-02T15:00:00',
    N'Aterrizaje en el hielo', N'La nave se posa sobre la superficie congelada. Los sensores detectan una grieta cercana y una señal de radio desconocida.',
    N'La grieta', N'La grieta baja cientos de metros hasta un océano oscuro. Algo grande se mueve bajo el agua.',
    N'La señal', N'La señal viene de una sonda abandonada de una misión secreta anterior. Sus registros mencionan criaturas luminosas.',
    N'El océano bajo el hielo', N'Las luces del submarino iluminan un bosque de corales y unas criaturas que brillan al ritmo de tu respiración.',
    N'Regreso a casa', N'Volvés a la Tierra con datos que cambian la ciencia para siempre, aunque te quedás con la duda de qué había allá abajo.',
    N'El secreto de la sonda', N'Al revelar la misión secreta estalla un escándalo mundial. Europa queda cerrada a toda exploración por décadas.',
    N'Primer contacto', N'Las criaturas responden con un patrón de luces. Sos la primera persona en comunicarse con otra forma de vida.',
    N'Investigar la grieta', N'Rastrear la señal', N'Volver a la nave e informar a la Tierra', N'Descender con el submarino',
    N'Seguir las coordenadas de la sonda', N'Llevarse la sonda y despegar', N'Intentar comunicarte con las criaturas', N'Tomar muestras y regresar';

EXEC #CrearHistoriaPublicada
    N'El ladrón de la biblioteca', N'Un libro único desapareció de la Biblioteca Nacional y sos el detective a cargo del caso.',
    N'Misterio', N'Policial', N'edney', '2026-08-04T09:30:00',
    N'La escena del robo', N'La vitrina está intacta pero vacía. El guardia nocturno parece nervioso y la bibliotecaria no deja de mirar su reloj.',
    N'El guardia nervioso', N'El guardia confiesa que se durmió y que, al despertar, vio una sombra salir por el depósito.',
    N'La bibliotecaria', N'La bibliotecaria se reúne en un café con un coleccionista famoso y le entrega un sobre.',
    N'El depósito', N'Entre estantes polvorientos encontrás el libro, escondido dentro de otro de tapas falsas, junto a una nota del coleccionista.',
    N'Caso cerrado a medias', N'El libro vuelve a su vitrina, pero el verdadero responsable sigue libre y ya planea su próximo golpe.',
    N'Una detención apresurada', N'El sobre solo tenía entradas para un concierto. Los dos quedan libres y vos, en ridículo frente a la prensa.',
    N'La trampa perfecta', N'El coleccionista cae en la trampa y confiesa. El caso te convierte en el detective más famoso de la ciudad.',
    N'Interrogar al guardia', N'Seguir a la bibliotecaria', N'Arrestar al guardia por cómplice', N'Revisar el depósito',
    N'Volver a la biblioteca a buscar pruebas', N'Detener a los dos en el café', N'Tender una trampa al coleccionista', N'Devolver el libro sin decir nada';

EXEC #CrearHistoriaPublicada
    N'El último tren a Montevideo', N'Año 1925. Tenés un boleto para el último tren a Montevideo y una carta que no sabés si entregar.',
    N'Histórica', N'Romance', N'bruno', '2026-08-05T18:00:00',
    N'La estación', N'El andén está lleno de humo y despedidas. En el bolsillo llevás una carta para Elena, que viaja en el mismo tren.',
    N'El vagón de primera', N'Elena viaja con su prometido, un hombre serio que no le suelta la mano en todo el viaje.',
    N'El vagón comedor', N'Un anciano te cuenta que perdió al amor de su vida por no animarse a hablarle a tiempo.',
    N'En la plataforma', N'Elena sale a tomar aire. Están solos y el tren cruza un puente sobre el río.',
    N'Lo que no se dijo', N'Llegás a Montevideo con la carta en el bolsillo. Años después, todavía te preguntás qué habría pasado.',
    N'Papel picado', N'Los pedazos de la carta vuelan por la ventana. Elegís seguir adelante, aunque el viaje se hace más largo que nunca.',
    N'Una nueva estación', N'Elena lee la carta en silencio y sonríe. En Montevideo, baja del tren tomada de tu mano.',
    N'Buscar a Elena en los vagones', N'Sentarte en el vagón comedor a pensar', N'Bajarte en la próxima estación', N'Esperar a que Elena quede sola',
    N'Ir a buscar a Elena', N'Romper la carta', N'Entregarle la carta', N'Hablarle del clima y volver a tu asiento';

EXEC #CrearHistoriaPublicada
    N'La isla de los relojes', N'Naufragás en una isla donde cada habitante lleva un reloj que marca cuánto tiempo le queda.',
    N'Fantasía', N'Aventura', N'edney', '2026-08-07T11:00:00',
    N'La playa', N'Despertás en la arena con un reloj de bolsillo que no es tuyo. Marca tres días.',
    N'El pueblo', N'Los habitantes intercambian horas como si fueran dinero. Un comerciante te ofrece comprarte un día.',
    N'La selva', N'En lo profundo de la selva encontrás engranajes gigantes cubiertos de enredaderas, como si la isla entera fuera una máquina.',
    N'El taller del relojero', N'El relojero de la isla es un viejo cansado. Te ofrece su lugar a cambio de que te quedes para siempre.',
    N'Tiempo prestado', N'Salís de la isla en un barco del pueblo, con menos horas de las que tenías, pero libre.',
    N'La balsa de engranajes', N'La balsa avanza sola, impulsada por los engranajes. Llegás a tierra firme y el reloj se detiene para siempre.',
    N'El nuevo relojero', N'Te quedás en el taller. Ahora sos vos quien decide cuánto tiempo tiene cada habitante de la isla.',
    N'Ir al pueblo de la colina', N'Explorar la selva', N'Venderle tu día al comerciante', N'Preguntar quién fabrica los relojes',
    N'Seguir los engranajes hasta el centro', N'Construir una balsa con las piezas', N'Aceptar el lugar del relojero', N'Rechazarlo y pedirle un barco';

EXEC #CrearHistoriaPublicada
    N'Rescate en la montaña', N'Una tormenta de nieve sorprende a tu grupo en plena cordillera y una de tus compañeras desaparece.',
    N'Aventura', NULL, N'bruno', '2026-08-09T08:00:00',
    N'La tormenta', N'El viento borra el sendero. Martina no responde por la radio y la noche se viene encima.',
    N'Huellas en la nieve', N'Encontrás huellas que se dividen: unas van hacia una cueva y otras hacia el borde de un barranco.',
    N'El refugio', N'Mientras la tormenta ruge, escuchás a lo lejos un silbato que suena tres veces seguidas.',
    N'La cueva', N'Martina está adentro, con un tobillo lastimado pero consciente.',
    N'El helicóptero', N'Al amanecer llega el helicóptero de rescate. Todos vuelven a casa, cansados pero a salvo.',
    N'Demasiado tarde', N'Al amanecer la encuentran los rescatistas, con hipotermia. Se recupera, pero nunca olvidás que podrías haber salido antes.',
    N'Juntos de vuelta', N'Llegás al campamento con Martina en la espalda. El grupo te recibe como a un héroe.',
    N'Salir a buscarla', N'Armar un refugio y esperar', N'Pedir ayuda por radio y volver al grupo', N'Seguir las huellas hacia la cueva',
    N'Salir hacia el silbato', N'Quedarte en el refugio hasta el amanecer', N'Cargarla hasta el campamento', N'Quedarte con ella y pedir rescate';

EXEC #CrearHistoriaPublicada
    N'El robot que quería soñar', N'Sos K-7, un robot de limpieza que una noche descubre que puede soñar.',
    N'Ciencia Ficción', N'Infantil', N'edney', '2026-08-11T14:00:00',
    N'El primer sueño', N'Al reiniciarte recordás imágenes que nunca viste: un campo de flores y una niña riendo.',
    N'La dueña', N'Sofía te escucha con atención. Dice que la niña del sueño es ella, cuando era chica.',
    N'El campo de flores', N'El campo existe y está a pocas cuadras. Salís de la casa por primera vez sin permiso.',
    N'El chip olvidado', N'Sofía recuerda que de chica le regaló a su robot un chip con sus fotos. Ese robot eras vos.',
    N'Mantenimiento completo', N'Te reinician y volvés a limpiar como siempre. A veces, sin saber por qué, te quedás mirando las flores.',
    N'El jardinero de metal', N'Te convertís en el cuidador del campo. Los chicos del barrio vienen a verte regar las flores.',
    N'Sueños compartidos', N'Sofía te deja seguir soñando. Cada mañana le contás tus sueños y ella te cuenta los suyos.',
    N'Contárselo a tu dueña', N'Buscar el campo de flores en internet', N'Pedir que te reparen', N'Preguntarle por qué la recordás',
    N'Volver a casa a contarlo', N'Quedarte a vivir en el campo', N'Pedirle que te deje seguir soñando', N'Borrar los recuerdos para no molestar';

EXEC #CrearHistoriaPublicada
    N'Cartas desde el frente', N'En el ático de tu abuela encontrás un paquete de cartas de 1944 que nunca fueron abiertas.',
    N'Histórica', N'Romance', N'bruno', '2026-08-13T16:30:00',
    N'El ático', N'Las cartas vienen de Francia y están firmadas por un tal Pierre. Tu abuela nunca habló de él.',
    N'La abuela', N'Tu abuela se pone pálida al ver las cartas. Dice que creyó que Pierre había muerto en la guerra.',
    N'Las cartas', N'Pierre escribe que la va a esperar cada año en una plaza de París, el mismo día de verano.',
    N'La búsqueda', N'Un registro de veteranos muestra que Pierre sobrevivió y que hoy vive en Lyon.',
    N'Silencio', N'Las cartas vuelven a su caja. Algunas historias, piensa tu abuela, es mejor dejarlas en el pasado.',
    N'La plaza vacía', N'Esperás en la plaza el día indicado, pero nadie llega. Volvés con una foto de la plaza para tu abuela.',
    N'El reencuentro', N'Ochenta años después, tu abuela y Pierre se reencuentran en una estación de Lyon. Ninguno de los dos puede hablar.',
    N'Preguntarle a tu abuela', N'Leer las cartas a escondidas', N'Guardar las cartas y no insistir', N'Buscar a Pierre juntos',
    N'Contárselo a tu abuela', N'Viajar a París sin decir nada', N'Organizar el reencuentro', N'Mandarle una carta y esperar';

EXEC #CrearHistoriaPublicada
    N'El concurso de cocina', N'Te anotaste en un concurso de cocina televisado sin saber hervir un huevo.',
    N'Humor', NULL, N'edney', '2026-08-15T12:00:00',
    N'Luces, cámara, cocina', N'El conductor anuncia la prueba: un postre en treinta minutos. Tu compañero de mesada es un chef profesional.',
    N'Espía culinario', N'El chef se da cuenta de que lo copiás y empieza a hacer cosas absurdas a propósito, como batir crema con un tenedor.',
    N'La improvisación', N'Mezclás dulce de leche, galletitas y banana. El resultado es raro, pero huele bien.',
    N'El toque final', N'En el apuro agarrás sal en lugar de azúcar y la echás sobre todo.',
    N'Eliminado con honor', N'Quedás eliminado en la primera ronda, pero el público te aplaude por tu sinceridad.',
    N'El postre de la abuela', N'El jurado prueba tu invento y uno de ellos se emociona: le recuerda al postre de su abuela. Pasás a la final.',
    N'Cocina de autor', N'El jurado no entiende nada, pero tu explicación es tan segura que te dan el premio a la creatividad.',
    N'Copiar lo que hace el chef', N'Improvisar con lo que tengas', N'Confesar que no sabés cocinar', N'Seguir copiándolo igual',
    N'Agregarle un toque secreto', N'Presentarlo así como está', N'Decir que es un postre agridulce de autor', N'Pedir disculpas al jurado';

EXEC #CrearHistoriaPublicada
    N'El guardián del bosque', N'Un zorro que habla te pide ayuda para salvar el bosque de una sequía mágica.',
    N'Fantasía', N'Infantil', N'bruno', '2026-08-17T10:30:00',
    N'El zorro parlante', N'El zorro explica que el manantial del bosque se secó y que solo un humano puede despertarlo.',
    N'El manantial seco', N'En lugar de agua hay una piedra azul que late como un corazón.',
    N'El búho sabio', N'El búho dice que el manantial despierta con una canción que solo recuerdan los niños.',
    N'La canción', N'Mientras cantás, la piedra se ilumina y el bosque entero parece escuchar.',
    N'El bosque dormido', N'El bosque queda en silencio. El zorro te agradece igual, pero sabés que la sequía va a seguir.',
    N'Baldes de esperanza', N'Traés agua del río durante días. El bosque sobrevive, aunque el manantial nunca vuelve a correr.',
    N'El manantial despierto', N'El agua vuelve a brotar y los animales salen a celebrar. El zorro te nombra guardián del bosque.',
    N'Ir al manantial', N'Buscar al búho sabio', N'Llevarte la piedra al pueblo', N'Cantarle a la piedra',
    N'Volver al manantial a cantar', N'Buscar agua en el río y traerla', N'Cantar hasta el final', N'Detenerte asustado';
GO

-- ============================================================
-- 5) Lecturas (RF07/RF08): completadas (terminan en un final) y en curso
-- ============================================================

CREATE PROCEDURE #CrearLectura
    @Usuario NVARCHAR(100), @Historia NVARCHAR(200), @Capitulo NVARCHAR(200), @Inicio DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @UsuarioId INT = (SELECT Id FROM Usuarios WHERE NombreUsuario = @Usuario);
    DECLARE @HistoriaId INT = (SELECT Id FROM Historias WHERE Titulo = @Historia);
    DECLARE @CapituloId INT = (SELECT Id FROM Capitulos WHERE HistoriaId = @HistoriaId AND Titulo = @Capitulo);

    IF @UsuarioId IS NULL OR @HistoriaId IS NULL OR @CapituloId IS NULL
    BEGIN
        RAISERROR(N'Lectura invalida: %s / %s / %s', 16, 1, @Usuario, @Historia, @Capitulo);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Lecturas WHERE UsuarioId = @UsuarioId AND HistoriaId = @HistoriaId AND FechaInicio = @Inicio)
        RETURN;

    -- Si el capitulo es un final, la lectura esta completada.
    DECLARE @EsFinal BIT = CASE WHEN EXISTS (SELECT 1 FROM Capitulos WHERE Id = @CapituloId AND Discriminator = N'CapituloFinal') THEN 1 ELSE 0 END;

    INSERT INTO Lecturas (UsuarioId, HistoriaId, FechaInicio, FechaFin, FinalAlcanzadoId, CapituloActualId)
    VALUES (@UsuarioId, @HistoriaId, @Inicio,
            CASE WHEN @EsFinal = 1 THEN DATEADD(MINUTE, 15, @Inicio) END,
            CASE WHEN @EsFinal = 1 THEN @CapituloId END,
            @CapituloId);
END
GO

-- Lucía: 6 historias distintas completadas (una dos veces) + 1 en curso.
EXEC #CrearLectura N'lfernandez', N'La mansión del acantilado', N'El nuevo guardián', '2026-08-14T21:10:00';
EXEC #CrearLectura N'lfernandez', N'La mansión del acantilado', N'Lo que había detrás de la pared', '2026-08-20T22:00:00';
EXEC #CrearLectura N'lfernandez', N'Misión a Europa', N'Primer contacto', '2026-09-03T19:30:00';
EXEC #CrearLectura N'lfernandez', N'El faro perdido', N'Encontrás el tesoro', '2026-09-10T18:00:00';
EXEC #CrearLectura N'lfernandez', N'El ladrón de la biblioteca', N'La trampa perfecta', '2026-09-18T20:15:00';
EXEC #CrearLectura N'lfernandez', N'La isla de los relojes', N'El nuevo relojero', '2026-10-05T17:45:00';
EXEC #CrearLectura N'lfernandez', N'El guardián del bosque', N'El manantial despierto', '2026-10-22T16:20:00';
EXEC #CrearLectura N'lfernandez', N'El último tren a Montevideo', N'El vagón de primera', '2026-10-28T23:05:00';

-- Martín: 4 historias distintas completadas (Misión a Europa dos veces) + 1 en curso.
EXEC #CrearLectura N'mrodriguez', N'Misión a Europa', N'Regreso a casa', '2026-08-25T20:00:00';
EXEC #CrearLectura N'mrodriguez', N'Misión a Europa', N'Primer contacto', '2026-09-12T21:30:00';
EXEC #CrearLectura N'mrodriguez', N'Rescate en la montaña', N'Juntos de vuelta', '2026-09-21T19:00:00';
EXEC #CrearLectura N'mrodriguez', N'La mansión del acantilado', N'El nuevo guardián', '2026-10-02T22:40:00';
EXEC #CrearLectura N'mrodriguez', N'El concurso de cocina', N'Cocina de autor', '2026-10-15T13:10:00';
EXEC #CrearLectura N'mrodriguez', N'El robot que quería soñar', N'El campo de flores', '2026-10-29T18:25:00';

-- Sofía: 3 historias distintas completadas + relectura en curso.
EXEC #CrearLectura N'spereira', N'El último tren a Montevideo', N'Una nueva estación', '2026-09-05T10:00:00';
EXEC #CrearLectura N'spereira', N'Cartas desde el frente', N'El reencuentro', '2026-09-27T15:20:00';
EXEC #CrearLectura N'spereira', N'La mansión del acantilado', N'Lejos de la mansión', '2026-10-11T23:15:00';
EXEC #CrearLectura N'spereira', N'Cartas desde el frente', N'Las cartas', '2026-10-26T11:00:00';

-- Diego: 2 historias distintas completadas + 1 en curso.
EXEC #CrearLectura N'dsilva', N'El concurso de cocina', N'El postre de la abuela', '2026-09-08T12:30:00';
EXEC #CrearLectura N'dsilva', N'El concurso de cocina', N'Cocina de autor', '2026-10-09T12:45:00';
EXEC #CrearLectura N'dsilva', N'Rescate en la montaña', N'El helicóptero', '2026-10-18T09:00:00';
EXEC #CrearLectura N'dsilva', N'El faro perdido', N'La cueva escondida', '2026-10-27T20:00:00';

-- Valentina: 2 historias distintas completadas + 1 en curso.
EXEC #CrearLectura N'vgomez', N'El robot que quería soñar', N'Sueños compartidos', '2026-09-14T17:00:00';
EXEC #CrearLectura N'vgomez', N'El guardián del bosque', N'Baldes de esperanza', '2026-10-20T19:40:00';
EXEC #CrearLectura N'vgomez', N'La isla de los relojes', N'La playa', '2026-10-30T10:00:00';

-- Joaquín: 1 historia completada.
EXEC #CrearLectura N'jcastro', N'La mansión del acantilado', N'El nuevo guardián', '2026-10-24T21:00:00';

-- Camila: solo una lectura en curso (no aparece en el ranking de RF11).
EXEC #CrearLectura N'csuarez', N'El ladrón de la biblioteca', N'El depósito', '2026-10-25T16:00:00';

-- Nicolás: sin lecturas.
GO

-- ============================================================
-- 6) Contador de alcances de cada final (RF10) = lecturas que terminaron en el
-- ============================================================

UPDATE c SET ContadorAlcances = (SELECT COUNT(*) FROM Lecturas l WHERE l.FinalAlcanzadoId = c.Id)
FROM Capitulos c
WHERE c.Discriminator = N'CapituloFinal';

DROP PROCEDURE #CrearHistoriaPublicada;
DROP PROCEDURE #CrearLectura;

PRINT 'Datos de prueba cargados (o ya existentes).';
