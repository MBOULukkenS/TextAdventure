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
            if (args.Length < 1)
                yield break;
            
            int volume = 100;
            if (args.Length > 1)
                volume = int.TryParse(args[1], out int result) ? result : volume;
                
            SoundSystem.Instance.PlaySound(args[0], volume);
            
            yield break;
        }
    }
}