namespace SimpleDomain.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Db;
    using Db.Model;
    using Microsoft.EntityFrameworkCore;

    public class GetUsersOfDepartment
    {
        private readonly Context db;

        public GetUsersOfDepartment(Context db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<User>> Execute(Guid departmentId)
        {
            var users = await db.Departments
                .Where(e => e.DepartmentId == departmentId)
                .SelectMany(e => e.Users)
                .Include(e => e.Department)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return users;
        }
    }
}