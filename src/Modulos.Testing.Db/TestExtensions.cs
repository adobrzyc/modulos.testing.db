using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Testing
{
    public static class TestExtensions
    {
        public static Task<TContext> SeedDb<TContext>(this ITest test, Func<ISeedProvider, Task> setup = null)
            where TContext : class
        {
            return SeedDb<TContext>(test, async (provider, context) =>
            {
                if(setup != null)
                    await setup(provider);
            });
        }

        public static async Task<TContext> SeedDb<TContext>(this ITest test, Func<ISeedProvider, TContext, Task> setup = null)
            where TContext : class
        {
            var seedProvider = test.GetRequiredService<ISeedProvider>();
            var context = test.GetRequiredService<TContext>();

            if (setup != null)
                await setup(seedProvider, context);

            await seedProvider.Seed();

            return context;
        }
    }
}