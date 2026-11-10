using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IAgregarCapitulo
{
    void Ejecutar(AgregarCapituloDTO dto, int administradorId);
}
