using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RetroShop_Client.Protos;

namespace RetroShop_Client.Controllers
{
    [Route("api/producto")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string GrpcChannelURL = "https://localhost:7110";
        #region mocks
        private List<ProductoDTO> _products = new List<ProductoDTO>();
        //private readonly ServerResponse serverResponseOk = new ResProductoDTO()
        //{
        //};
        private readonly List<CategoriaDTO> _categorias = new List<CategoriaDTO>()
        {
            new CategoriaDTO() { IdCategoria=1, Categoria="Vintage" }
        };
        #endregion

        #region endpoints
        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductoDTO producto)
        {
            try
            {
                //using var channel = GrpcChannel.ForAddress(GrpcChannelURL);
                //var client = new ProductoService.ProductoServiceClient(channel);
                //var response = client.Post(producto);
                _products.Add(producto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
                //return BadRequest(new ResProductoDTO() { Producto = null, Response = new ServerResponse() { Code = 400, Msg = "Error" } });
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{idProducto}")]
        public void Put(int idProducto, [FromBody] ProductoDTO producto)
        {
        }
        //GET api/<ProductController>/
        [HttpGet]
        public async Task<ActionResult> GetByFilter([FromBody] ProductoFilterDTO productoFilter)
        {
            return Ok(_products);
        }
        //GET api/<ProductController>/
        [HttpGet]
        [Route("categoria")]
        public async Task<ActionResult> GetAllCategorias()
        {
            return Ok(_categorias);
        }
        #endregion
    }
}
