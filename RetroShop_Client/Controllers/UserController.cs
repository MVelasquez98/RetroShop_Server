using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model.Config;
using RetroShop_Client.Protos;

namespace RetroShop_Client.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<ApiConfig> _config;
        private UsuarioService.UsuarioServiceClient _service;
        #region mocks
        private List<UsuarioDTO> _usuarios = new List<UsuarioDTO>();
        private readonly UsuarioDTO usuarioResponse = new UsuarioDTO()
        {
            Apellido = "Velasquez",
            Nombre = "Matias",
            Dni = "41716615",
            Email = "matiasvelasquez840@gmail.com",
            Clave = "pass"
        };

        public UserController(IOptions<ApiConfig> config)
        {
            _config = config;
            using var channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURL);
            _service = new UsuarioService.UsuarioServiceClient(channel);
        }


        #endregion


        #region endpoints
        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioDTO usuario)
        {
            try
            {
                //var response = _service.addUsuario(usuario);
                _usuarios.Add(usuario);
                return Ok(_usuarios.Count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //GET
        [HttpGet]
        [Route("login")] public async Task<ActionResult> Login([FromBody] GetByUsuarioYClaveRequest usuarioLogin)
        {
            
            //var response = _service.getByUsuarioYClaveRequest(usuarioLogin);
            return Ok(usuarioResponse);
        }
        #endregion
    }
}
