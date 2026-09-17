-- Script de prueba manual para RF03 (NO es el script final del obligatorio).
-- Carga categorías y un par de historias completas (con su capítulo inicial)
-- para poder navegar Index/Details/Edit sin tener que crear nada a mano primero.
-- Ejecutar con: sqlcmd -S "(localdb)\MSSQLLocalDB" -d HistoriasDB -i scripts\datos-prueba-rf03.sql

SET NOCOUNT ON;

INSERT INTO Categorias (Nombre) VALUES (N'Terror');
INSERT INTO Categorias (Nombre) VALUES (N'Fantasía');
INSERT INTO Categorias (Nombre) VALUES (N'Ciencia Ficción');
INSERT INTO Categorias (Nombre) VALUES (N'Aventura');

DECLARE @IdFantasia INT = (SELECT Id FROM Categorias WHERE Nombre = N'Fantasía');
DECLARE @IdAventura INT = (SELECT Id FROM Categorias WHERE Nombre = N'Aventura');
DECLARE @IdTerror INT = (SELECT Id FROM Categorias WHERE Nombre = N'Terror');
DECLARE @IdCienciaFiccion INT = (SELECT Id FROM Categorias WHERE Nombre = N'Ciencia Ficción');

-- Historia 1: El bosque encantado (Fantasía + Aventura)
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

-- Historia 2: Estación abandonada (Terror + Ciencia Ficción)
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

PRINT 'Datos de prueba cargados.';
