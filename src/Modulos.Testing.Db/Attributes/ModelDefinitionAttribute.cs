using System;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Testing
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ModelDefinitionAttribute : Attribute
    {
    }
}