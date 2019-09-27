using System;

namespace TerminalUI.Commands.Base
{
    /// <summary>
    /// Dit attribuut kan worden gebruikt om aliases te definiëren voor commando's.
    /// </summary>
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