// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Testing
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ModelDefinitionAttribute : Attribute
    {
        public ModelDefinitionAttribute(bool isRootDefinition = false)
        {
            IsRootDefinition = isRootDefinition;
        }

        public bool IsRootDefinition { get; }
    }
}