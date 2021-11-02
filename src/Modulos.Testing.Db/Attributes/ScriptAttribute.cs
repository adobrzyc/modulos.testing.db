namespace Modulos.Testing
{
    using System;

    public abstract class ScriptAttribute : Attribute
    {
        protected ScriptAttribute(string splitter)
        {
            Splitter = splitter;
        }

        public string Splitter { get; }
    }
}