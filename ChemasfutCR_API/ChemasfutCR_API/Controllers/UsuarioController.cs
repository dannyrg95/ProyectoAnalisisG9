using ChemasfutCR_API.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ChemasfutCR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        [Route("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario(Usuario entidad)
        {
            Respuesta resp = new Respuesta();

            using (var context = new SqlConnection("Server=LIED95\\SQLEXPRESS;Database=ChemasfutCR;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                var result = await context.ExecuteAsync("RegistrarUsuario", 
                    new { entidad.Nombre, entidad.Apellido, entidad.Fecha_Nacimiento, entidad.Teléfono, entidad.Email, entidad.Contraseña, entidad.Identificacion}, 
                    commandType: System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    resp.Codigo = 1;
                    resp.Mensaje = "OK";
                    resp.Contenido = true;
                    return Ok(resp);
                }                    
                else
                {
                    resp.Codigo = 0;
                    resp.Mensaje = "La informacion del usuario ya se encuentra registrada";
                    resp.Contenido = false;
                    return Ok("Error");
                }
            }
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion(Usuario entidad)
        {
            Respuesta resp = new Respuesta();

            using (var context = new SqlConnection("Server=LIED95\\SQLEXPRESS;Database=ChemasfutCR;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                var result = await context.QueryAsync<Usuario>("IniciarSesion",
                    new { entidad.Email, entidad.Contraseña},
                    commandType: System.Data.CommandType.StoredProcedure);

                if (result.Count() > 0)
                {
                    resp.Codigo = 1;
                    resp.Mensaje = "OK";
                    resp.Contenido = result;
                    return Ok(resp);
                }
                else
                {
                    resp.Codigo = 0;
                    resp.Mensaje = "La informacion del usuario ya se encuentra registrada";
                    resp.Contenido = false;
                    return Ok("Error");
                }
            }
        }

    }
}
