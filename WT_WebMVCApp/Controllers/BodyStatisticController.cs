using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WT_WebMVCApp.Helpers;
using WT_WebMVCApp.Models;
using WT_WebMVCApp.Services;

namespace WT_WebMVCApp.Controllers
{
    public class BodyStatisticController : Controller
    {
        private readonly ILogger<BodyStatisticController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public BodyStatisticController(ILogger<BodyStatisticController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? month)
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };

            if (month == null)
                month = DateTime.Now.Month;

            var response =  await _workoutTrackerService.GetBodyStatistucForMonth(UserVM, month.Value);

            return View(response.ViewModel);
        }

    }
}