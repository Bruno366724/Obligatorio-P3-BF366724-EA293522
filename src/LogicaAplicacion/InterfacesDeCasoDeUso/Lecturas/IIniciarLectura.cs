using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IIniciarLectura
{
    LecturaDTO Ejecutar(int usuarioId, int historiaId);
}
