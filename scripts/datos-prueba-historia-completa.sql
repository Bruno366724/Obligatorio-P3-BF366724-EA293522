-- Script de prueba manual (NO es el script final del obligatorio).
-- Carga UNA historia completa y publicable: 3 capítulos intermedios con
-- ramificación + 2 finales, todo dentro de la misma historia (no se comparten
-- capítulos entre historias, como exige la letra).
-- Ejecutar con: sqlcmd -S "(localdb)\MSSQLLocalDB" -d HistoriasDB -f 65001 -i scripts\datos-prueba-historia-completa.sql

SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Fantasía')
    INSERT INTO Categorias (Nombre) VALUES (N'Fantasía');
IF NOT EXISTS (SELECT 1 FROM Categorias WHERE Nombre = N'Aventura')
    INSERT INTO Categorias (Nombre) VALUES (N'Aventura');

DECLARE @IdFantasia INT = (SELECT Id FROM Categorias WHERE Nombre = N'Fantasía');
DECLARE @IdAventura INT = (SELECT Id FROM Categorias WHERE Nombre = N'Aventura');

DECLARE @HistoriaId INT;
DECLARE @CapInicialId INT;
DECLARE @CapPlayaId INT;
DECLARE @CapRocasId INT;
DECLARE @FinalTesoroId INT;
DECLARE @FinalNieblaId INT;

-- 1) Historia, todavía sin capítulo inicial (se completa después, por el ciclo de FKs).
INSERT INTO Historias (Titulo, Sinopsis, Estado, CapituloInicialId)
VALUES (N'El faro perdido', N'Explorás la costa en busca de un faro abandonado, con dos caminos posibles.', 1, NULL);
SET @HistoriaId = CAST(SCOPE_IDENTITY() AS INT);

-- 2) Capítulos, todos ya con su HistoriaId.
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

-- 3) Ahora que el capítulo inicial ya tiene Id real, se completa el link.
UPDATE Historias SET CapituloInicialId = @CapInicialId WHERE Id = @HistoriaId;

-- 4) Opciones que encadenan los capítulos (la ramificación).
INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Seguir por la playa', @CapInicialId, @CapPlayaId);
INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Subir por las rocas', @CapInicialId, @CapRocasId);
INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Entrar a la cueva', @CapPlayaId, @FinalTesoroId);
INSERT INTO Opciones (Texto, CapituloIntermedioId, CapituloDestinoId) VALUES (N'Avanzar por la niebla', @CapRocasId, @FinalNieblaId);

-- 5) Categorías de la historia.
INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@HistoriaId, @IdFantasia);
INSERT INTO HistoriaCategorias (HistoriaId, CategoriasId) VALUES (@HistoriaId, @IdAventura);

PRINT 'Historia completa cargada.';
