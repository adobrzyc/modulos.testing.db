// ReSharper disable All

namespace SimpleDomain.Tests
{
    using System;
    using Db.Model;
    using Modulos.Testing;

    public partial class V2
    {
        [ModelDefinition]
        public class Organizations
        {
            [InlineData]
            public static Organization SuperCompany => new Organization
            {
                OrganizationId = new Guid("CB4E6AAB-6BAC-48E0-9308-4D45501C2E06"),
                Name = "Super company"
            };
        }
    }
}