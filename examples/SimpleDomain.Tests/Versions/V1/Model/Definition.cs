﻿// ReSharper disable All

namespace SimpleDomain.Tests
{
    using System;
    using Modulos.Testing;

    public partial class V1
    {
        [ModelDefinition]
        public class Definition
        {
            [IncludeModel] public static Type[] Include =
            {
                typeof(Departments),
                typeof(Administrators),
                typeof(RegularUsers)
            };
        }
    }
}