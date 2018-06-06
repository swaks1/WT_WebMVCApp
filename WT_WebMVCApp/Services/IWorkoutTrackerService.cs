using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Services
{
    public interface IWorkoutTrackerService
    {
        Task<WTServiceResponse<List<ExerciseVM>>> GetExercisesForUser(UserVM user);
        Task<WTServiceResponse<ExerciseVM>> SaveExerciseForUser(ExerciseVM exercise);
    }
}
