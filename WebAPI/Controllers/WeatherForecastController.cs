using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        string[] nombres = new[] {"Fabian","Carlos", "Diana","Nadia"};
        return nombres;
    }
}
