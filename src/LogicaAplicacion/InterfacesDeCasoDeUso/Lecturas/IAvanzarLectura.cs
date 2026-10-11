using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IAvanzarLectura
{
    LecturaDTO Ejecutar(int lecturaId, int opcionId, int usuarioId);
}
