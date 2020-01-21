using AutoMapper;
using HomeDashboard.Library.Models;
using HomeDashboard.Library.Models.Hue;
using HomeDashboard.Models;
using kriez.HomeDashboard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HueController : ControllerBase
    {
        private DatabaseContext _context;
        private readonly IMapper _mapper;
        public HueController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HueResponse response = new HueResponse();
            response.LastUpdated = (await _context.UpdateTables.SingleAsync(u => u.Key.Equals(UpdateType.Hue))).LastUpdated;

            response.Hues = _context.HueScenes.Include(s => s.Lights).Select(s => _mapper.Map<HueSceneDto>(s)).ToList();

            return Ok(response);
        }
    }
}