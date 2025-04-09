using MiBlog.Entities;

namespace MiBlog.DTOs
{
    public class BlogDTO
    {
        public int IdBlog { get; set; }
        public required string Titulo { get; set; }
        public required string Contenido { get; set; }
        public DateTime FechaDePublicacion { get; set; } = DateTime.UtcNow;
        public required string Descripcion { get; set; }
        public required string Enlace { get; set; }
        // un blog puede tener varios ETIQUETAS las bases de datos no pueden guardar listas 
        public List<string> Etiquetas { get; set; } = new List<string>();
    }
}
