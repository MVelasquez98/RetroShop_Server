using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model;
using RetroShop_Client.Model.Config;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Web.Http.Cors;

namespace RetroShop_Client.Controllers
{
    [Route("api/producto")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ProductController : ControllerBase
    {
        #region fields
        private readonly IOptions<ApiConfig> _config;
        private ProductoService.ProductoServiceClient _service;
        #endregion

        #region constructor
        public ProductController(IOptions<ApiConfig> config)
        {
            _config = config;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            GrpcChannel channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURL);
            _service = new ProductoService.ProductoServiceClient(channel);
        }
        #endregion

        #region endpoints
        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Producto producto)
        {
            try
            {
                ProductoDTO productoDTO = new ProductoDTO()
                {
                    IdProducto = producto.idProducto,
                    Nombre = producto.nombre,
                    Descripcion = producto.descripcion,
                    Precio = producto.precio,
                    CantidadDisponible = producto.cantidadDisponible,
                    FechaFabricacion = producto.fechaFabricacion,
                    IdCategoria = producto.idCategoria,
                    IdUsuario = producto.idUsuario,
                    PrecioBase = producto.precioBase,
                    EsSubasta = producto.esSubasta,
                    FechaLimite = producto.fechaLimite
                };
                foreach (var foto in producto.fotos)
                {
                    FotoDTO fotoDTO = new FotoDTO() { Nombre = foto.nombre, File = foto.file, IdFoto = foto.idFoto };
                    productoDTO.Fotos.Add(fotoDTO);
                }

                var response = await _service.addProductoAsync(productoDTO);
                return Ok(response.Producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        //GET api/<ProductController>/
        [HttpPost]
        [Route("byFilter")]
        public async Task<ActionResult> GetByFilter([FromBody] ProductoFilterDTO productoFilter)
        {
            try
            {
                var response = await _service.getByFilterAsync(productoFilter);
                if (response.Productos.Count == 0) return NoContent();
                return Ok(response.Productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Producto producto)
        {
            try
            {
                ProductoDTO productoDTO = new ProductoDTO()
                {
                    IdProducto = producto.idProducto,
                    Nombre = producto.nombre,
                    Descripcion = producto.descripcion,
                    Precio = producto.precio,
                    CantidadDisponible = producto.cantidadDisponible,
                    FechaFabricacion = producto.fechaFabricacion,
                    IdCategoria = producto.idCategoria,
                    IdUsuario = producto.idUsuario,
                    PrecioBase = producto.precioBase,
                    EsSubasta = producto.esSubasta,
                    FechaLimite = producto.fechaLimite
                };
                foreach (var foto in producto.fotos)
                {
                    FotoDTO fotoDTO = new FotoDTO() { Nombre = foto.nombre, File = foto.file, IdFoto = foto.idFoto };
                    productoDTO.Fotos.Add(fotoDTO);
                }

                var response = await _service.addProductoAsync(productoDTO);
                return Ok(response.Producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProductos()
        {
            try
            {
                var response = await _service.getAllProductosAsync(new Empty());//ese empty rari
                if (response.Productos.Count == 0) return NoContent();
                return Ok(response.Productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        [HttpGet]
        [Route("byUser")]
        public async Task<ActionResult> GetByUsuario(int idUsuario)
        {
            try
            {
                IdUsuarioDTO idUsuarioDTO = new IdUsuarioDTO() { IdUsuario = idUsuario };
                var response = await _service.getAllProductosByUserAsync(idUsuarioDTO);
                if (response.Productos.Count == 0) return NoContent();
                return Ok(response.Productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        //GET api/<ProductController>/
        [HttpGet]
        [Route("categoria")]
        public async Task<ActionResult> GetAllCategorias()
        {
            try
            {
                var response = await _service.getAllCategoriasAsync(new Empty());//ese empty rari
                if (response.Categorias.Count == 0) return NoContent();
                return Ok(response.Categorias);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("transaccion")]
        public async Task<ActionResult> Get([FromBody] GetProductosRequest transaccion)
        {
            try
            {
                var response = await _service.getComprasOVentasByUsuarioRequestAsync(transaccion);
                return Ok(response.Productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        #endregion
    }
}
