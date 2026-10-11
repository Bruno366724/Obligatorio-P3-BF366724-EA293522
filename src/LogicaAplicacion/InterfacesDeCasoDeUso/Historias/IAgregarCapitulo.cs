namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IAgregarCapitulo
{
    void Ejecutar(int historiaId, int capituloAntecedenteId, bool esFinal, string titulo, string texto, string textoOpcion, int administradorId);
}
