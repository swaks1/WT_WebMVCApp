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
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;

namespace WT_WebMVCApp.Controllers
{
    [Authorize]
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
            await WriteOutIdentityInformation();

            var UserVM = new UserVM { ID = WorkotTrackerHelper.GetUserId(User) };
            var response = await _workoutTrackerService.GetExercisesForUser(UserVM);

            if(response.ResponseMessage == "AccessDenied" || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            //set image path relative to api's URL ... 
            response.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);

            ViewData["Category"] = GetCategorySelectList();

            return View(response.ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise([FromForm] ExerciseVM exercise)
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

            var response = await _workoutTrackerService.AddExercise(exercise);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var response = await _workoutTrackerService.SaveExercise(exercise);

            return Json(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExercise([FromForm] int ID)
        {
            var response = await _workoutTrackerService.DeleteExercise(ID);

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