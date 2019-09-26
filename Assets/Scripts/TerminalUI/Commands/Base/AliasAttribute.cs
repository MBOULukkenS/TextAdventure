using System;

namespace TerminalUI.Commands.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AliasAttribute : Attribute
    {
        public string[] Aliases { get; set; }
        
        public AliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}