using ContentProccessService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.SubmitContent
{
    public class SubmitContentHandler : IRequestHandler<SubmitContentCommand>
    {
        private readonly ContentoDbContext _context;

        public SubmitContentHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(SubmitContentCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var upContent = _context.Contents.AsNoTracking().FirstOrDefault(x => x.Id == request.IdContent);
                upContent.TheContent = request.Content;
                upContent.Name = request.Name;
                upContent.ModifiedDate = DateTime.UtcNow;
                _context.Attach(upContent);
                _context.Entry(upContent).State = EntityState.Modified;
                var upTask = _context.Tasks.AsNoTracking().FirstOrDefault(x => x.Id == request.IdTask);
                upTask.Status = 3;
                upTask.ModifiedDate = DateTime.UtcNow;
                _context.Attach(upTask);
                _context.Entry(upTask).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                transaction.Commit();
                return Unit.Value;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return Unit.Value;
            }
        }
    }
}
