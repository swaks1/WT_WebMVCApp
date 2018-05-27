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


        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };
            var response = await _workoutTrackerService.GetExercisesForUser(UserVM);
            response.ViewModel.ForEach(item => item.ImagePath = "https://static2.fjcdn.com/comments/Heres+without+the+quote+_395729dda93f0ff281812ec84603e63a.jpg");

            ViewData["Category"] = GetCategorySelectList();

            return View(response.ViewModel);
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