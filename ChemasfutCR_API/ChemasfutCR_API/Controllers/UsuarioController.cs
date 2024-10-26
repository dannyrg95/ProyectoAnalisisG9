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

            //using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;Trusted_Connection=True;TrustServerCertificate=True;"))
            using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
            {
                var result = await context.ExecuteAsync("RegistrarUsuario", 
                    new { entidad.Nombre, entidad.Apellido, entidad.Identificacion, entidad.Fecha_Nacimiento, entidad.Telefono, entidad.Email, entidad.Password, entidad.ID_Rol },
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

        [HttpGet]
        [Route("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            Respuesta resp = new Respuesta();

            using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
            {
                await context.OpenAsync();

                var result = await context.QueryAsync<Usuario>("ObtenerTodosLosUsuarios", commandType: System.Data.CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    resp.Codigo = 200;
                    resp.Mensaje = "OK";
                    resp.Contenido = result;
                }
            }

            return Ok(resp);
        }

        [HttpGet]
        [Route("ObtenerUsuarioPorId")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            Respuesta resp = new Respuesta();

            using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
            {
                await context.OpenAsync();

                var parametros = new {ID_Usuario = id};

                var result = await context.QueryFirstOrDefaultAsync<Usuario>("ObtenerUsuarioPorId", parametros, commandType: System.Data.CommandType.StoredProcedure);

                if (result != null)
                {
                    resp.Codigo = 200;
                    resp.Mensaje = "OK";
                    resp.Contenido = result;
                }
            }

            return Ok(resp);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion(Usuario entidad)
        {
            Respuesta resp = new Respuesta();

            using (var context = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=CHEMASFUT_DB;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                var result = await context.QueryAsync<Usuario>("IniciarSesion",
                    new { entidad.Email, entidad.Password},
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
