using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WT_WebMVCApp.Services;
using WT_WebMVCApp.Models;
using WT_WebMVCApp.Helpers;
using Newtonsoft.Json;

namespace WT_WebMVCApp.Controllers
{
    public class TimelineController : Controller
    {
        private readonly ILogger<TimelineController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public TimelineController(ILogger<TimelineController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;     

            var response = await _workoutTrackerService.GetSessionsForMonth(UserVM,year, month);
            

            return View(response.ViewModel);
        }
        

    }
}