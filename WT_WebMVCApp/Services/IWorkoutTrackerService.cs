using System;
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

    }
}
