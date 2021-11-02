// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Testing
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InlineDataAttribute : Attribute
    {
        public InlineDataAttribute(OperationKind operationKind = OperationKind.Default)
        {
            OperationKind = operationKind;
        }

        public OperationKind OperationKind { get; }
    }
}