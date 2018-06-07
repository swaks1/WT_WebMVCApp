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


        #region Exercises

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

        public async Task<WTServiceResponse<ExerciseVM>> AddExercise(ExerciseVM exercise)
        {
            // serialize it
            var serializedExercise = JsonConvert.SerializeObject(exercise);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Exercises/user/{WorkotTrackerHelper.UserId}",
                                                new StringContent(serializedExercise, System.Text.Encoding.Unicode, "application/json"));

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var exerciseVM = JsonConvert.DeserializeObject<ExerciseVM>(content);

                return new WTServiceResponse<ExerciseVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = exerciseVM,
                };
            });
        }

        public async Task<WTServiceResponse<string>> SaveExercise(ExerciseVM exercise)
        {
            // serialize it
            var serializedExercise = JsonConvert.SerializeObject(exercise);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Exercises/user/{WorkotTrackerHelper.UserId}/exercise/{exercise.ID}",
                                                new StringContent(serializedExercise, System.Text.Encoding.Unicode, "application/json"));

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> DeleteExercise(int id)
        {

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.DeleteAsync($"/api/Exercises/user/{WorkotTrackerHelper.UserId}/exercise/{id}");

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        #endregion



        #region Routines

        public async Task<WTServiceResponse<List<WorkoutRoutineVM>>> GetRoutinesForUser(UserVM user)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.GetAsync($"/api/Routines/user/{user.ID}");

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var routines = JsonConvert.DeserializeObject<List<WorkoutRoutineVM>>(content);

                return new WTServiceResponse<List<WorkoutRoutineVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = routines,
                };
            });
        }

        #endregion



        #region Handle Response Methods (sync and async)

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

        #endregion

    }
}
