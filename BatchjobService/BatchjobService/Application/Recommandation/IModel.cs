using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public interface IModel
    {
        Task<List<Personalization>> getListTagsAsync(int idUser);
        Task<List<Users>> GetListUserInteraction(int IdTag);
        Task<List<Tags>> GetColumns();
        Task<List<Users>> GetRows();
        Task<bool> ApplySuggestionAsync(Personalization personalization);
        Task<TimeInteraction> GetinteractionTime(int UserId, int TagId);


    }
}
