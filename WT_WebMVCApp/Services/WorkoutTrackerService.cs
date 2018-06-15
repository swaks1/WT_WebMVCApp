﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WT_WebMVCApp.Helpers;
using WT_WebMVCApp.Models;
using WT_WebMVCApp.Models.WorkoutAssets;

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

        public async Task<WTServiceResponse<WorkoutRoutineVM>> AddRoutine(WorkoutRoutineVM routine)
        {
            // serialize it
            var serializedRoutine = JsonConvert.SerializeObject(routine);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Routines/user/{WorkotTrackerHelper.UserId}",
                                                new StringContent(serializedRoutine, System.Text.Encoding.Unicode, "application/json"));

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var routineVM = JsonConvert.DeserializeObject<WorkoutRoutineVM>(content);

                return new WTServiceResponse<WorkoutRoutineVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = routineVM,
                };
            });
        }

        public async Task<WTServiceResponse<string>> SaveRoutine(WorkoutRoutineVM routine)
        {
            // serialize it
            var serializedRoutine = JsonConvert.SerializeObject(routine);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Routines/user/{WorkotTrackerHelper.UserId}/routine/{routine.ID}",
                                                new StringContent(serializedRoutine, System.Text.Encoding.Unicode, "application/json"));

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> DeleteRoutine(int id)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.DeleteAsync($"/api/Routines/user/{WorkotTrackerHelper.UserId}/routine/{id}");

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> SaveExercisesForRoutine(UpdateExerciseForRoutineModel exercisesModel)
        {
            // serialize it
            var serializedExercises = JsonConvert.SerializeObject(exercisesModel.ExerciseIds);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Routines/user/{WorkotTrackerHelper.UserId}/routine/{exercisesModel.ID}/exercises",
                                                new StringContent(serializedExercises, System.Text.Encoding.Unicode, "application/json"));

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


        #region Sessions

        public async Task<WTServiceResponse<WorkoutSessionVM>> GetSessionForDay(WorkoutSessionRequest workoutSessionRequest)
        {
            var serializedRequest = JsonConvert.SerializeObject(workoutSessionRequest);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{workoutSessionRequest.User.ID}/SessionForDay",
                                                new StringContent(serializedRequest, System.Text.Encoding.Unicode, "application/json"));


            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var session = JsonConvert.DeserializeObject<WorkoutSessionVM>(content);

                return new WTServiceResponse<WorkoutSessionVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = session
                };
            });
        }


        public async Task<WTServiceResponse<string>> UpdateConcreteExerciseAttributes(ConcreteExerciseVM exercise)
        {
            var request = new WorkoutSessionRequest { ConcreteExercises = new List<ConcreteExerciseVM>() };
            request.ConcreteExercises.Add(exercise);
        
            // serialize it
            var serializedRequest = JsonConvert.SerializeObject(request);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Sessions/user/{WorkotTrackerHelper.UserId}/session/{exercise.WorkoutSessionID}/UpdateConcreteExercisesAttributes",
                                                new StringContent(serializedRequest, System.Text.Encoding.Unicode, "application/json"));

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> DeleteConcreteExercise(int exerciseId, int sessionId)
        {
            var request = new List<int>();
            request.Add(exerciseId);
            var serializedRequest = JsonConvert.SerializeObject(request);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{WorkotTrackerHelper.UserId}/session/{sessionId}/DeleteConcreteExercises",
                            new StringContent(serializedRequest, System.Text.Encoding.Unicode, "application/json"));

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
