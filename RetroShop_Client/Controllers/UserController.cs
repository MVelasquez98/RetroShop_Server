using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model.Config;
using System.Web.Http.Cors;

namespace RetroShop_Client.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    [EnableCors(origins: "http://localhost:3000/", headers: "*", methods: "*")]

    public class UserController : ControllerBase
    {
        #region fields
        private readonly IOptions<ApiConfig> _config;
        private UsuarioService.UsuarioServiceClient _service;
        #endregion

        #region constructor
        public UserController(IOptions<ApiConfig> config)
        {
            _config = config;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            GrpcChannel channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURLUsuario);
            _service = new UsuarioService.UsuarioServiceClient(channel);
        }
        #endregion

        #region endpoints
        [HttpGet]
        public async Task<ActionResult> Get(int idUsuario)
        {
            try
            {
                GetUsuarioRequest idUsuarioDTO = new GetUsuarioRequest() { IdUsuario = idUsuario };
                var response = await _service.getUsuarioAsync(idUsuarioDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var response = await _service.addUsuarioAsync(usuario);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] GetByUsuarioYClaveRequest usuarioLogin)
        {
            try
            {
                var response = await _service.getByUsuarioYClaveRequestAsync(usuarioLogin);
                if (response.Usuario == null) return NotFound();
                return Ok(response.Usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
        // PUT api/<UserController>
        [HttpPut]
        [Route("saldo")]
        public async Task<ActionResult> CargarSaldo([FromBody] UpdateUsuarioCargaSaldoRequest usuario)
        {
            try
            {
                var response = await _service.updateUsuarioCargaSaldoRequestAsync(usuario);
                return Ok(response.Usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        // PUT api/<UserController>
        [HttpPut]
        [Route("comprar")]
        public async Task<ActionResult> ComprarProducto([FromBody] UpdateUsuarioCompraProductoRequest usuario)
        {
            try
            {
                var response = await _service.updateUsuarioCompraProductoRequestAsync(usuario);
                return Ok(response.Usuario);
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
