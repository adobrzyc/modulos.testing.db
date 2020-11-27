using System;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Testing
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class IncludeModelAttribute : Attribute
    {
    }
}