using Dominio.Excepciones;

namespace Dominio.Entidades;

public class CapituloIntermedio : Capitulo
{
    public List<Opcion> Opciones { get; set; } = new();

    public override void Validar()
    {
        base.Validar();

        if (Opciones.Count == 0)
            throw new DomainException("Un capítulo intermedio debe tener al menos una opción.");

        foreach (var opcion in Opciones)
            opcion.Validar();
    }
}
