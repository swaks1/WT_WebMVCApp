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
using WT_WebMVCApp.Models.WorkoutAssets;

namespace WT_WebMVCApp.Controllers
{
    public class WorkoutRoutineController : Controller
    {
        private readonly ILogger<WorkoutRoutineController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public WorkoutRoutineController(ILogger<WorkoutRoutineController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.GetUserId(User) };
            var routineResponse = await _workoutTrackerService.GetRoutinesForUser(UserVM);

            //set image path relative to api's URL ... 
            routineResponse.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);
            routineResponse.ViewModel.ForEach(item => item.Exercises.ToList()
                                                            .ForEach(img => img.ImagePath = WorkotTrackerHelper.ApiUrl + img.ImagePath));


            //Get exercises for the user
            var exercisesResposne = await _workoutTrackerService.GetExercisesForUser(UserVM);
            //set image path relative to api's URL ... 
            exercisesResposne.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);
            ViewData["Exercises"] = exercisesResposne.ViewModel;


            return View(routineResponse.ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoutine([FromForm] WorkoutRoutineVM routine)
        {
            var exercises = JsonConvert.DeserializeObject<List<ExerciseVM>>(routine.ExercisesSerialized);
            routine.Exercises = exercises;
            if (routine.Image != null)
            {
                routine.ImagePath = routine.Image.FileName;
                using (var fileStream = routine.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        routine.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.AddRoutine(routine);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRoutine([FromForm] WorkoutRoutineVM routine)
        {
            var exercises = JsonConvert.DeserializeObject<List<ExerciseVM>>(routine.ExercisesSerialized);
            routine.Exercises = exercises;
            if (routine.Image != null)
            {
                routine.ImagePath = routine.Image.FileName;
                using (var fileStream = routine.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        routine.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.SaveRoutine(routine);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoutine([FromForm] int ID)
        {
            var response = await _workoutTrackerService.DeleteRoutine(ID);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveExercisesForRoutine([FromForm] UpdateExerciseForRoutineModel exercisesModel)
        {
            exercisesModel.ExerciseIds = JsonConvert.DeserializeObject<List<int>>(exercisesModel.SerializedExerciseIds);
            var response = await _workoutTrackerService.SaveExercisesForRoutine(exercisesModel);

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