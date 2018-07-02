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
using System.IO;
using WT_WebMVCApp.Models.WorkoutAssets;

namespace WT_WebMVCApp.Controllers
{
    public class WorkoutProgramController : Controller
    {
        private readonly ILogger<WorkoutProgramController> _logger;
        private readonly IWorkoutTrackerService _workoutTrackerService;

        public WorkoutProgramController(ILogger<WorkoutProgramController> logger, IWorkoutTrackerService workoutTrackerService)
        {
            _logger = logger;
            _workoutTrackerService = workoutTrackerService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.UserId };

            var response = await _workoutTrackerService.GetProgramsForUser(UserVM);
            //set image path relative to api's URL ... 
            response.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);
            response.ViewModel.ForEach(item => item.WorkoutRoutines.ToList()
                                                .ForEach(img => img.ImagePath = WorkotTrackerHelper.ApiUrl + img.ImagePath));

            //Get routines for the user
            var routineResponse = await _workoutTrackerService.GetRoutinesForUser(UserVM);
            //set image path relative to api's URL ... 
            routineResponse.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);
            routineResponse.ViewModel.ForEach(item => item.Exercises.ToList()
                                                            .ForEach(img => img.ImagePath = WorkotTrackerHelper.ApiUrl + img.ImagePath));
            ViewData["Routines"] = routineResponse.ViewModel;

            return View(response.ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgram([FromForm] WorkoutProgramVM program)
        {
            var routines = JsonConvert.DeserializeObject<List<WorkoutRoutineVM>>(program.RoutinesSerialized);
            program.WorkoutRoutines = routines;

            if (program.Image != null)
            {
                program.ImagePath = program.Image.FileName;
                using (var fileStream = program.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        program.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.AddProgram(program);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProgram([FromForm] WorkoutProgramVM program)
        {
            var routines = JsonConvert.DeserializeObject<List<WorkoutRoutineVM>>(program.RoutinesSerialized);
            program.WorkoutRoutines = routines;
            if (program.Image != null)
            {
                program.ImagePath = program.Image.FileName;
                using (var fileStream = program.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        program.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.SaveProgram(program);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProgram([FromForm] int ID)
        {
            var response = await _workoutTrackerService.DeleteProgram(ID);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRoutinesForProgram([FromForm] UpdateRoutinesForProgramModel routinesModel)
        {
            var routineIds = JsonConvert.DeserializeObject<List<int>>(routinesModel.SerializedRoutineIds);
            routinesModel.Routines = new List<WorkoutRoutineVM>();
            foreach(var routineId in routineIds)
            {
                var wr = new WorkoutRoutineVM();
                wr.ID = routineId;
                wr.PlannedDates = null;
                routinesModel.Routines.Add(wr);
            }
            var response = await _workoutTrackerService.SaveRoutinesForProgram(routinesModel);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDatesForProgramRoutines([FromForm] WorkoutRoutineVM routineModel)
        {
            var response = await _workoutTrackerService.UpdateDatesForProgramRoutines(routineModel);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateOrDeactivateProgram([FromForm] WorkoutProgramVM programModel)
        {
            var response = await _workoutTrackerService.ActivateOrDeactivateProgram(programModel);

            return Json(response);
        }

    }
}