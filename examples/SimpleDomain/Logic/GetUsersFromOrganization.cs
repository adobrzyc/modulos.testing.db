namespace SimpleDomain.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Db;
    using Db.Model;
    using Microsoft.EntityFrameworkCore;

    public class GetUsersFromOrganization
    {
        private readonly Context db;

        public GetUsersFromOrganization(Context db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<User>> Execute(Guid organizationId)
        {
            var users = await db.Departments
                .Where(e => e.OrganizationId == organizationId)
                .SelectMany(e => e.Users)
                .Include(e => e.Department)
                .Include(e => e.Department.Organization)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return users;
        }
    }
}