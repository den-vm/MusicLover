using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MuloApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            
            return new JsonResult(new {Name = "Denis", Role = "BackEnd"});
        }
    }
}