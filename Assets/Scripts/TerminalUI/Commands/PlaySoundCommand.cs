using System.Collections;
using Managers;
using TerminalUI.Commands.Base;

namespace TerminalUI.Commands
{
    /// <summary>
    /// Een command dat geluiden afspeelt.
    /// </summary>
    [Alias("Sound", "SFX")]
    public class PlaySoundCommand : Command
    {
        public override IEnumerator Run(params string[] args)
        {
            foreach (string arg in args)
                SoundSystem.Instance.PlaySound(arg);
            
            yield break;
        }
    }
}