using BatchjobService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public interface IAlgorithmDataLogic
    {
        Task<ModelAlgorithm> GetDataAsync();
        Task<bool> CreateSuggestionAsync(int UserReciever, int UserSuggest);
        void UpdateSuggestion();
        Task<List<AlgorithmDataBeforeModel>> AlgorithmDataBefore();
        List<TaskInterModel> GetTaskTwoMonth(List<TaskInterModel> ListTags);
        void UpdateTimeInteraction(List<AlgorithmDataBeforeModel> ListTags);
    }
}
