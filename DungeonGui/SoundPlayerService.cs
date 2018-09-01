using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventureGameEngine;

namespace AdventureGameGui
{
    public sealed class SoundPlayerService : ISoundPlayerService
    {
        private System.Media.SoundPlayer soundEffectPlayer;
        private System.Media.SoundPlayer backgroundMusicPlayer;

        public SoundPlayerService()
        {
            this.soundEffectPlayer = new System.Media.SoundPlayer();
            this.backgroundMusicPlayer = new System.Media.SoundPlayer();
        }

        public void PlaySoundEffect(string id)
        {
            var strmName = string.Format("AdventureGameGui.Sounds.{0}.wav", id);
            var stream = typeof(AdventureGameGui.SoundPlayerService).Assembly.GetManifestResourceStream(strmName);
            this.soundEffectPlayer.Stream = stream;
            this.soundEffectPlayer.Load();
            this.soundEffectPlayer.Play();
        }

        public void PlayBackgroundMusic()
        {
            var strmName = string.Format("AdventureGameGui.Sounds.{0}.wav", "BackgroundMusic");
            var stream = typeof(AdventureGameGui.SoundPlayerService).Assembly.GetManifestResourceStream(strmName);
            this.backgroundMusicPlayer.Stream = stream;
            this.backgroundMusicPlayer.Load();
            this.backgroundMusicPlayer.PlayLooping();
        }

        public void StopBackgroundMusic()
        {

        }
    }
}
