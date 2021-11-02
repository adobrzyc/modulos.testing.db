// ReSharper disable MemberCanBePrivate.Global

namespace Modulos.Testing
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ScriptsDirectoryAttribute : ScriptAttribute
    {
        public ScriptsDirectoryAttribute()
            : this("")
        {
        }

        public ScriptsDirectoryAttribute(string splitter) : base(splitter)
        {
        }
    }
}