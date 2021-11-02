// ReSharper disable MemberCanBePrivate.Global

namespace Modulos.Testing
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ScriptFileAttribute : ScriptAttribute
    {
        public ScriptFileAttribute()
            : this("")
        {
        }

        public ScriptFileAttribute(string splitter) : base(splitter)
        {
        }
    }
}