using PRN222_TrackingTool.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        Task<int> SaveAsync();
    }
}
