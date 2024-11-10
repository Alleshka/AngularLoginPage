using AngularLoginPage.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularLoginPage.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly AngularLoginPageDbContext _context;

        public CountryController(AngularLoginPageDbContext context)
        {
            _context = context;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(await _context.Countries.OrderBy(x=>x.Name).ToListAsync());
        }


        [HttpGet("{countryId}/provinces")]
        public async Task<IActionResult> GetProvinces(Guid countryId)
        {
            return Ok(await _context.Provinces.Where(p => p.CountryId == countryId).OrderBy(x=>x.Name).ToListAsync());
        }
    }
}
