// ReSharper disable All

namespace SimpleDomain.Tests
{
    using System;
    using Modulos.Testing;

    public partial class V2
    {
        [ModelDefinition]
        public class Definition
        {
            [IncludeModel] public static Type[] Include =
            {
                typeof(Organizations),
                typeof(V1.Definition),
                typeof(RegularUsers),
                typeof(Departments)
            };
        }
    }
}