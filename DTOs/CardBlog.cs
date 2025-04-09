namespace MiBlog.DTOs
{
    public class CardBlog
    {
        public int IdBlog { get; set; }
        public required string Titulo { get; set; }
        public DateTime FechaDePublicacion { get; set; } = DateTime.UtcNow;
        public required string Descripcion { get; set; }
        public required string Enlace { get; set; }

        //public int AutorId { get; set; }  // ID del usuario que creó el blog
        //public Usuario Autor { get; set; } // Relación con el usuario

        // un blog puede tener varios ETIQUETAS las bases de datos no pueden guardar listas 
        public List<string> BlogEtiquetas { get; set; } = new List<string>();

        // un blog puede tener varios usuarios
        //public List<UsuarioBlog> usuarioBlog { get; set; } = new List<UsuarioBlog>();
    }
}
