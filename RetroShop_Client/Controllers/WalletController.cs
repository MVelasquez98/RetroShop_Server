using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model.Config;
using RetroShop_Client.Protos;

namespace RetroShop_Client.Controllers
{
    [Route("api/billetera")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        #region fields
        private readonly IOptions<ApiConfig> _config;
        private TransaccionService.TransaccionServiceClient _service;
        #endregion

        #region constructor
        public WalletController(IOptions<ApiConfig> config)
        {
            _config = config;
            using var channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURL);
            _service = new TransaccionService.TransaccionServiceClient(channel);
        }
        #endregion

        #region endpoints

        // POST api/<WalletController>
        [HttpPost]
        [Route("transaccion")]
        public async Task<ActionResult> Post([FromBody] TransaccionDTO transaccion)
        {
            try
            {
                var response = await _service.addTransaccionAsync(transaccion);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        // GET api/<WalletController>
        [HttpGet]
        [Route("transaccion")]
        public async Task<ActionResult> Get([FromBody] GetTransaccionesRequest transaccion)
        {
            try
            {
                var response = await _service.getComprasOVentasByUsuarioRequestAsync(transaccion);
                return Ok(response);
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
