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

namespace WT_WebMVCApp.Controllers
{
    public class WorkoutSessionController : Controller
    {
        private readonly ILogger<WorkoutSessionController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public WorkoutSessionController(ILogger<WorkoutSessionController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}