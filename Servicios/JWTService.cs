using MiBlog.AppDbContext;
using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MiBlog.Servicios
{
    public class JWTService
    {

        private readonly AppDbBlogContext _appDbContext;
        public IConfiguration _configuration { get; set; }


        public JWTService(AppDbBlogContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<Usuario> ValidarLogin(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    return null;

                }

                var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == loginDTO.NombreDeUsuario);


                if (usuario == null || usuario.Clave != loginDTO.Clave)
                {
                    return null;
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al validar el login: {ex.Message}");
            }
        }

       



        public async Task<string> GenerarToken(Usuario usuario)
        {
            try
            {
                if(usuario == null)
                {
                    return null;
                }

                // Obtener la lista de roles del usuario
                var roles = await _appDbContext.UsuarioRoles
                    .Where(ur => ur.IdUsuario == usuario.IdPersona)
                    .Select(ur => ur.Rol.NombreRol) // Obtiene los nombres de los roles
                    .ToListAsync();

                // generar el token
                var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
                var jwtIssuer = _configuration["Jwt:Issuer"];
                var jwtAudience = _configuration["Jwt:Audience"];
                var jwtSubject = _configuration["Jwt:Subject"];

                
                List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.NombreUsuario),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("IdPersona", usuario.IdPersona.ToString()),
                    new Claim("NombreUsuario", usuario.NombreUsuario),
                };
                foreach (var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));

                }


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    jwtIssuer,
                    jwtAudience,
                    claims,
                    expires: DateTime.UtcNow.AddHours(30), // Token válido por x cantidad de tiempo
                    signingCredentials: signIn
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            
            }
            catch (Exception ex)
            {
                return ($"ERRROR al generar un TOKEN {ex.Message}");
            }
        }

        
       

        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity == null)
                {
                    return new
                    {
                        success = false,
                        message = "Token no válido",
                        result = ""
                    };
                }

                var idPersona = identity.Claims.FirstOrDefault(c => c.Type == "IdPersona")?.Value;
                var nombreUsuario = identity.Claims.FirstOrDefault(c => c.Type == "NombreUsuario")?.Value;

                if (string.IsNullOrEmpty(idPersona) || string.IsNullOrEmpty(nombreUsuario))
                {
                    return new
                    {
                        success = false,
                        message = "Token no válido",
                        result = ""
                    };
                }

                return new
                {
                    success = true,
                    message = "Token válido",
                    result = new
                    {
                        IdPersona = idPersona,
                        NombreUsuario = nombreUsuario
                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = $"Error al validar el token: {ex.Message}",
                    result = ""
                };
            }
        }


    }
}
