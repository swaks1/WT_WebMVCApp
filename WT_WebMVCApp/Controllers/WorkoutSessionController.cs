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
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;

namespace WT_WebMVCApp.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string queryDate)
        {
            await WriteOutIdentityInformation();

            var currentDate = DateTime.Now;
            if(queryDate != null)
            {
                DateTime parsedDate;
                if(DateTime.TryParse(queryDate, out parsedDate))
                {
                    currentDate = parsedDate;
                }
            }
            var UserVM = new UserVM { ID = WorkotTrackerHelper.GetUserId(User) };
            var sessionRequest = new WorkoutSessionRequest { User = UserVM, CurrentDate = currentDate };

            var response = await _workoutTrackerService.GetSessionForDay(sessionRequest);
            //set image path relative to api's URL ... 
            response.ViewModel.ConcreteExercises.ToList().ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);

            //Get exercises for the user
            var exercisesResposne = await _workoutTrackerService.GetExercisesForUser(UserVM);
            //set image path relative to api's URL ... 
            exercisesResposne.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);
            ViewData["Exercises"] = exercisesResposne.ViewModel;

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
        public async Task<IActionResult> SaveExercisesForSession([FromForm] WorkoutSessionRequest workoutSessionRequest)
        {
            workoutSessionRequest.Exercises = new List<ExerciseVM>();
            var exerciseIDs = JsonConvert.DeserializeObject<List<int>>(workoutSessionRequest.SerializedExerciseIds);
            exerciseIDs.ForEach(item => workoutSessionRequest.Exercises.Add(new ExerciseVM { ID = item }));

            var response = await _workoutTrackerService.SaveExercisesForSession(workoutSessionRequest);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRoutinesForSession([FromForm] WorkoutSessionRequest workoutSessionRequest)
        {
            workoutSessionRequest.Routines = new List<WorkoutRoutineVM>();
            var routineIDs = JsonConvert.DeserializeObject<List<int>>(workoutSessionRequest.SerializedRoutineIds);
            routineIDs.ForEach(item => workoutSessionRequest.Routines.Add(new WorkoutRoutineVM { ID = item }));

            var response = await _workoutTrackerService.SaveRoutinesForSession(workoutSessionRequest);

            return Json(response);
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
        public async Task<IActionResult> DeleteConcreteExercise([FromForm] int ID, [FromForm] int sessionId)
        {
            var response = await _workoutTrackerService.DeleteConcreteExercise(ID, sessionId);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConcreteExercisesFromRoutine([FromForm] int ID, [FromForm] int sessionId)
        {
            var response = await _workoutTrackerService.DeleteConcreteExercisesFromRoutine(ID, sessionId);

            return Json(response);
        }

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}