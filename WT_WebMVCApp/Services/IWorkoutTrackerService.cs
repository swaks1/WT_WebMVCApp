﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebMVCApp.Models;
using WT_WebMVCApp.Models.WorkoutAssets;

namespace WT_WebMVCApp.Services
{
    public interface IWorkoutTrackerService
    {
        #region Exercises

        Task<WTServiceResponse<List<ExerciseVM>>> GetExercisesForUser(UserVM user);
        Task<WTServiceResponse<ExerciseVM>> AddExercise(ExerciseVM exercise);
        Task<WTServiceResponse<string>> SaveExercise(ExerciseVM exercise);
        Task<WTServiceResponse<string>> DeleteExercise(int iD);

        #endregion

        #region Routines

        Task<WTServiceResponse<List<WorkoutRoutineVM>>> GetRoutinesForUser(UserVM userVM);
        Task<WTServiceResponse<WorkoutRoutineVM>> AddRoutine(WorkoutRoutineVM routine);
        Task<WTServiceResponse<string>> SaveRoutine(WorkoutRoutineVM routine);
        Task<WTServiceResponse<string>> DeleteRoutine(int iD);
        Task<WTServiceResponse<string>> SaveExercisesForRoutine(UpdateExerciseForRoutineModel exercisesModel);

        #endregion

        #region Sessions

        Task<WTServiceResponse<WorkoutSessionVM>> GetSessionForDay(WorkoutSessionRequest workoutSessionRequest);
        Task<WTServiceResponse<string>> UpdateConcreteExerciseAttributes(ConcreteExerciseVM exercise);
        Task<WTServiceResponse<string>> DeleteConcreteExercise(int exerciseId, int sessionId);
        Task<WTServiceResponse<string>> SaveExercisesForSession(WorkoutSessionRequest workoutSessionRequest);
        Task<WTServiceResponse<string>> SaveRoutinesForSession(WorkoutSessionRequest workoutSessionRequest);
        Task<WTServiceResponse<string>> DeleteConcreteExercisesFromRoutine(int routineId, int sessionId);

        #endregion

        #region Programs

        Task<WTServiceResponse<List<WorkoutProgramVM>>> GetProgramsForUser(UserVM userVM);
        Task<WTServiceResponse<WorkoutProgramVM>> AddProgram(WorkoutProgramVM program);
        Task<WTServiceResponse<string>> SaveProgram(WorkoutProgramVM program);
        Task<WTServiceResponse<string>> DeleteProgram(int iD);
        Task<WTServiceResponse<string>> SaveRoutinesForProgram(UpdateRoutinesForProgramModel routinesModel);
        Task<WTServiceResponse<string>> UpdateDatesForProgramRoutines(WorkoutRoutineVM routineModel);
        Task<WTServiceResponse<string>> ActivateOrDeactivateProgram(WorkoutProgramVM programModel);

        #endregion

        #region Timeline

        Task<WTServiceResponse<List<WorkoutSessionVM>>> GetSessionsForMonth(UserVM user, int year, int month);

        #endregion

        #region BodyStatistic

        Task<WTServiceResponse<List<BodyStatisticVM>>> GetBodyStatistucForMonth(UserVM user, int month, int year);
        Task<WTServiceResponse<List<BodyAttributeTemplateVM>>> GetAttributeTemplates(UserVM userVM);
        Task<WTServiceResponse<BodyStatisticVM>> AddStatistic(BodyStatisticVM statistic);

        #endregion

    }
}
