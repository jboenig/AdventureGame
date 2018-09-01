using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGameEngine
{
    public class Portal : RoomFeature
    {
        private IUserPromptService userPrompt;
        private string desc;
        private Password pwd;
        private int failedAttempts;
        private int maxAttempts;
        private GameBoard.Position destination;

        public Portal(IUserPromptService userPrompt,
            string name,
            string description,
            GameBoard.Position destination,
            Password pwd,
            int maxAttempts)
        {
            this.userPrompt = userPrompt;
            this.Name = name;
            this.desc = description;
            this.destination = destination;
            this.pwd = pwd;
            this.maxAttempts = maxAttempts;
        }

        public override string Description
        {
            get
            {
                return this.desc;
            }
        }

        /// <summary>
        /// Prompts the user for a password if necessary and attempts
        /// to enter the portal.
        /// </summary>
        /// <returns></returns>
        public BoolMessageResult TryEnter(IMovePlayer movePlayer)
        {
            string pwdText = string.Empty;

            if (this.pwd != null)
            {
                pwdText = this.userPrompt.PromptText("Enter Portal", "What's the password? ");
            }

            return this.TryEnter(movePlayer, pwdText);
        }

        public BoolMessageResult TryEnter(IMovePlayer movePlayer, string pwd)
        {
            if (this.pwd != null)
            {
                if (this.failedAttempts >= this.maxAttempts)
                {
                    var msg = string.Format("This portal has closed because you have tried {0} incorrect passwords to access it. Tough luck!", this.failedAttempts);
                    return new BoolMessageResult(false, msg);
                }

                if (string.IsNullOrEmpty(pwd))
                {
                    return new BoolMessageResult(false, "This portal requires a password");
                }
                else if (!this.pwd.IsMatch(pwd))
                {
                    this.failedAttempts++;
                    return new BoolMessageResult(false, "That is not the correct password for this portal");
                }
            }

            return movePlayer.MoveTo(this.destination);
        }
    }
}
