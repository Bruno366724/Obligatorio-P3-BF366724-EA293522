-- Script de prueba manual, idempotente: un Administrador, un Lector, categorias, e
-- historias en distintos estados.
-- Ejecutar con: sqlcmd -S "(localdb)\MSSQLLocalDB" -d HistoriasDB -f 65001 -i scripts\datos-prueba-completo.sql

SET NOCOUNT ON;

-- ============================================================
-- 1) Usuarios (RF02/RF01/RF06) -- Rol: 0 = Administrador, 1 = Lector
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'admin1')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Admin de Prueba', N'admin@obligatorio.com', N'admin1', N'Admin123!', 0);

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE NombreUsuario = N'lector1')
    INSERT INTO Usuarios (NombreCompleto, Email_Direccion, NombreUsuario, Password_Valor, Rol)
    VALUES (N'Lector de Prueba', N'lector@obligatorio.com', N'lector1', N'Lector123!', 1);

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

DECLARE @IdFantasia INT = (SELECT Id FROM Categorias WHERE Nombre = N'Fantasía');
DECLARE @IdAventura INT = (SELECT Id FROM Categorias WHERE Nombre = N'Aventura');
DECLARE @IdTerror INT = (SELECT Id FROM Categorias WHERE Nombre = N'Terror');
DECLARE @IdCienciaFiccion INT = (SELECT Id FROM Categorias WHERE Nombre = N'Ciencia Ficción');

-- ============================================================
-- 3) Historias (RF03/RF04)
-- ============================================================

-- Historia 1: Borrador, un solo capitulo sin opciones.
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

-- Historia 2: igual que la 1, otro ejemplo en Borrador.
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

-- Historia 3: Publicada y completa (3 intermedios con ramificación + 2 finales).
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

PRINT 'Datos minimos de prueba (RF01-RF06) cargados u ya existentes.';
