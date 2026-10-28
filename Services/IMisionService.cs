using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Services;

public interface IMisionService
{
    List<Mision> ObtenerTodas();
    Mision ObtenerPorId(int id);
    Mision Agregar(Mision nueva);
    void Actualizar(int id, Mision actualizada);
    void Eliminar(int id);
}
