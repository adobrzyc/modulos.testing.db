namespace SimpleDomain.Db.Model
{
    using System;
    using System.Collections.Generic;

    public class Department
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<User> Users { get; set; }
    }
}