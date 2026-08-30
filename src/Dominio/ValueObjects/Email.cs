using System.Text.RegularExpressions;
using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.ValueObjects;

public class Email : IValidable
{
    private static readonly Regex FormatoValido = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Direccion { get; }

    public Email(string direccion)
    {
        Direccion = direccion;
        Validar();
    }

    protected Email()
    {
        Direccion = string.Empty;
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Direccion) || !FormatoValido.IsMatch(Direccion))
            throw new DomainException("El email ingresado no es válido.");
    }

    public override bool Equals(object? obj) =>
        obj is Email otro && Direccion.Equals(otro.Direccion, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Direccion.ToLowerInvariant().GetHashCode();

    public override string ToString() => Direccion;
}
