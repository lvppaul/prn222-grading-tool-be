using Microsoft.EntityFrameworkCore.Storage;
using PRN222_TrackingTool.DataAccessLayer.Context;
using PRN222_TrackingTool.DataAccessLayer.Interfaces;
using PRN222_TrackingTool.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn222TrackingToolContext _context;
        private IDbContextTransaction _transaction;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public UnitOfWork(Prn222TrackingToolContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ??= new RoleRepository(_context); }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Async Dispose
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            await _context.DisposeAsync();
        }
    }
}
