using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.ValueObjects;

public class Password : IValidable
{
    private const int LongitudMinima = 8;

    public string Valor { get; }

    public Password(string valor)
    {
        Valor = valor;
        Validar();
    }

    protected Password()
    {
        Valor = string.Empty;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(Valor) || Valor.Length < LongitudMinima)
            throw new DomainException($"La contraseña debe tener al menos {LongitudMinima} caracteres.");

        if (!Valor.Any(char.IsUpper))
            throw new DomainException("La contraseña debe incluir al menos una letra mayúscula.");

        if (!Valor.Any(char.IsLower))
            throw new DomainException("La contraseña debe incluir al menos una letra minúscula.");

        if (!Valor.Any(char.IsDigit))
            throw new DomainException("La contraseña debe incluir al menos un número.");

        if (!Valor.Any(c => !char.IsLetterOrDigit(c)))
            throw new DomainException("La contraseña debe incluir al menos un carácter especial.");
    }

    public override bool Equals(object? obj) =>
        obj is Password otra && Valor == otra.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => new string('*', Valor.Length);
}
