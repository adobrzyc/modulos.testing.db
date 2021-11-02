namespace SimpleDomain.Db.Model
{
    using System;
    using System.Collections.Generic;

    public class Organization
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}