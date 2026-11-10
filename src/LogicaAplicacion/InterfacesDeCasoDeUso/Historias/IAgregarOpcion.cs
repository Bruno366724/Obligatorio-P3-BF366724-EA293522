using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IAgregarOpcion
{
    void Ejecutar(AgregarOpcionDTO dto, int administradorId);
}
