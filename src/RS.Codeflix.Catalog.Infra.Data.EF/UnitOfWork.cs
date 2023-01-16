﻿using RS.CodeFlix.Catalog.Application.Interfaces;

namespace RS.CodeFlix.Catalog.Infra.Data.EF
{
    public class UnitOfWork
        : IUnitOfWork
    {
        private readonly CodeFlixCatalogDbContext _context;

        public UnitOfWork(CodeFlixCatalogDbContext context)
        {
            _context = context;
        }

        public Task Commit(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
