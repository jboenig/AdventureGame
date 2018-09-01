using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class represents the entire game.
    /// </summary>
    public abstract class Game : IMovePlayer, ICommandProcessor, INotifyPropertyChanged
    {
        #region Member Variables

        private IGameHost gameHost;
        private IConsoleOutputService consoleOut;
        private IUserPromptService userPrompt;
        private ISoundPlayerService soundPlayer;
        private string mapName;
        private GameBoard gameBoard;
        private Player player;
        private List<string> commandHistory;
        private Conversation currentConversation;

        private const int MoveHealthCost = 1;
        private const int DrinkHealthBenefit = 5;

        #endregion

        #region Constructors

        public Game(IGameHost gameHost,
            IConsoleOutputService consoleOut,
            IUserPromptService userPrompt,
            ISoundPlayerService soundPlayer)
        {
            this.gameHost = gameHost;
            this.consoleOut = consoleOut;
            this.userPrompt = userPrompt;
            this.soundPlayer = soundPlayer;

            this.mapName = null;
            this.Reset();
        }

        #endregion

        #region Property Change Notification

        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string propertyName)
        {
            var propChangedHandler = this.PropertyChanged;
            if (propChangedHandler != null)
            {
                var evtArgs = new PropertyChangedEventArgs(propertyName);
                propChangedHandler(this, evtArgs);
            }
        }

        #endregion

        #region Services

        protected IConsoleOutputService ConsoleOut
        {
            get
            {
                return this.consoleOut;
            }
        }

        protected IUserPromptService UserPrompt
        {
            get
            {
                return this.userPrompt;
            }
        }

        protected ISoundPlayerService SoundPlayer
        {
            get
            {
                return this.soundPlayer;
            }
        }

        protected ICommandProcessor CommandProcessor
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Game Board

        /// <summary>
        /// Gets or sets the name for the map used
        /// by the game board.
        /// </summary>
        public string MapName
        {
            get
            {
                return this.mapName;
            }
            set
            {
                this.mapName = value;
            }
        }

        /// <summary>
        /// Creates the game board.
        /// </summary>
        /// <param name="mapName">
        /// Name of map to create.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="AdventureGameEngine.GameBoard"/> object.
        /// </returns>
        public abstract GameBoard CreateGameBoard(string mapName);

        public GameBoard GameBoard
        {
            get { return this.gameBoard; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Player CreatePlayer();

        /// <summary>
        /// Resets the game.
        /// </summary>
        public void Reset()
        {
            this.gameBoard = this.CreateGameBoard(this.mapName);
            this.gameBoard.Initialize();
            this.player = this.CreatePlayer();
            var mapEntry = this.gameBoard.GetEntry(this.gameBoard.StartPosition);
            if (mapEntry == null)
            {
                throw new InvalidOperationException("GameMap start position not defined");
            }
            mapEntry.Enter(this.player);
            this.commandHistory = new List<string>();
            this.currentConversation = Conversation.Start(this.player);
        }

        public Player Player
        {
            get { return this.player; }
        }

        public GameBoard.Position PlayerPosition
        {
            get { return this.player.Position; }
        }

        private Room CurrentRoom
        {
            get
            {
                if (!this.player.Position.IsUndefined)
                {
                    return this.gameBoard.GetEntry(this.player.Position) as Room;
                }
                return null;
            }
        }

        #region Command Processing

        public IList<string> CommandHistory
        {
            get { return this.commandHistory; }
        }

        /// <summary>
        /// This method parses the specified command line and execute
        /// the command.
        /// </summary>
        /// <param name="commandLine">Command line to execute</param>
        public bool ExecuteCommand(string commandLine)
        {
            bool isValidCommand = false;

            commandLine = commandLine.ToLower();
            var tokens = commandLine.Split(new char[] { ' ' });

            if (tokens.Length > 0)
            {
                if (tokens[0] == "quit" || tokens[0] == "leave" || tokens[0] == "exit")
                {
                    isValidCommand = true;
                    var answer = this.UserPrompt.PromptBool("Question", "Are you really a quitter?");
                    if (answer == true)
                    {
                        if (this.gameHost != null)
                        {
                            this.gameHost.Exit();
                        }
                    }
                }
                else if (tokens[0] == "enter")
                {
                    isValidCommand = true;
                    this.Enter();
                }
                else if (tokens[0] == "clear")
                {
                    isValidCommand = true;
                    this.ConsoleOut.Clear();
                }
                else if (tokens[0] == "show" && tokens.Length > 1)
                {
                    if (tokens[1] == "map")
                    {
                        isValidCommand = true;
                        this.gameBoard.Display(this.player.Position);
                    }
                    else if (tokens[1] == "history")
                    {
                        isValidCommand = true;
                        this.DisplayCommandHistory();
                    }
                    else if (tokens[1] == "health")
                    {
                        isValidCommand = true;
                        this.player.DisplayHealth();
                    }
                }
                else if (tokens[0] == "move")
                {
                    if (tokens.Length < 2)
                    {
                        this.ConsoleOut.WriteLine("Tell me which direction (east, west, north, or south)");
                    }
                    else
                    {
                        isValidCommand = this.MovePlayer(tokens[1]);
                    }
                }
                else if (tokens[0] == "forward")
                {
                    isValidCommand = true;
                    this.MovePlayerNorth();
                }
                else if (tokens[0] == "back")
                {
                    isValidCommand = true;
                    this.MovePlayerSouth();
                }
                else if (tokens[0] == "left")
                {
                    isValidCommand = true;
                    this.MovePlayerWest();
                }
                else if (tokens[0] == "right")
                {
                    this.MovePlayerEast();
                    isValidCommand = true;
                }
                else if (tokens[0] == "look")
                {
                    this.DisplayCurrentRoomDescription();
                    isValidCommand = true;
                }
                else if (tokens[0] == "followers")
                {
                    this.DisplayFollowers();
                    isValidCommand = true;
                }
                else if (tokens[0] == "read")
                {
                    string runeName = null;
                    if (tokens.Length > 1)
                    {
                        runeName = tokens[1];
                    }

                    this.ReadRune(runeName);
                    isValidCommand = true;
                }
                else if (tokens[0].StartsWith("inv"))
                {
                    this.DisplayPlayerInventory();
                    isValidCommand = true;
                }
                else if (tokens[0] == "take")
                {
                    isValidCommand = true;
                    if (tokens.Length < 2)
                    {
                        this.ConsoleOut.WriteLine("Tell me what you want to take!");
                    }
                    else if (tokens.Length >= 4 && tokens[2] == "from")
                    {
                        // Fourth token is the name of a character
                        this.TakeItemFromCharacter(tokens[1], tokens[3]);
                    }
                    else
                    {
                        this.TakeItemFromRoom(tokens[1]);
                    }
                }
                else if (tokens[0] == "drop")
                {
                    isValidCommand = true;
                    if (tokens.Length < 2)
                    {
                        this.ConsoleOut.WriteLine("Tell me what you want to drop!");
                    }
                    else
                    {
                        this.DropItem(tokens[1]);
                    }
                }
                else if (tokens[0] == "use")
                {
                    isValidCommand = true;
                    if (tokens.Length < 2)
                    {
                        this.ConsoleOut.WriteLine("Tell me what you want to drop!");
                    }
                    else
                    {
                        this.UseItem(tokens[1]);
                    }
                }
                else if (tokens[0] == "drink")
                {
                    isValidCommand = true;
                    int drinkQty = 5;
                    if (tokens.Length > 1)
                    {
                        try
                        {
                            drinkQty = Convert.ToInt32(tokens[1]);
                        }
                        catch
                        {
                            drinkQty = 5;
                        }
                    }
                    this.Drink(drinkQty);
                }
                else if (tokens[0] == "attack")
                {
                    isValidCommand = true;
                    if (tokens.Length > 1)
                    {
                        this.Attack(tokens[1]);
                    }
                    else
                    {
                        this.Attack();
                    }
                }
                else if (tokens[0] == "talk" && tokens[1] == "to")
                {
                    isValidCommand = true;
                    if (tokens.Length > 2)
                    {
                        this.TalkTo(tokens[2]);
                    }
                    else
                    {
                        this.ConsoleOut.WriteLine("Who would like to talk to?");
                    }
                }
                else if (tokens[0] == "say")
                {
                    isValidCommand = true;
                    if (tokens.Length > 1)
                    {
                        var dialogText = commandLine.Substring(3).Trim();
                        this.Say(dialogText);
                    }
                    else
                    {
                        this.ConsoleOut.WriteLine("Say What?");
                    }
                }
                else if (tokens[0] == "history")
                {
                    isValidCommand = true;
                    this.DisplayCommandHistory();
                }
                else if (tokens[0] == "help")
                {
                    isValidCommand = true;
                    this.DisplayHelp();
                }
            }

            if (isValidCommand)
            {
                // Command line successfully executed. Add it
                // to the command history.
                this.commandHistory.Add(commandLine);
                this.FireCommandExecuted();
            }

            return isValidCommand;
        }

        public event EventHandler CommandExecuted;

        private void FireCommandExecuted()
        {
            if (this.CommandExecuted != null)
            {
                this.CommandExecuted(this, new EventArgs());
            }
        }

        #endregion

        #region Implementation

        private void Enter()
        {
            if (this.player.Position.IsUndefined)
            {
                // Player just now entering maze
                var mapEntry = this.gameBoard.GetEntry(this.gameBoard.StartPosition);
                if (mapEntry == null)
                {
                    throw new InvalidOperationException("GameMap start position not defined");
                }
                mapEntry.Enter(this.player);
                this.DisplayCurrentRoomDescription();
            }
            else
            {
                // Check to see if the room contains a portal
                var portal = this.CurrentRoom.GetFeatures<Portal>().FirstOrDefault();
                if (portal != null)
                {
                    // Portal found. Try to enter.
                    var res = portal.TryEnter(this);
                    if (!res.IsSuccess)
                    {
                        this.ConsoleOut.WriteLine(res.Message);
                    }
                    else
                    {
                        this.SoundPlayer.PlaySoundEffect("PortalEnter");
                        this.ConsoleOut.WriteLine(res.Message);
                    }
                }
                else
                {
                    this.ConsoleOut.WriteLine("You are already in the maze");
                }
            }
        }

        BoolMessageResult IMovePlayer.MoveTo(GameBoard.Position pos)
        {
            if (pos.IsUndefined)
            {
                // Undefined means exit the game
                this.player.Position = GameBoard.Position.Undefined;
            }
            else
            {
                this.player.Position = pos;
                this.OnPlayerPositionChanged();
            }

            return new BoolMessageResult(true, "Zap!");
        }

        private bool MovePlayer(string direction)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(direction))
            {
                if (direction == "north")
                {
                    this.MovePlayerNorth();
                    success = true;
                }
                else if (direction == "south")
                {
                    this.MovePlayerSouth();
                    success = true;
                }
                else if (direction == "east")
                {
                    this.MovePlayerEast();
                    success = true;
                }
                else if (direction == "west")
                {
                    this.MovePlayerWest();
                    success = true;
                }
            }

            return success;
        }

        private void MovePlayerNorth()
        {
            if (this.player.Position.IsUndefined)
            {
                // Player just now entering maze
                this.player.Position = this.gameBoard.StartPosition;
                this.OnPlayerPositionChanged();
            }
            else
            {
                // Update player position if not a wall
                var curMapEntry = this.gameBoard.GetEntry(this.player.Position);
                var newMapEntry = this.gameBoard.GetEntry(this.player.Position.Forward());
                if (curMapEntry.CanExit(this.player) && newMapEntry.CanEnter(this.player))
                {
                    curMapEntry.Exit(this.player);
                    newMapEntry.Enter(this.player);
                    this.player.Position = this.player.Position.Forward();
                    this.OnPlayerPositionChanged();
                }
                else
                {
                    this.ConsoleOut.WriteLine();
                    this.ConsoleOut.WriteLine(@"You can't move there");
                }
            }
        }

        private void MovePlayerSouth()
        {
            if (!this.player.Position.IsUndefined)
            {
                // Update player position if not a wall
                var curMapEntry = this.gameBoard.GetEntry(this.player.Position);
                var newMapEntry = this.gameBoard.GetEntry(this.player.Position.Back());
                if (curMapEntry.CanExit(this.player) && newMapEntry.CanEnter(this.player))
                {
                    curMapEntry.Exit(this.player);
                    newMapEntry.Enter(this.player);
                    this.player.Position = this.player.Position.Back();
                    this.OnPlayerPositionChanged();
                }
                else
                {
                    this.ConsoleOut.WriteLine("You cannot move there");
                }
            }
        }

        private void MovePlayerWest()
        {
            if (!this.player.Position.IsUndefined)
            {
                // Update player position if not a wall
                var curMapEntry = this.gameBoard.GetEntry(this.player.Position);
                var newMapEntry = this.gameBoard.GetEntry(this.player.Position.Left());
                if (curMapEntry.CanExit(this.player) && newMapEntry.CanEnter(this.player))
                {
                    curMapEntry.Exit(this.player);
                    newMapEntry.Enter(this.player);
                    this.player.Position = this.player.Position.Left();
                    this.OnPlayerPositionChanged();
                }
                else
                {
                    this.ConsoleOut.WriteLine("You cannot move there");
                }
            }
        }

        private void MovePlayerEast()
        {
            if (!this.player.Position.IsUndefined)
            {
                // Update player position if not a wall
                var curMapEntry = this.gameBoard.GetEntry(this.player.Position);
                var newMapEntry = this.gameBoard.GetEntry(this.player.Position.Right());
                if (curMapEntry.CanExit(this.player) && newMapEntry.CanEnter(this.player))
                {
                    curMapEntry.Exit(this.player);
                    newMapEntry.Enter(this.player);
                    this.player.Position = this.player.Position.Right();
                    this.OnPlayerPositionChanged();
                }
                else
                {
                    this.ConsoleOut.WriteLine("You cannot move there");
                }
            }
        }

        protected virtual void OnPlayerPositionChanged()
        {
            var mapEntry = this.GameBoard.GetEntry(this.player.Position);
            this.currentConversation = Conversation.Start(this.player);
            this.SoundPlayer.PlaySoundEffect("Walking");
            this.player.DecreaseHealth(MoveHealthCost);

            foreach (var follower in this.player.Followers)
            {
                follower.Position = this.player.Position;
            }

            this.DisplayCurrentRoomDescription();
            this.FirePropertyChanged("PlayerPosition");
        }

        private void TakeItemFromRoom(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                var item = this.CurrentRoom.TakeItemByName(itemName);
                if (item != null)
                {
                    var res = this.player.ReceiveItem(item);
                    if (!res.IsSuccess)
                    {
                        this.ConsoleOut.WriteLine(res.Message);
                    }
                    else
                    {
                        this.CurrentRoom.RemoveItem(item);
                        item.DisplayTakeMessage();
                    }
                }
                else
                {
                    this.ConsoleOut.WriteLine(string.Format("This room does not contain a {0}", itemName));
                }
            }
        }

        private void TakeItemFromCharacter(string itemName, string characterName)
        {
            if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(characterName))
            {
                InventoryItem item = null;

                var character = this.CurrentRoom.GetCharacterByName(characterName);
                if (character == null)
                {
                    this.ConsoleOut.WriteLine(string.Format("{0} is not a character in this room", characterName));
                }
                else
                {
                    if (character is Enemy && !character.IsDead())
                    {
                        this.ConsoleOut.WriteLine(string.Format("You will have to kill {0} to take the {1}", characterName, itemName));
                    }
                    else
                    {
                        var follower = this.player.GetFollowerByName(characterName);
                        if (follower != null)
                        {
                            var followerItemProvider = follower as IItemProvider;
                            if (followerItemProvider != null)
                            {
                                item = followerItemProvider.TakeItemByName(itemName);
                            }
                        }

                        if (item == null)
                        {
                            this.ConsoleOut.WriteLine(string.Format("{0} cannot give you the {1}", characterName, itemName));
                        }
                    }
                }

                if (item != null)
                {
                    var res = this.player.ReceiveItem(item);
                    if (!res.IsSuccess)
                    {
                        this.ConsoleOut.WriteLine(res.Message);
                    }
                    else
                    {
                        item.DisplayTakeMessage();
                    }
                }
            }
        }

        private void DropItem(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                var item = this.player.FindItemByName(itemName);
                if (item != null)
                {
                    var res = this.player.RemoveFromInventory(itemName);
                    if (!res.IsSuccess)
                    {
                        this.ConsoleOut.WriteLine(res.Message);
                    }
                    else
                    {
                        this.CurrentRoom.AddItem(item);
                    }
                }
                else
                {
                    this.ConsoleOut.Write(string.Format("You do not have a {0}", itemName));
                }
            }
        }

        private void UseItem(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                var item = this.player.UseItemByName(itemName);
                if (item == null)
                {
                    this.ConsoleOut.Write(string.Format("You do not have a {0} to use", itemName));
                }
                else
                {
                    this.ConsoleOut.Write(string.Format("You are now holding the {0}", itemName));
                }
            }
        }

        private void Drink(int drinkQty)
        {
            var flask = this.player.UseItemByName("flask") as Flask;
            if (flask == null)
            {
                this.ConsoleOut.WriteLine("You have nothing to drink from");
            }
            else
            {
                if (flask.Drink(drinkQty))
                {
                    this.SoundPlayer.PlaySoundEffect("Drink");
                    this.player.IncreaseHealth(DrinkHealthBenefit);
                    this.ConsoleOut.WriteLine("Ahhh refreshing water");
                }
                else
                {
                    this.ConsoleOut.WriteLine("Uh oh, your flask is empty");
                }
            }
        }

        private void ReadRune(string runeName)
        {
            Rune rune = null;

            if (!string.IsNullOrEmpty(runeName))
            {
                rune = this.player.UseItemByName(runeName) as Rune;
                if (rune == null)
                {
                    this.ConsoleOut.WriteLine("You do not have a that rune");
                }
            }
            else
            {
                rune = this.player.GetItemInUse<Rune>();
                if (rune == null)
                {
                    this.ConsoleOut.WriteLine("You are not holding a rune");
                }
            }

            if (rune != null)
            {
                rune.Read();
            }
        }

        public void Attack(string enemyName = "")
        {
            Enemy enemy = null;

            var enemies = this.CurrentRoom.GetEnemies();
            if (!string.IsNullOrEmpty(enemyName))
            {
                enemy = (from e in enemies
                         where e.Name.ToLower().StartsWith(enemyName.ToLower())
                         select e).FirstOrDefault();
            }
            else if (string.IsNullOrEmpty(enemyName) && enemies.Count() > 1)
            {
                this.ConsoleOut.WriteLine("Tell me which enemy you would like to attack");
            }
            else
            {
                enemy = enemies.FirstOrDefault();
            }

            if (enemy == null)
            {
                this.ConsoleOut.WriteLine("There is nobody to attack!");
            }
            else
            {
                int enemyStartHealth = enemy.Health;

                var result = this.player.Attack(enemy);
                if (!result.IsSuccess)
                {
                    this.ConsoleOut.WriteLine(result.Message);
                }
                else
                {
                    this.SoundPlayer.PlaySoundEffect("SwordsClashing");

                    int damage = enemyStartHealth - enemy.Health;
                    this.ConsoleOut.WriteLine(string.Format("You inflicted {0} points of damage on {1}", damage, enemy.Name));

                    foreach (var follower in this.player.Followers)
                    {
                        enemyStartHealth = enemy.Health;

                        var resFollower = follower.Attack(enemy);
                        if (!resFollower.IsSuccess)
                        {
                            this.ConsoleOut.WriteLine(resFollower.Message);
                        }
                        else
                        {
                            damage = enemyStartHealth - enemy.Health;
                            this.ConsoleOut.WriteLine(string.Format("{0} inflicted {1} points of damage on {2}", follower.Name, damage, enemy.Name));
                        }
                    }

                    if (enemy.IsDead())
                    {
                        this.ConsoleOut.WriteLine(string.Format("You have defeated {0}", enemy.Name));
                        if (enemy.GetItemCount<InventoryItem>() > 0)
                        {
                            this.ConsoleOut.WriteLine();
                            this.ConsoleOut.WriteLine(string.Format("{0} has the following items", enemy.Name));
                            this.ConsoleOut.WriteLine();
                            enemy.DisplayInventory();
                        }
//                        this.CurrentRoom.Characters.Remove(enemy);
                    }
                    else
                    {
                        // Counter attack
                        enemy.Attack(this.player);
                        this.ConsoleOut.WriteLine(string.Format("{0} now has {1} health remaining", enemy.Name, enemy.Health));
                        this.ConsoleOut.WriteLine(string.Format("{0} has counter attacked and you now have {1} health remaining", enemy.Name, this.player.Health));
                    }
                }
            }
        }

        private void TalkTo(string characterName)
        {
            Character character = this.CurrentRoom.GetCharacterByName(characterName);
            if (character == null)
            {
                this.ConsoleOut.WriteLine(string.Format("{0} is not a character", characterName));
                return;
            }

            if (character.IsDead())
            {
                this.ConsoleOut.WriteLine(string.Format("{0} is pretty dead right now and doesn't have much to say about it", characterName));
            }

            var conversationParticipant = character as IConversationParticipant;
            if (conversationParticipant == null)
            {
                this.ConsoleOut.WriteLine(string.Format("{0} is not the talkative type", characterName));
            }
            else if (this.currentConversation.IsParticipant(conversationParticipant.Name))
            {
                this.ConsoleOut.WriteLine(string.Format("You are already talking to {0}", characterName));
            }
            else
            {
                this.currentConversation.Join(conversationParticipant);
                this.ConsoleOut.WriteLine(string.Format("{0} is listening", characterName));
            }
        }

        private void Say(string dialogText)
        {
            this.currentConversation.Say(this.player.Name, dialogText);
        }

        public void DisplayGameIntro()
        {
            this.ConsoleOut.WriteLine("");
            this.ConsoleOut.WriteLine("You wake up to find yourself in a dark cavern.");
        }

        public void DisplayHelp()
        {
            this.ConsoleOut.WriteLine("");
            this.ConsoleOut.WriteLine("show map                                 ==> shows the map");
            this.ConsoleOut.WriteLine("show health                              ==> shows your character's health");
            this.ConsoleOut.WriteLine("look                                     ==> describes your surroundings");
            this.ConsoleOut.WriteLine("move north                               ==> move north");
            this.ConsoleOut.WriteLine("move south                               ==> move south");
            this.ConsoleOut.WriteLine("move east                                ==> move east");
            this.ConsoleOut.WriteLine("move west                                ==> move west");
            this.ConsoleOut.WriteLine("inv                                      ==> shows the items in your inventory");
            this.ConsoleOut.WriteLine("drink                                    ==> drink some water to restore health");
            this.ConsoleOut.WriteLine("use [item name]                          ==> prepares an inventory item to be used");
            this.ConsoleOut.WriteLine("take [item name]                         ==> takes an item in the room and puts it in your inventory");
            this.ConsoleOut.WriteLine("take [item name] from [character name]   ==> takes an item from a character and puts it in your inventory");
            this.ConsoleOut.WriteLine("drop [item name]                         ==> drops an item in your inventory and leaves it in the room");
            this.ConsoleOut.WriteLine("read [rune name]                         ==> reads a rune in your possession");
            this.ConsoleOut.WriteLine("attack [character name]                  ==> attacks a character in the room");
            this.ConsoleOut.WriteLine("followers                                ==> shows a list of your followers");
            this.ConsoleOut.WriteLine("talk to [character name]                 ==> starts a conversation with a character in the room");
            this.ConsoleOut.WriteLine("say [text]                               ==> say something in a conversation (start conversation with talk to)");
            this.ConsoleOut.WriteLine("history                                  ==> show command history");
            this.ConsoleOut.WriteLine("exit                                     ==> exits the game");
            this.ConsoleOut.WriteLine("");
        }

        public void DisplayCurrentRoomDescription()
        {
            var room = this.CurrentRoom;
            if (room != null)
            {
                room.DisplayDescription();
            }
        }

        public void DisplayFollowers()
        {
            if (this.Player.Followers.Count() <= 0)
            {
                this.ConsoleOut.WriteLine("You have no followers");
            }
            else
            {
                this.ConsoleOut.Write("You are being followed by");

                foreach (var curFollower in this.Player.Followers)
                {
                    this.ConsoleOut.Write(" " + curFollower.Name);
                }
            }
        }

        private void DisplayPlayerInventory()
        {
            this.player.DisplayInventory();
        }

        public void DisplayCommandSeparator()
        {
            this.ConsoleOut.WriteLine("---------------------------------------------------------");
        }

        private void DisplayCommandHistory()
        {
            this.ConsoleOut.WriteLine("");
            this.ConsoleOut.WriteLine("Here are all of the command that you have typed");
            this.ConsoleOut.WriteLine("");
            foreach (var cmdLine in this.commandHistory)
            {
                this.ConsoleOut.WriteLine(cmdLine);
            }
        }

        #endregion
    }
}
