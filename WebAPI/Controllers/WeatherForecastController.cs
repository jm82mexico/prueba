using Dominio;
using Microsoft.AspNetCore.Mvc;
using Persitencia;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly CursosOnlineContext context;

    public WeatherForecastController(CursosOnlineContext _context)
    {
        this.context = _context;
    }
    
    [HttpGet]
    public IEnumerable<Curso> Get()
    {
        return context.Curso.ToList();
    }
}
