using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

public interface IEncontrarTodasCategorias
{
    List<CategoriaDTO> Ejecutar();
}
