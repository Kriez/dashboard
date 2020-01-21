using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeDashboard.Library.Models;
using HomeDashboard.Library.Models.Calendar;
using HomeDashboard.Models;
using kriez.HomeDashboard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeDashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private DatabaseContext _context;
        private readonly IMapper _mapper;
        public CalendarController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CalendarResponse response = new CalendarResponse();
            response.LastUpdated = (await _context.UpdateTables.SingleAsync(u => u.Key.Equals(UpdateType.Calendar))).LastUpdated;
            response.Calendars = await _context.CalendarItems.OrderBy(c => c.Start).Select(c => _mapper.Map<CalendarItemDto>(c)).ToListAsync();
            return Ok(response);
        }
    }
}