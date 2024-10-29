using ChemasfutCR_API.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ChemasfutCR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpPost]
        [Route("RegistrarProducto")]
        public async Task<IActionResult> RegistrarProducto(Producto entidad)
        {
            Respuesta resp = new Respuesta();

            try
            {
                using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
                {
                    var result = await context.ExecuteAsync("RegistrarProducto", new {entidad.Nombre_Producto,entidad.Descripcion,entidad.Precio,entidad.ID_Categoria,entidad.Stock,entidad.Talla,entidad.Color}, 
                        commandType:System.Data.CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        resp.Codigo = 200;
                        resp.Mensaje = "OK";
                        resp.Contenido = true;
                    } 
                    else 
                    {
                        resp.Codigo = 404;
                        resp.Mensaje = "El registro no se completó";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                resp.Codigo = 500;
                resp.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return Ok(resp);
        }

        [HttpGet]
        [Route("ObtenerProductos")]
        public async Task<IActionResult> ObtenerProductos()
        {
            Respuesta resp = new Respuesta();

            try
            {
                using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
                {
                    await context.OpenAsync();

                    var result = await context.QueryAsync<Producto>("ObtenerTodosLosProductos", commandType: System.Data.CommandType.StoredProcedure);

                    if (result != null)
                    {
                        resp.Codigo = 200;
                        resp.Mensaje = "OK";
                        resp.Contenido = result;
                    }
                    else
                    {
                        resp.Codigo = 404;
                        resp.Mensaje = "No se encontraron productos";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Codigo = 500;
                resp.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return Ok(resp);
        }

        [HttpGet]
        [Route("ObtenerProductoPorId")]
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            Respuesta resp = new Respuesta();

            try
            {
                using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
                {
                    await context.OpenAsync();

                    var parametros = new {ID_Producto = id};

                    var result = await context.QueryFirstOrDefaultAsync<Producto>("ObtenerProductoPorId", parametros, commandType: System.Data.CommandType.StoredProcedure);

                    if (result != null)
                    {
                        resp.Codigo = 200;
                        resp.Mensaje = "OK";
                        resp.Contenido = result;
                    }
                    else
                    {
                        resp.Codigo = 404;
                        resp.Mensaje = "No se encontraron productos.";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Codigo = 500;
                resp.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return Ok(resp);
        }

        [HttpPost]
        [Route("ActualizarProducto")]
        public async Task<IActionResult> ActualizarProducto(Producto entidad)
        {
            Respuesta resp = new Respuesta();

            try
            {
                using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
                {
                    var result = await context.ExecuteAsync("ActualizarProducto", 
                        new { entidad.ID_Producto, entidad.Nombre_Producto, entidad.Descripcion, entidad.Precio, entidad.ID_Categoria, entidad.Stock, entidad.Talla, entidad.Color },
                        commandType: System.Data.CommandType.StoredProcedure);
                    
                    if (result > 0)
                    {
                        resp.Codigo = 200;
                        resp.Mensaje = "OK";
                        resp.Contenido = true;
                    }
                    else
                    {
                        resp.Codigo = 404;
                        resp.Mensaje = "No se realizaron los cambios.";
                        resp.Contenido = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Codigo = 500;
                resp.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return Ok(resp);
        }

        [HttpPost]
        [Route("InhabilitarProducto")]
        public async Task<IActionResult> InhabilitarProducto(Producto entidad)
        {
            Respuesta resp = new Respuesta();

            try
            {
                using (var context = new SqlConnection("Server=localhost;Database=CHEMASFUT_DB;User Id=SA;Password=chemasfut12*;TrustServerCertificate=true;"))
                {
                    var result = await context.ExecuteAsync("InactivarProducto", 
                        new { entidad.ID_Producto }, commandType: System.Data.CommandType.StoredProcedure);
                    
                    if (result > 0)
                    {
                        resp.Codigo = 200;
                        resp.Mensaje = "OK";
                        resp.Contenido = true;
                    }
                    else
                    {
                        resp.Codigo = 404;
                        resp.Mensaje = "No se realizaron los cambios.";
                        resp.Contenido = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Codigo = 500;
                resp.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return Ok(resp);
        }
    }
}