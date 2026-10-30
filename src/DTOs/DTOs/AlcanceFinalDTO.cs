namespace DTOs.DTOs;

// Un final de una historia con cuantas veces fue alcanzado.
public class AlcanceFinalDTO
{
    public int CapituloId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int ContadorAlcances { get; set; }
    public bool EsMasFrecuente { get; set; }
}
