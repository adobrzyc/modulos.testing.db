using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Modulos.Testing;
using Modulos.Testing.EF;
using SimpleDomain.Db;
using SimpleDomain.Logic;
using Xunit;

namespace SimpleDomain.Tests.SqlServer
{
    [Collection(nameof(V1) + nameof(SqlServer))]
    public class AddUserFunctionalityTests
    {
        private readonly SqlServerEnv<V1.Definition> env;

        public AddUserFunctionalityTests(SqlServerEnv<V1.Definition> env)
        {
            this.env = env;
        }

        [Fact(Skip = "Only for manual execution.")]
        public async Task add_non_existing_user()
        {
            await using var test = await env.CreateTest<Test>();
            
            var functionality = test.Resolve<AddUserFunctionality>();
          
            var user = await functionality.CreateUser("John", "Good", V1.Departments.Sales.DepartmentId);
            user.Should().NotBeNull();
            user.Name.Should().Be("John");
            user.SName.Should().Be("Good");
            user.UserId.Should().NotBe(Guid.Empty);
            user.DepartmentId.Should().Be(V1.Departments.Sales.DepartmentId);
            user.Department.Should().BeNull();

            await test.AssertDb<Context>(async db =>
            {
                var toCheck = await db.Users.SingleOrDefaultAsync(e => e.UserId == user.UserId);

                toCheck.Should().NotBeNull();
                toCheck.Should().NotBeNull();
                toCheck.Name.Should().Be("John");
                toCheck.SName.Should().Be("Good");
                toCheck.UserId.Should().NotBe(Guid.Empty);
                toCheck.DepartmentId.Should().Be(V1.Departments.Sales.DepartmentId);
                toCheck.Should().NotBeNull();
            });
        }

        [Fact(Skip = "Only for manual execution.")]
        public async Task FORBIDDEN__add_user_with_null_name()
        {
            await using var test = await env.CreateTest<Test>();
            try
            {
                var functionality = test.Resolve<AddUserFunctionality>();
                await functionality.CreateUser(null, "Good", V1.Departments.Sales.DepartmentId);
            }
            catch (ArgumentException ex)
            {
                ex.Should().NotBeNull();
                ex.ParamName.Should().Be("name");
                return;
            }

            throw new Exception($"Missing suspected exception: {nameof(ArgumentException)}");
        }

        [Fact(Skip = "Only for manual execution.")]
        public async Task FORBIDDEN__add_user_with_null_sName()
        {
            await using var test = await env.CreateTest<Test>();
            try
            {
                var functionality = test.Resolve<AddUserFunctionality>();
                await functionality.CreateUser("John", null, V1.Departments.Sales.DepartmentId);
            }
            catch (ArgumentException ex)
            {
                ex.Should().NotBeNull();
                ex.ParamName.Should().Be("sName");
                return;
            }

            throw new Exception($"Missing suspected exception: {nameof(ArgumentException)}");
        }

        [Fact(Skip = "Only for manual execution.")]
        public async Task FORBIDDEN__add_existing_user()
        {
            await using var test = await env.CreateTest<Test>();

            var user = V1.Administrators.Admin;

            try
            {
                var functionality = test.Resolve<AddUserFunctionality>();
                await functionality.CreateUser(user.Name, user.SName, V1.Departments.Sales.DepartmentId, user.UserId);
            }
            catch (DbUpdateException)
            {
                return;
            }

            throw new Exception($"Missing suspected exception: {nameof(DbUpdateException)}");
        }
    }
}
