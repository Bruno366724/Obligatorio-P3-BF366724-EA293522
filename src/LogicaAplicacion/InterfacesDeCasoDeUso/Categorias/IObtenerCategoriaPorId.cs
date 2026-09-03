using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

public interface IObtenerCategoriaPorId
{
    CategoriaDTO Ejecutar(int id);
}
