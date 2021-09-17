using System;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Testing
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ModelDefinitionAttribute : Attribute
    {
        public ModelDefinitionAttribute(bool isRootDefinition = false)
        {
            IsRootDefinition = isRootDefinition;
        }

        /// <summary>
        /// Defines if the model is a root model. Root models don't explore their properties and nested types
        /// except definitions marked with <see cref="IncludeModelAttribute"/>.  
        /// </summary>
        public bool IsRootDefinition { get; }
    }
}