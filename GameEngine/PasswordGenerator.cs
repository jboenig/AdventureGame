using System;
using System.Collections.Generic;

namespace AdventureGameEngine
{
    /// <summary>
    /// Instances of this class can be used to generate passwords
    /// with hints.
    /// </summary>
    public sealed class PasswordGenerator
    {
        private List<Password> passwords;
        private Random randomizer;

        public PasswordGenerator()
        {
            this.passwords = new List<Password>();
            this.randomizer = new Random(DateTime.Now.Second);
            this.InitPasswords();
        }

        public Password GetPassword()
        {
            var idx = this.randomizer.Next(0, this.passwords.Count);
            return this.passwords[idx];
        }

        private void InitPasswords()
        {
            var pwd1 = new Password("DaffyDuck");
            pwd1.AddHint("Looney Tunes Cartoon Character");
            pwd1.AddHint("Despicable");
            pwd1.AddHint("Black and yellow");
            pwd1.AddHint("Starts with D");
            pwd1.AddHint("Kind of wacky");
            pwd1.AddHint("Adversary of Bugs");
            this.passwords.Add(pwd1);

            var pwd3 = new Password("Cosmos");
            pwd3.AddHint("Position 1 is a C");
            pwd3.AddHint("The final frontier");
            pwd3.AddHint("Position 2 is an o");
            pwd3.AddHint("Position 3 is an s");
            pwd3.AddHint("Position 4 is a m");
            pwd3.AddHint("Infinite space");
            pwd3.AddHint("Position 5 is a o");
            pwd3.AddHint("Position 6 is a s");
            this.passwords.Add(pwd3);

            var pwd4 = new Password("Green");
            pwd4.AddHint("Position 1 is a G");
            pwd4.AddHint("Color of grass");
            pwd4.AddHint("Position 2 is a r");
            pwd4.AddHint("Position 3 is a e");
            pwd4.AddHint("Position 4 is a e");
            pwd4.AddHint("Position 5 is a n");
            this.passwords.Add(pwd4);

            var pwd6 = new Password("Aloha");
            pwd6.AddHint("Position 1 is a A");
            pwd6.AddHint("Hawaiian goodbye");
            pwd6.AddHint("Position 2 is a l");
            pwd6.AddHint("Position 3 is a o");
            pwd6.AddHint("Position 4 is a h");
            pwd6.AddHint("Position 5 is a a");
            this.passwords.Add(pwd6);
        }
    }

    public class Password
    {
        private string password;
        private List<string> hints;

        public Password(string password)
        {
            this.hints = new List<string>();
            this.password = password;
        }

        public void AddHint(string hint)
        {
            this.hints.Add(hint);
        }

        public IEnumerable<string> Hints
        {
            get { return this.hints; }
        }

        public bool IsMatch(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return (this.password.ToLower().CompareTo(value.ToLower()) == 0);
         }
    }
}
