namespace MiBlog.Entities
{
    public class UsuarioRol
    {
        public int IdUsuarioRol { get; set; }

        // Relación con Usuario
        public Usuario Usuario { get; set; }
        public int IdUsuario { get; set; }

        // Relación con Rol
        public Rol Rol { get; set; }
        public int IdRol { get; set; }
    }
}
