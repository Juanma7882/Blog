namespace MiBlog.Entities
{
    public class Etiqueta
    {
        public int IdEtiqueta { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<BlogEtiqueta> BlogEtiquetas { get; set; } = new List<BlogEtiqueta>();
    }
}
