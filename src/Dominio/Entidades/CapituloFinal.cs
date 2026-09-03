namespace Dominio.Entidades;

public class CapituloFinal : Capitulo
{
    public int ContadorAlcances { get; set; } = 0;

    public void RegistrarAlcance() => ContadorAlcances++;
}
