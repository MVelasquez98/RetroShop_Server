using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetroShop_Client.Model.Config;

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
            GrpcChannel channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURLTransaccion);
            _service = new TransaccionService.TransaccionServiceClient(channel);
        }
        #endregion

        #region endpoints

        // POST api/<WalletController>
        [HttpPost]
        [Route("compra")]
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
        [HttpPost]
        [Route("transaccion")]
        public async Task<ActionResult> Get([FromBody] GetTransaccionesRequest transaccion)
        {
            try
            {
                var response = await _service.getComprasOVentasByUsuarioRequestAsync(transaccion);
                return Ok(response.Transacciones);
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
