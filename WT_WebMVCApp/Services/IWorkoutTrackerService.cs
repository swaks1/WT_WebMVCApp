﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Services
{
    public interface IWorkoutTrackerService
    {
        #region Exercises

        Task<WTServiceResponse<List<ExerciseVM>>> GetExercisesForUser(UserVM user);
        Task<WTServiceResponse<string>> SaveExercise(ExerciseVM exercise);
        Task<WTServiceResponse<string>> DeleteExercise(int iD);
        Task<WTServiceResponse<ExerciseVM>> AddExercise(ExerciseVM exercise);

        #endregion


        #region Routines

        Task<WTServiceResponse<List<WorkoutRoutineVM>>> GetRoutinesForUser(UserVM userVM);

        #endregion

    }
}
