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
    public class WorkoutSessionController : Controller
    {
        private readonly ILogger<WorkoutSessionController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public WorkoutSessionController(ILogger<WorkoutSessionController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };
            var sessionRequest = new WorkoutSessionRequest { User = UserVM, CurrentDate = DateTime.Now };

            var response = await _workoutTrackerService.GetSessionForDay(sessionRequest);

            //set image path relative to api's URL ... 
            response.ViewModel.ConcreteExercises.ToList().ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);

            return View(response.ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateConcreteExerciseAttributes([FromForm] ConcreteExerciseVM exercise)
        {
            var attrs = JsonConvert.DeserializeObject<List<ConcreteExerciseAttributeVM>>(exercise.AttributesSerialized);
            exercise.Attributes = attrs;

            var response = await _workoutTrackerService.UpdateConcreteExerciseAttributes(exercise);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConcreteExercise([FromForm] int ID,[FromForm] int sessionId)
        {
            var response = await _workoutTrackerService.DeleteConcreteExercise(ID,sessionId);

            return Json(response);
        }

        
    }
}