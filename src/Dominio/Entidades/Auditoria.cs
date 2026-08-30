using Dominio.Excepciones;
using Dominio.Interfaces;

namespace Dominio.Entidades;

public class Auditoria : IValidable
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Accion { get; set; } = string.Empty;
    public int AdministradorId { get; set; }
    public Usuario? Administrador { get; set; }
    public int HistoriaId { get; set; }
    public Historia? Historia { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Accion))
            throw new DomainException("La acción registrada en la auditoría es obligatoria.");

        if (AdministradorId <= 0 && Administrador is null)
            throw new DomainException("La auditoría debe estar asociada a un administrador.");

        if (HistoriaId <= 0 && Historia is null)
            throw new DomainException("La auditoría debe estar asociada a una historia.");
    }
}
