namespace SimpleDomain.Tests.SqLite
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Logic;
    using Modulos.Testing;
    using Xunit;

    [Collection(nameof(V2) + nameof(SqLite))]
    public class GetUsersFromOrganizationTests
    {
        private readonly SqLiteEnv<V2.Definition> env;

        public GetUsersFromOrganizationTests(SqLiteEnv<V2.Definition> env)
        {
            this.env = env;
        }

        [Fact]
        public async Task obtain_users_from_SuperCompany_organization()
        {
            await using var test = await env.CreateTest<Test>();

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