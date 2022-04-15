using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persitencia
{
    public class DataPrueba
    {
        public static async Task
        InsertarData(
            CursosOnlineContext context,
            UserManager<Usuario> usuarioManager
        )
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario =
                    new Usuario {
                        NombreCompleto = "Juan Del Angel",
                        UserName = "jangel",
                        Email = "jmdelan2012@gmail.com"
                    };
                await usuarioManager.CreateAsync(usuario, "Camus2514*");
            }
        }
    }
}
