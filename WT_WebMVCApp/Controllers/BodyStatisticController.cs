using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Index(DateTime? date)
        {
            var UserVM = new UserVM { ID = WorkotTrackerHelper.GetUserId(User) };

            if (date == null)
            {
                date = DateTime.Now;
            }
            ViewData["Date"] = date;

            var response =  await _workoutTrackerService.GetBodyStatistucForMonth(UserVM, date.Value.Month,date.Value.Year);
            //set image path relative to api's URL ... 
            response.ViewModel.ForEach(item => item.ImagePath = WorkotTrackerHelper.ApiUrl + item.ImagePath);

            var attributeTemplates = await _workoutTrackerService.GetAttributeTemplates(UserVM);
            ViewData["AttributeTemplates"] = attributeTemplates.ViewModel;

            return View(response.ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStatistic([FromForm] BodyStatisticVM statistic)
        {
            var attrs = JsonConvert.DeserializeObject<List<BodyStatAttributeVM>>(statistic.AttributesSerialized);
            statistic.BodyStatAttributes = attrs;
            if (statistic.Image != null)
            {
                statistic.ImagePath = statistic.Image.FileName;
                using (var fileStream = statistic.Image.OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        statistic.ImageBytes = ms.ToArray();
                    }
                }
            }

            var response = await _workoutTrackerService.AddStatistic(statistic);

            return Json(response);
        }

    }
}