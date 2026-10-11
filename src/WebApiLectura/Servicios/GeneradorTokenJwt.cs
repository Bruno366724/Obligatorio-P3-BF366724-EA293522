using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTOs.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace WebApiLectura.Servicios;

// Arma el JWT firmado que devuelve el login.
public class GeneradorTokenJwt
{
    private readonly IConfiguration configuracion;

    public GeneradorTokenJwt(IConfiguration config)
    {
        configuracion = config;
    }

    public string Generar(UsuarioDTO usuario)
    {
        IConfigurationSection configJwt = configuracion.GetSection("Jwt");

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString())
        ];

        SymmetricSecurityKey clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwt["Clave"]!));
        SigningCredentials credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: configJwt["Emisor"],
            audience: configJwt["Audiencia"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(configJwt["ExpiraMinutos"]!)),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
