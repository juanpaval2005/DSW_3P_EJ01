using DSW_3P_EJ01.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DSW_3P_EJ01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;

    private static readonly object _bloqueo = new();

    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(environment.ContentRootPath, "Data", "misiones.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Mision> Leer()
    {
        string json = File.ReadAllText(_ruta);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Mision>();

        return JsonSerializer.Deserialize<List<Mision>>(json, _opciones) ?? new List<Mision>();
    }

    private void Guardar(List<Mision> datos)
    {
        string json = JsonSerializer.Serialize(datos, _opciones);

        File.WriteAllText(_ruta, json);
    }

    public List<Mision> ObtenerTodas()
    {
        lock (_bloqueo)
        {
            return Leer();
        }
    }

    public Mision? ObtenerPorId(int id)
    {
        lock (_bloqueo)
        {
            return Leer().FirstOrDefault(m => m.Id == id);
        }
    }

    public Mision Agregar(Mision nueva)
    {
        lock (_bloqueo)
        {
            var datos = Leer();

            nueva.Id = datos.Count == 0 ? 1 : datos.Max(m => m.Id) + 1;
            datos.Add(nueva);
            Guardar(datos);

            return nueva;
        }
    }

    public bool Actualizar(int id, Mision actualizada)
    {
        lock (_bloqueo)
        {
            var datos = Leer();

            var existente = datos.FirstOrDefault(m => m.Id == id);

            if (existente == null)
                return false;

            existente.Nombre = actualizada.Nombre;
            existente.Dificultad = actualizada.Dificultad;
            existente.RecompensaXP = actualizada.RecompensaXP;
            existente.Completada = actualizada.Completada;

            Guardar(datos);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_bloqueo)
        {
            var datos = Leer();

            var existente = datos.FirstOrDefault(m => m.Id == id);

            if (existente == null)
                return false;

            datos.Remove(existente);
            Guardar(datos);
            return true;
        }
    }
}
