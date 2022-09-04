using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model.Config;
using System;
using System.Diagnostics.Eventing.Reader;

namespace RetroShop_Client.Controllers
{
    [Route("api/producto")]
    [ApiController]
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
            GrpcChannel channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURLProducto);
            _service = new ProductoService.ProductoServiceClient(channel);
        }
        #endregion

        #region endpoints
        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductoDTO producto)
        {
            try
            {
                var response = await _service.addProductoAsync(producto);
                return Ok(response.Producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        //GET api/<ProductController>/
        [HttpGet]
        [Route("byfilter")]
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
        #endregion
    }
}
