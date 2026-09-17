using System.Text.RegularExpressions;
using Dominio.Excepciones;
using Dominio.InterfacesDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects;

[Owned]
public record Email : IValidable
{
    private static readonly Regex FormatoValido = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Direccion { get; init; } = string.Empty;

    public Email(string direccion)
    {
        Direccion = direccion;
        Validar();
    }

    protected Email()
    {
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Direccion) || !FormatoValido.IsMatch(Direccion))
            throw new UsuarioException("El email ingresado no es válido.");
    }

    public virtual bool Equals(Email? otro) =>
        otro is not null && Direccion.Equals(otro.Direccion, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Direccion.ToLowerInvariant().GetHashCode();

    public override string ToString() => Direccion;
}
