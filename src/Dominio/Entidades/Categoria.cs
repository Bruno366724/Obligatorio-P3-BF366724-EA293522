using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.Entidades;

public class Categoria : IValidable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            throw new CategoriaException("El nombre de la categoría es obligatorio.");
    }
}
