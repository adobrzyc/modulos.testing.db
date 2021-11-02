// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration

namespace SimpleDomain.Tests.SqlServer
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Logic;
    using Modulos.Testing;
    using Xunit;

    [Collection(nameof(V2) + nameof(SqlServer))]
    public class GetUsersFromOrganization_tests
    {
        private readonly SqlServerEnv<V2.Definition> _env;

        public GetUsersFromOrganization_tests(SqlServerEnv<V2.Definition> env)
        {
            _env = env;
        }

        [Fact(Skip = "Only for manual execution.")]
        public async Task obtain_users_from_SuperCompany_organization()
        {
            await using var test = await _env.CreateTest<Test>();

            var functionality = test.Resolve<GetUsersFromOrganization>();

            var users = await functionality.Execute(V2.Organizations.SuperCompany.OrganizationId)
                .ConfigureAwait(false);

            users.Should().HaveCount(2);
            users.All(e => e.Department != null).Should().BeTrue();
            users.All(e => e.Department.Organization != null).Should().BeTrue();
            users.All(e => e.DepartmentId == V2.Departments.IT.DepartmentId).Should().BeTrue();
        }
    }
}