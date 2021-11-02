// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SimpleDomain.Tests
{
    using Db.Model;
    using Modulos.Testing;

    public partial class V2
    {
        [ModelDefinition]
        public class Departments
        {
            [InlineData(OperationKind.Update)]
            public static Department IT
            {
                get
                {
                    var obj = V1.Departments.IT;
                    obj.OrganizationId = Organizations.SuperCompany.OrganizationId;
                    return obj;
                }
            }
        }
    }
}