using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Testing
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InlineScriptAttribute : ScriptAttribute
    {
        public InlineScriptAttribute()
            : this("")
        {

        }

        public InlineScriptAttribute(string splitter) : base(splitter)
        {
        }
    }
}