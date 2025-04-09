namespace MiBlog.Entities
{
    public class Blog
    {
        public int IdBlog { get; set; }
        public required string Titulo { get; set; }
        public required string Contenido { get; set; }
        public DateTime FechaDePublicacion { get; set; } = DateTime.UtcNow;
        public required string Descripcion { get; set; }
        public required string Enlace { get; set; }

        //public int AutorId { get; set; }  // ID del usuario que creó el blog
        //public Usuario Autor { get; set; } // Relación con el usuario

        // un blog puede tener varios ETIQUETAS las bases de datos no pueden guardar listas 
        public List<BlogEtiqueta> BlogEtiquetas { get; set; } = new List<BlogEtiqueta>();
        
        // un blog puede tener varios usuarios
        //public List<UsuarioBlog> usuarioBlog { get; set; } = new List<UsuarioBlog>();
    
    }
}
