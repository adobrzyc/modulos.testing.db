namespace Modulos.Testing
{
    using System;

    public sealed class ModelDefinition : IEquatable<ModelDefinition>
    {
        public ModelDefinition(Type classType)
        {
            ClassType = classType ?? throw new ArgumentNullException(nameof(classType));
        }

        public Type ClassType { get; }

        public bool Equals(ModelDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ClassType == other.ClassType;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ModelDefinition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ClassType.GetHashCode();
        }

        public static bool operator ==(ModelDefinition left, ModelDefinition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ModelDefinition left, ModelDefinition right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(ClassType)}: {ClassType}";
        }
    }
}