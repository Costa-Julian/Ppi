using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly EfAppDbContext _appDbContext;

        public EfUnitOfWork(EfAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
         
        }

        public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default)
        {
            var strategy = _appDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync( async () => 
            {
                await using var tx = await _appDbContext.Database.BeginTransactionAsync(ct);
                try
                {
                    await action(ct);
                    await _appDbContext.SaveChangesAsync(ct);
                    await tx.CommitAsync(ct);
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default)
        {
            T result = default!;
            var strategy = _appDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => 
            {
                await using var tx = await _appDbContext.Database.BeginTransactionAsync(ct);
                try 
                {
                    result = await action(ct);
                    await _appDbContext.SaveChangesAsync(ct);
                    await tx.CommitAsync(ct); 
                }
                catch 
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });
            return result;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _appDbContext.SaveChangesAsync(ct);
    }
}
