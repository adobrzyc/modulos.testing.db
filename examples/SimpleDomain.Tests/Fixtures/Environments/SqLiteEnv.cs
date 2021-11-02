// ReSharper disable ClassNeverInstantiated.Global

namespace SimpleDomain.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Blocks;
    using Db;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Modules;
    using Modulos.Testing;
    using Modulos.Testing.EF;
    using Wrappers;
    using Xunit;

    public class SqLiteEnv<TModel> : TestEnvironment, IAsyncLifetime
        where TModel : class
    {
        public SqLiteEnv()
        {
            Add<InitializeIoc>(block =>
            {
                block.Autofac.RegisterModule<RegisterDependencies>();
                block.AddScoped<ISeedProvider, SeedProviderForEf<Context>>();
                block.AddDbContext<Context>(options => { options.UseSqlite($"Data Source={GetDbName()}.db"); });
            });

            Add<DropAndCreateDb<Context, TModel>>((block, evn) => { block.DropDbAtTheEnd = false; });

            Wrap<BeginRollbackTran<Context>>();
        }

        public async Task InitializeAsync()
        {
            await Build();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await DisposeAsync();
        }

        private static string GetDbName()
        {
            return "SimpleDomainDb." + string.Join('-',
                typeof(TModel).FullName?.Split('.').Last().Split('+').First()
                ?? Guid.NewGuid().ToString());
        }
    }
}