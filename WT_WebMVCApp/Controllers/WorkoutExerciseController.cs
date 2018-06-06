using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WT_WebMVCApp.Services;
using WT_WebMVCApp.Models;
using WT_WebMVCApp.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IO;

namespace WT_WebMVCApp.Controllers
{
    public class WorkoutExerciseController : Controller
    {
        private readonly ILogger<WorkoutSessionController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public WorkoutExerciseController(ILogger<WorkoutSessionController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };
            var response = await _workoutTrackerService.GetExercisesForUser(UserVM);
            //set image path relative to api's URL ... 
            response.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);

            ViewData["Category"] = GetCategorySelectList();

            return View(response.ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveExercise([FromForm] ExerciseVM exercise)
        {
            var attrs = JsonConvert.DeserializeObject<List<ExerciseAttributeVM>>(exercise.AttributesSerialized);
            exercise.Attributes = attrs;
            if (exercise.Image != null)
            {
                exercise.ImagePath = exercise.Image.FileName;
                using (var fileStream = exercise.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        exercise.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.SaveExerciseForUser(exercise);

            return Json(response);
        }

        private SelectList GetCategorySelectList()
        {
            var enumData = Enum.GetValues(typeof(Category)).OfType<Enum>()
                                 .Select(x => new SelectListItem
                                 {
                                     Text = Enum.GetName(typeof(Category), x),
                                     Value = (Convert.ToInt32(x)).ToString()
                                 });
            var selectList = new SelectList(enumData, "Value", "Text");


            return selectList;
        }
    }
}