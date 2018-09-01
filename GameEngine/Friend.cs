using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGameEngine
{
    public class Friend : Character
    {
        private string desc;

        public Friend(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, string name, string desc) :
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
