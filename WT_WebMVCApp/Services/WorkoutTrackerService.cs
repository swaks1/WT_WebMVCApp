using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkoutTrackerHttpClient _workoutTrackerHttpClient;

        public WorkoutTrackerService(IWorkoutTrackerHttpClient workoutTrackerHttpClient, IHttpContextAccessor httpContextAccessor)
        {
            _workoutTrackerHttpClient = workoutTrackerHttpClient;
            _httpContextAccessor = httpContextAccessor;
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

            var response = await httpClient.PostAsync($"/api/Exercises/user/{UserId()}",
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

            var response = await httpClient.PutAsync($"/api/Exercises/user/{UserId()}/exercise/{exercise.ID}",
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

            var response = await httpClient.DeleteAsync($"/api/Exercises/user/{UserId()}/exercise/{id}");

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

            var response = await httpClient.PostAsync($"/api/Routines/user/{UserId()}",
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

            var response = await httpClient.PutAsync($"/api/Routines/user/{UserId()}/routine/{routine.ID}",
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

            var response = await httpClient.DeleteAsync($"/api/Routines/user/{UserId()}/routine/{id}");

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

            var response = await httpClient.PostAsync($"/api/Routines/user/{UserId()}/routine/{exercisesModel.ID}/exercises",
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

        public async Task<WTServiceResponse<string>> SaveExercisesForSession(WorkoutSessionRequest workoutSessionRequest)
        {
            // serialize it
            var serializedRequest = JsonConvert.SerializeObject(workoutSessionRequest);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{UserId()}/CreateOrUpdate",
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

        public async Task<WTServiceResponse<string>> SaveRoutinesForSession(WorkoutSessionRequest workoutSessionRequest)
        {
            // serialize it
            var serializedRequest = JsonConvert.SerializeObject(workoutSessionRequest);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{UserId()}/CreateOrUpdate",
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

        public async Task<WTServiceResponse<string>> UpdateConcreteExerciseAttributes(ConcreteExerciseVM exercise)
        {
            var request = new WorkoutSessionRequest { ConcreteExercises = new List<ConcreteExerciseVM>() };
            request.ConcreteExercises.Add(exercise);

            // serialize it
            var serializedRequest = JsonConvert.SerializeObject(request);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Sessions/user/{UserId()}/session/{exercise.WorkoutSessionID}/UpdateConcreteExercisesAttributes",
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

            var response = await httpClient.PostAsync($"/api/Sessions/user/{UserId()}/session/{sessionId}/DeleteConcreteExercises",
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

        public async Task<WTServiceResponse<string>> DeleteConcreteExercisesFromRoutine(int routineId, int sessionId)
        {
            var request = new List<int>();
            request.Add(routineId);
            var serializedRequest = JsonConvert.SerializeObject(request);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{UserId()}/session/{sessionId}/DeleteConcreteExercisesFromRoutine",
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


        #region Programs


        public async Task<WTServiceResponse<List<WorkoutProgramVM>>> GetProgramsForUser(UserVM user)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.GetAsync($"/api/Programs/user/{user.ID}");

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var programs = JsonConvert.DeserializeObject<List<WorkoutProgramVM>>(content);

                return new WTServiceResponse<List<WorkoutProgramVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = programs
                };
            });
        }

        public async Task<WTServiceResponse<WorkoutProgramVM>> AddProgram(WorkoutProgramVM program)
        {
            // serialize it
            var serializedProgram = JsonConvert.SerializeObject(program);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Programs/user/{UserId()}",
                                                new StringContent(serializedProgram, System.Text.Encoding.Unicode, "application/json"));

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var programVM = JsonConvert.DeserializeObject<WorkoutProgramVM>(content);

                return new WTServiceResponse<WorkoutProgramVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = programVM
                };
            });
        }

        public async Task<WTServiceResponse<string>> SaveProgram(WorkoutProgramVM program)
        {
            // serialize it
            var serializedProgram = JsonConvert.SerializeObject(program);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PutAsync($"/api/Programs/user/{UserId()}/program/{program.ID}",
                                                new StringContent(serializedProgram, System.Text.Encoding.Unicode, "application/json"));

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> DeleteProgram(int id)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.DeleteAsync($"/api/Programs/user/{UserId()}/program/{id}");

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> SaveRoutinesForProgram(UpdateRoutinesForProgramModel routinesModel)
        {
            // serialize it
            var serializedRoutines = JsonConvert.SerializeObject(routinesModel.Routines);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Programs/user/{UserId()}/program/{routinesModel.ID}/routines",
                                                new StringContent(serializedRoutines, System.Text.Encoding.Unicode, "application/json"));

            return HandleApiResponse(response, () =>
            {
                return new WTServiceResponse<string>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = "",
                };
            });
        }

        public async Task<WTServiceResponse<string>> UpdateDatesForProgramRoutines(WorkoutRoutineVM routineModel)
        {
            // serialize it
            var serializedRoutine = JsonConvert.SerializeObject(routineModel);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Programs/user/{UserId()}/program/{routineModel.ProgramId}/updateDatesForRoutine",
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

        public async Task<WTServiceResponse<string>> ActivateOrDeactivateProgram(WorkoutProgramVM programModel)
        {
            // serialize it
            var serializedProgram = JsonConvert.SerializeObject(programModel);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var path = "activate";

            if (programModel.ToBeActivated == false)
                path = "deactivate";

            var response = await httpClient.PostAsync($"/api/Programs/user/{UserId()}/program/{programModel.ID}/{path}",
                                   new StringContent(serializedProgram, System.Text.Encoding.Unicode, "application/json"));

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


        #region Timeline

        public async Task<WTServiceResponse<List<WorkoutSessionVM>>> GetSessionsForMonth(UserVM user, int year, int month)
        {
            var startDate = new DateTime(year - 2, month, 1);
            var endDate = new DateTime(year + 2, month, 1);

            var sessionRequest = new WorkoutSessionRequest { User = user, StartDate = startDate, EndDate = endDate };

            var serializedRequest = JsonConvert.SerializeObject(sessionRequest);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/Sessions/user/{sessionRequest.User.ID}/Sessions",
                                                new StringContent(serializedRequest, System.Text.Encoding.Unicode, "application/json"));


            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var sessions = JsonConvert.DeserializeObject<List<WorkoutSessionVM>>(content);

                return new WTServiceResponse<List<WorkoutSessionVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = sessions
                };
            });
        }

        #endregion

        #region BodyStatistic

        public async Task<WTServiceResponse<List<BodyStatisticVM>>> GetBodyStatistucForMonth(UserVM user, int month, int year)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.GetAsync($"/api/BodyStatistics/user/{user.ID}/BodyStat/ForMonth/{month}/year/{year}");


            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var bodyStatsForMonth = JsonConvert.DeserializeObject<List<BodyStatisticVM>>(content);

                return new WTServiceResponse<List<BodyStatisticVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = bodyStatsForMonth
                };
            });
        }

        public async Task<WTServiceResponse<List<BodyAttributeTemplateVM>>> GetAttributeTemplates(UserVM userVM)
        {
            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.GetAsync($"/api/BodyStatistics/user/{userVM.ID}/AttributeTemplates");

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var attributeTemplates = JsonConvert.DeserializeObject<List<BodyAttributeTemplateVM>>(content);

                return new WTServiceResponse<List<BodyAttributeTemplateVM>>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = attributeTemplates
                };
            });
        }

        public async Task<WTServiceResponse<BodyStatisticVM>> AddStatistic(BodyStatisticVM statistic)
        {
            // serialize it
            var serializedStatistic = JsonConvert.SerializeObject(statistic);

            var httpClient = await _workoutTrackerHttpClient.GetClient();

            var response = await httpClient.PostAsync($"/api/BodyStatistics/user/{UserId()}/BodyStat",
                                                new StringContent(serializedStatistic, System.Text.Encoding.Unicode, "application/json"));

            return await HandleApiResponse(response, async () =>
            {
                var content = await response.Content.ReadAsStringAsync();
                var statisticVM = JsonConvert.DeserializeObject<BodyStatisticVM>(content);

                return new WTServiceResponse<BodyStatisticVM>
                {
                    StatusCode = response.StatusCode,
                    ViewModel = statisticVM,
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

        private int UserId()
        {
            return WorkotTrackerHelper.GetUserId(_httpContextAccessor.HttpContext.User);
        }
    }
}
