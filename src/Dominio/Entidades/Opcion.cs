using Dominio.Excepciones;
using Dominio.Interfaces;

namespace Dominio.Entidades;

public class Opcion : IValidable
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int CapituloIntermedioId { get; set; }
    public int CapituloDestinoId { get; set; }
    public Capitulo? CapituloDestino { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Texto))
            throw new DomainException("El texto de la opción es obligatorio.");

        if (CapituloDestinoId <= 0 && CapituloDestino is null)
            throw new DomainException("La opción debe llevar a un capítulo destino.");
    }
}
