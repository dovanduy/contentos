using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.SaveContent
{
    public class SaveContentHandler : IRequestHandler<SaveContentCommand, ContentsViewModel>
    {
        private readonly ContentoDbContext _context;

        public SaveContentHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<ContentsViewModel> Handle(SaveContentCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var upContent = _context.Contents.AsNoTracking().FirstOrDefault(x=>x.Id == request.Id);
                upContent.TheContent = request.Content;
                upContent.Name = request.Name;
                upContent.ModifiedDate = DateTime.UtcNow;
                _context.Attach(upContent);
                _context.Entry(upContent).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                var resultReturn = new ContentsViewModel
                {
                     Name = upContent.Name,
                     Content = upContent.TheContent
                };
                transaction.Commit();
                return resultReturn;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}
