using DSW_3P_EJ01.Data;
using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Repositories;

public class MisionSqlRepository : IMisionRepository
{
    private readonly AppDbContext _context;

    public MisionSqlRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Mision> ObtenerTodos()
    {
        return _context.Misiones.ToList();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _context.Misiones.Find(id);
    }

    public Mision Agregar(Mision mision)
    {
        mision.Id = 0;
        _context.Misiones.Add(mision);
        _context.SaveChanges();

        return mision;
    }

    public bool Actualizar(int id, Mision mision)
    {
        var actual = _context.Misiones.Find(id);
        if (actual is null)
        {
            return false;
        }

        actual.Nombre = mision.Nombre;
        actual.Dificultad = mision.Dificultad;
        actual.RecompensaXP = mision.RecompensaXP;
        actual.Completada = mision.Completada;

        _context.SaveChanges();
        return true;
    }

    public bool Eliminar(int id)
    {
        var Mision = _context.Misiones.Find(id);
        if (Mision is null)
        {
            return false;
        }

        _context.Misiones.Remove(Mision);
        _context.SaveChanges();
        return true;
    }
}