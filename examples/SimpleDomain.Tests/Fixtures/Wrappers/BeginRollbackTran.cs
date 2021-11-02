// ReSharper disable ClassNeverInstantiated.Global

namespace SimpleDomain.Tests.Wrappers
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Modulos.Testing;

    public class BeginRollbackTran<TContext> : ITestWrapper
        where TContext : DbContext
    {
        private readonly TContext db;
        private IDbContextTransaction transaction;

        public BeginRollbackTran(TContext db)
        {
            this.db = db;
        }

        public async Task Begin()
        {
            if (db.Database.CurrentTransaction == null)
                transaction = await db.Database.BeginTransactionAsync();
        }

        public async Task Finish()
        {
            if (transaction != null)
            {
                await transaction?.RollbackAsync();
                await transaction.DisposeAsync();
            }
        }
    }
}