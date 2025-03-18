namespace MiBlog.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public List<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();

    }
}
