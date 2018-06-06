using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WT_WebMVCApp.Helpers;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Services
{
    public class WorkoutTrackerService : IWorkoutTrackerService
    {
        private readonly IWorkoutTrackerHttpClient _workoutTrackerHttpClient;

        public WorkoutTrackerService(IWorkoutTrackerHttpClient workoutTrackerHttpClient)
        {
            _workoutTrackerHttpClient = workoutTrackerHttpClient;
        }

        public async Task<WTServiceResponse<List<ExerciseVM>>> GetExercisesForUser(UserVM user)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.GetAsync($"/api/Exercises/user/{user.ID}");

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var exerciseVM = JsonConvert.DeserializeObject<List<ExerciseVM>>(content);

                return new WTServiceResponse<List<ExerciseVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = exerciseVM,
                };
            });
        }

        public async Task<WTServiceResponse<ExerciseVM>> SaveExerciseForUser(ExerciseVM exercise)
        {                    
            // serialize it
            var serializedExercise = JsonConvert.SerializeObject(exercise);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Exercises/user/{WorkotTrackerHelper.UserId}/exercise/{exercise.ID}", 
                                                new StringContent(serializedExercise, System.Text.Encoding.Unicode, "application/json"));
       
            return  HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<ExerciseVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = null,
                };
            });
        }


        private WTServiceResponse<T> HandleApiResponse<T>(HttpResponseMessage response, Func<WTServiceResponse<T>> onSuccess)
        {
            var serviceResponse = new WTServiceResponse<T>();
            serviceResponse.StatusCode = response.StatusCode;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Created:
                    {
                        return onSuccess();
                    }
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    {
                        serviceResponse.ResponseMessage = "AccessDenied";
                        return serviceResponse;
                    }
                default:
                    {
                        serviceResponse.ResponseMessage = $"A problem happened while calling the API: {response.ReasonPhrase}";
                        return serviceResponse;
                    }
            }
        }

        //ASYNC CALL
        private async Task<WTServiceResponse<T>> HandleApiResponse<T>(HttpResponseMessage response, Func<Task<WTServiceResponse<T>>> onSuccess)
        {
            var serviceResponse = new WTServiceResponse<T>();
            serviceResponse.StatusCode = response.StatusCode;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Created:
                    {
                        return await onSuccess();
                    }
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    {
                        serviceResponse.ResponseMessage = "AccessDenied";
                        return serviceResponse;
                    }
                default:
                    {
                        serviceResponse.ResponseMessage = $"A problem happened while calling the API: {response.ReasonPhrase}";
                        return serviceResponse;
                    }
            }
        }

    }
}
