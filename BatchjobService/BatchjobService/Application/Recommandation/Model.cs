
using BatchjobService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
namespace BatchjobService.Application.Recommandation
{
    public class Model : IModel
    {
        private  ContentoDbContext _context;


        public Model(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Personalization>> getListTagsAsync(int idUser)
        {
            var ListTag = new List<Personalization> ();
            var person = await _context.Personalizations.Where(p => p.IdUser == idUser && p.IsChosen == true && p.IsSuggestion == false).ToListAsync();

            //var tags = await _context.Tags
            //    .Include(t => t.Personalizations)
            //    .Where(t => t.Personalizations.Any(p => p.IdUser == person.IdUser))
            //    .Where(t => t.IsActive == true)
            //    .Select(t => new 
            //    {
            //        t,
            //        Personalizations = t.Personalizations.Where(x=>x.IsChosen == false && x.IsSuggestion == false)
            //    }).ToListAsync();

            foreach (var item in person)
            {
                Personalization p = new Personalization
                {

                    TagId = item.IdTag,
                    UserId = idUser
                };
                ListTag.Add(p);

            }

            return ListTag;
        }

          
        public async Task<List<Users>> GetListUserInteraction(int IdTag)
        {
            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Personalizations)
                .Where(u => u.Personalizations.Any(p => p.IdTag == IdTag && p.IsChosen == true))
                .ToListAsync();
            return user;
        }

        public async Task<List<Users>> GetRows()
        {
            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Accounts)
                .Where(u => u.Accounts.Any(a => a.IdRole == 4 && a.IsActive == true)).ToListAsync();               
            return user;
        }

        public async Task<List<Tags>> GetColumns()
        {
            var tags = await _context.Tags.AsNoTracking()
                .Where(t => t.IsActive == true).ToListAsync();
            return tags;
        }

        public async Task<bool> ApplySuggestionAsync(Personalization personalization)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Personalizations
                    .Where(p => p.IdUser == personalization.UserId && p.IdTag == personalization.TagId)
                    .FirstOrDefault().IsSuggestion = true;
                await _context.SaveChangesAsync();
                transaction.Commit();
                //Personalizations newP = new Personalizations
                //{
                //    IdTag = personalization.TagId,
                //    IdUser = personalization.UserId,
                //    CreatedDate = DateTime.UtcNow,
                //    ModifiedDate = DateTime.UtcNow,
                //    TimeInteraction = 0,
                //    IsSuggestion = true
                    
                //};
                //_context.Add(newP);

                //await _context.SaveChangesAsync();
                //transaction.Commit();
                return true;
            }
            catch(Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<TimeInteraction> GetinteractionTime(int UserId, int TagId)
        {
            var person =  await _context.Personalizations.AsNoTracking()
                .Where(p => p.IdTag == TagId)
                .Where(p => p.IdUser == UserId).
                Select(p => new TimeInteraction
                {
                    UserId = p.IdUser,
                    TagId = p.IdTag,
                    time = Double.Parse("" + p.TimeInteraction),
                    IsChosen = p.IsChosen ?? false,
                    IsSuggest = p.IsSuggestion ?? false
                }).FirstOrDefaultAsync();
                return person;
        }


    }


}
