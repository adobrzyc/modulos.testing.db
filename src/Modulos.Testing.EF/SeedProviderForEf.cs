// ReSharper disable StaticMemberInGenericType
// ReSharper disable UnusedType.Global

namespace Modulos.Testing.EF
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class SeedProviderForEf<TContext> : SeedProviderBase
        where TContext : DbContext
    {
        private static ConcurrentBag<string> _log = new();

        public SeedProviderForEf(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private TContext Context { get; }

        public override async Task DropAndCreateDb()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }

        public override async Task Seed()
        {
            var dbsets = Context.GetDbSetsFromContext();

            foreach (var model in Model)
            {
                await HandleFieldsAsScripts(model.ClassType);

                await HandlePropsAsEntities(model.ClassType, dbsets);
            }
        }

        public override object GetDb()
        {
            return Context;
        }

        private async Task HandlePropsAsEntities(Type classType, IReadOnlyDictionary<Type, dynamic> dbsets)
        {
            var entityWithOperations = GetEntitiesWithOperationFromClass(classType);

            foreach (var entityWithOperation in entityWithOperations)
            {
                //var entity = cloner.Clone(entityWithOperation.Entity);
                var entity = entityWithOperation.Entity; //cloner.Clone(entityWithOperation.Entity);
                var operation = entityWithOperation.Operation;

                if (!dbsets.TryGetValue(entity.GetType(), out var dbset)) throw new ApplicationException($"Unable to determine DbSet for entity: {entity.GetType().FullName}");

                try
                {
                    switch (operation)
                    {
                        case OperationKind.Insert:
                            dbset.Add((dynamic)entity);
                            break;
                        case OperationKind.Delete:
                            dbset.Remove((dynamic)entity);
                            break;
                        case OperationKind.Update:
                            Context.Entry(entity).State = EntityState.Modified;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e)
                {
                    switch (operation)
                    {
                        case OperationKind.Insert:
                            throw new ApplicationException($"Unable to invoke: {nameof(DbSet<object>.Add)}.", e);
                        case OperationKind.Delete:
                            throw new ApplicationException($"Unable to invoke: {nameof(DbSet<object>.Remove)}.", e);
                        case OperationKind.Update:
                            throw new ApplicationException("Unable to invoke: Context.Entry(entity).State = EntityState.Modified.", e);
                    }
                }
            }

            await Context.SaveChangesAsync();

            foreach (var entityEntry in Context.ChangeTracker.Entries().ToArray())
                entityEntry.State = EntityState.Detached;

            Context.ClearLocals();
        }

        private async Task HandleFieldsAsScripts(Type classType)
        {
            var scripts = classType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Select(e => new
                {
                    InlineAttr = e.GetCustomAttribute<InlineScriptAttribute>(),
                    ScripstAttr = e.GetCustomAttribute<ScriptFileAttribute>(),
                    DirectoriesAttr = e.GetCustomAttribute<ScriptsDirectoryAttribute>(),
                    Field = e
                })
                .Where(e => e.InlineAttr != null || e.ScripstAttr != null || e.DirectoriesAttr != null)
                .ToArray();


            foreach (var script in scripts)
            {
                var value = script.Field.GetValue(null).ToString();

                if (script.InlineAttr != null)
                {
                    _log.Add($"{Environment.NewLine}#Inline: {value}{Environment.NewLine}");
                    var sql = value;
                    await ExecuteSql(sql, script.InlineAttr.Splitter);
                    continue;
                }

                if (script.ScripstAttr != null)
                {
                    _log.Add($"{Environment.NewLine}#File: {value}{Environment.NewLine}");
                    var sql = File.ReadAllText(value);
                    await ExecuteSql(sql, script.ScripstAttr.Splitter);
                    continue;
                }

                if (script.DirectoriesAttr != null)
                {
                    _log.Add($"{Environment.NewLine}#Directory: {value}{Environment.NewLine}");
                    foreach (var sql in Directory.GetFiles(value)
                        .OrderBy(Path.GetFileName)
                        .Select(File.ReadAllText))
                        await ExecuteSql(sql, script.DirectoriesAttr.Splitter);
                }
            }

            _log = new ConcurrentBag<string>();
        }


        private async Task ExecuteSql(string sql, string splitter)
        {
            DbConnection connection = null;
            try
            {
                _log.Add(sql);
                connection = Context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open) await connection.OpenAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(splitter))
                    await ExecuteSqlViaCommand(connection, sql).ConfigureAwait(false);
                else
                    foreach (var sqlToExecute in sql.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries))
                        await ExecuteSqlViaCommand(connection, sqlToExecute).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var msg = "DB: " + (connection?.Database ?? "")
                                 + Environment.NewLine
                                 + "CS: " + (connection?.ConnectionString ?? "")
                                 + Environment.NewLine
                                 + "Exception:"
                                 + Environment.NewLine + e
                                 + "SQL:"
                                 + Environment.NewLine + sql;


                msg += $"{Environment.NewLine}~~~~~~LOGS~~~~~~{Environment.NewLine}" +
                       $"{string.Join($"{Environment.NewLine}--------------------------------------{Environment.NewLine}", _log)}";

                throw new ApplicationException($"Script execution exception. INFO: {Environment.NewLine}{msg}", e);
            }
        }

        private static async Task ExecuteSqlViaCommand(DbConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}