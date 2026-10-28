namespace DSW_3P_EJ01.Models;

public class Mision
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Dificultad { get; set; } = "";
    public int RecompensaXP { get; set; }
    public bool Completada { get; set; }
}
