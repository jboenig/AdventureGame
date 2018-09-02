namespace AdventureGameEngine
{
    public class NeutralCharacter : Character
    {
        private string desc;

        public NeutralCharacter(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
            base(consoleOut, soundPlayer)
        {
            this.Name = name;
            this.desc = desc;
        }

        public override string Description
        {
            get { return this.desc; }
        }
    }
}
