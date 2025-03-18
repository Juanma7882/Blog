using MiBlog.Enums;

namespace MiBlog.DTOs
{
    public class SesionDTO
    {
        public int IdPersona { get; set; }
        public required string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Dni { get; set; }
        public List<RolesEnum> UsuarioRoles { get; set; } = new List<RolesEnum>();

    }
}
