using System;

namespace AdventureGameEngine
{
    public interface ISoundPlayerService
    {
        void PlaySoundEffect(string id);
        void PlayBackgroundMusic();
        void StopBackgroundMusic();
    }
}
