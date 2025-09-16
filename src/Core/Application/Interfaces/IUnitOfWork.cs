﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task ExecuteInTransactionAsync(Func<CancellationToken,Task> action, CancellationToken ct = default);
        Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken,Task<T>> action, CancellationToken ct = default);
    }
}
