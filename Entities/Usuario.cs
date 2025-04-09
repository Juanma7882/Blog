namespace MiBlog.Entities
{
    public class Usuario
    {
        public int IdPersona { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Clave { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Dni { get; set; }

        // Relación con UsuarioRol (Un usuario puede tener varios roles)
        public List<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();

    }
}
