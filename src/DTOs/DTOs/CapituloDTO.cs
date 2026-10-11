using System.Text.Json.Serialization;

namespace DTOs.DTOs;

// Discriminador "tipo" en el JSON para poder serializar/deserializar los subtipos.
[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipo")]
[JsonDerivedType(typeof(CapituloIntermedioDTO), "intermedio")]
[JsonDerivedType(typeof(CapituloFinalDTO), "final")]
public abstract class CapituloDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public int HistoriaId { get; set; }
}
