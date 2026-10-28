using System.Globalization;
using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Repositories;

namespace DSW_3P_EJ01.Services;

public class MisionService : IMisionService
{
    private static readonly string[] DificultadesValidas = { "Fácil", "Media", "Difícil" };

    // Acepta "facil", "FACIL", "Fácil", etc.
    private static readonly StringComparer ComparadorTexto = StringComparer.Create(
        CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

    private readonly IMisionRepository _repository;

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    public List<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Mision ObtenerPorId(int id)
    {
        return BuscarExistente(id);
    }

    public Mision Agregar(Mision nueva)
    {
        Validar(nueva);
        nueva.Completada = false;

        return _repository.Agregar(nueva);
    }

    public void Actualizar(int id, Mision actualizada)
    {
        BuscarExistente(id);
        Validar(actualizada);

        _repository.Actualizar(id, actualizada);
    }

    public void Eliminar(int id)
    {
        BuscarExistente(id);
        _repository.Eliminar(id);
    }

    private Mision BuscarExistente(int id)
    {
        var mision = _repository.ObtenerPorId(id);

        if (mision == null)
            throw new KeyNotFoundException($"No existe la misión con id {id}");

        return mision;
    }

    private static void Validar(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
            throw new ArgumentException("El nombre de la misión es obligatorio");

        var dificultad = DificultadesValidas.FirstOrDefault(d => ComparadorTexto.Equals(d, mision.Dificultad?.Trim()));

        if (dificultad == null)
            throw new ArgumentException("La dificultad solo puede ser Fácil, Media o Difícil");

        if (mision.RecompensaXP < 0)
            throw new ArgumentException("La recompensa XP no puede ser negativa");

        mision.Nombre = mision.Nombre.Trim();
        mision.Dificultad = dificultad;
    }
}
