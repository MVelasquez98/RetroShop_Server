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
            GrpcChannel channel = GrpcChannel.ForAddress(_config.Value.GrpcChannelURL);
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
                if (response.ServerResponse.Cod == 500) throw new Exception(response.ServerResponse.Msg);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500, ex.Message);
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
                if (response.ServerResponse.Cod == 500) throw new Exception(response.ServerResponse.Msg);
                return Ok(response.Transacciones);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
