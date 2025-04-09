namespace MiBlog.Entities
{
    public class BlogEtiqueta
    {
        public int IdBlogEtiqueta { get; set; }

        public int IdBlog { get; set; }
        public Blog Blog { get; set; }

        public int IdEtiqueta { get; set; }
        public Etiqueta Etiqueta { get; set; }
    }
}
