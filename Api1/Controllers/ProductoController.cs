using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api1.Models;
using Microsoft.AspNetCore.Cors;

namespace Api1.Controllers
{
    [EnableCors("mCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly Api1Context _dbcontext;

        public ProductoController(Api1Context _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista = _dbcontext.Productos.Include(c => c.oCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);
            if(oProducto == null)
            {
                return BadRequest("Producto no Encontrado");
            }
            try
            {
                oProducto = _dbcontext.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto obt)
        {
            try
            {
                _dbcontext.Productos.Add(obt);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto obt)
        {
            Producto oProducto = _dbcontext.Productos.Find(obt.IdProducto);
            if (oProducto == null)
            {
                return BadRequest("Producto no Encontrado");
            }
            try
            {
                oProducto.CodigoBarra = obt.CodigoBarra is null ? oProducto.CodigoBarra : obt.CodigoBarra;
                oProducto.Descripcion = obt.Descripcion is null ? oProducto.Descripcion : obt.Descripcion;
                oProducto.Marca = obt.Marca is null ? oProducto.Marca : obt.Marca;
                oProducto.IdCategoria = obt.IdCategoria is null ? oProducto.IdCategoria : obt.IdCategoria;
                oProducto.Precio = obt.Precio is null ? oProducto.Precio : obt.Precio;
                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);
            if (oProducto == null)
            {
                return BadRequest("Producto no Encontrado");
            }
            try
            {
                _dbcontext.Productos.Remove(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }





    }
}
