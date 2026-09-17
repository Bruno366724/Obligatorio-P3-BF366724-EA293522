using Dominio.Excepciones;
using Dominio.InterfacesDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects;

[Owned]
public record Password : IValidable
{
    private const int LongitudMinima = 8;

    public string Valor { get; init; } = string.Empty;

    public Password(string valor)
    {
        Valor = valor;
        Validar();
    }

    protected Password()
    {
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(Valor) || Valor.Length < LongitudMinima)
            throw new UsuarioException($"La contraseña debe tener al menos {LongitudMinima} caracteres.");

        if (!Valor.Any(char.IsUpper))
            throw new UsuarioException("La contraseña debe incluir al menos una letra mayúscula.");

        if (!Valor.Any(char.IsLower))
            throw new UsuarioException("La contraseña debe incluir al menos una letra minúscula.");

        if (!Valor.Any(char.IsDigit))
            throw new UsuarioException("La contraseña debe incluir al menos un número.");

        if (!Valor.Any(c => !char.IsLetterOrDigit(c)))
            throw new UsuarioException("La contraseña debe incluir al menos un carácter especial.");
    }

    public override string ToString() => new string('*', Valor.Length);
}
