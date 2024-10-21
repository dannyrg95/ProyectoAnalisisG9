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
        public IActionResult RegistrarUsuario(Usuario entidad)
        {
            using(var context = new SqlConnection("Server=LIED95\\SQLEXPRESS;Database=ChemasfutCR;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                var result = context.Execute("RegistrarUsuario", 
                    new { entidad.Nombre, entidad.Apellido, entidad.Fecha_Nacimiento, entidad.Teléfono, entidad.Email, entidad.Contraseña, entidad.Identificacion}, 
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            return Ok("Todo bien");
        } 

    }
}
