using System;
using System.Collections.Generic;
using System.Linq;

using AdventureGameEngine;

namespace Dungeon
{
    /// <summary>
    /// This class represents the entire game map.
    /// </summary>
    public class DungeonBoard : GameBoard
    {
        private IConsoleOutputService consoleOut;
        private ISoundPlayerService soundPlayer;
        private IUserPromptService userPrompt;

        public const int NumColumns = 24;
        public const int NumRows = 24;

        private readonly string mapName;
        private Position startPosition = Position.Create(NumRows - 1, NumColumns / 2);
        private List<Room> rooms = new List<Room>();
        private List<RoomFeature> roomFeatures = new List<RoomFeature>();
        private List<Weapon> weapons = new List<Weapon>();
        private List<Friend> friends = new List<Friend>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<InventoryItem> items = new List<InventoryItem>();
        private Random randomRoomGenerator = new Random(DateTime.Now.Minute);

        public DungeonBoard(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, IUserPromptService userPrompt) :
            this(consoleOut, soundPlayer, userPrompt, null)
        {
        }

        public DungeonBoard(IConsoleOutputService consoleOut, ISoundPlayerService soundPlayer, IUserPromptService userPrompt, string mapName) :
            base(NumRows, NumColumns)
        {
            this.consoleOut = consoleOut;
            this.soundPlayer = soundPlayer;
            this.userPrompt = userPrompt;
            this.mapName = mapName;
        }

        #region Init Methods

        public override void Initialize()
        {
            base.Initialize();

            // Initialize the entire map with walls first
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    this.Tiles[i, j] = new Wall(this.consoleOut);
                }
            }

            bool isMazeGenerated = false;

            if (!string.IsNullOrEmpty(mapName))
            {
                if (mapName.ToLower().CompareTo("singlecorridor") == 0)
                {
                    this.InitMazeSingleCorridor();
                    isMazeGenerated = true;
                }
                else if (mapName.ToLower().CompareTo("crosspattern") == 0)
                {
                    this.InitMazeCrossPattern();
                    isMazeGenerated = true;
                }
                else if (mapName.ToLower().CompareTo("crosswithsquare") == 0)
                {
                    this.InitMazeCrossWithSquare();
                    isMazeGenerated = true;
                }
            }

            if (!isMazeGenerated)
            {
                this.InitMazeRandomCorridors();
            }

            var runes = new RuneCollection();
            this.InitStartRoom(this.rooms.First());
            this.InitRoomFeatures(runes);
            this.InitWeapons();
            this.InitFriends(runes);
            this.InitEnemies(runes);
        }

        public void InitStartRoom(Room room)
        {
            room.Name = "Start";
        }

        public void InitWeapons()
        {
            // Add a Katana
            var katana = new Katana(this.consoleOut, this.soundPlayer);
            this.Weapons.Add(katana);
            var curRoom = this.PickRandomRoom();
            curRoom.Items.Add(katana);

            // Add a Broad Sword
            var broadSword = new BroadSword(this.consoleOut, this.soundPlayer);
            this.Weapons.Add(broadSword);
            curRoom = this.PickRandomRoom();
            curRoom.Items.Add(broadSword);

            // Add a Battle Axe
            var battleAxe = new BattleAxe(this.consoleOut, this.soundPlayer);
            this.Weapons.Add(battleAxe);
            curRoom = this.PickRandomRoom();
            curRoom.Items.Add(battleAxe);

            // Add the Bow
            var bow = new Bow(this.consoleOut, this.soundPlayer);
            this.Weapons.Add(bow);
            curRoom = this.PickRandomRoom();
            curRoom.Items.Add(bow);

            // Add the Staff
            var staff = new Staff(this.consoleOut, this.soundPlayer);
            this.Weapons.Add(staff);
            curRoom = this.PickRandomRoom();
            curRoom.Items.Add(staff);
        }

        public void InitFriends(RuneCollection runes)
        {
            // Add Ginger to the maze
            var ginger = new Dog(this.consoleOut, this.soundPlayer, "Ginger", "A cute but somewhat dim puppy dog");
            this.Friends.Add(ginger);
            Room curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(ginger);

            // Add Bombur to the maze
            var dwarf1 = new Dwarf(this.consoleOut, this.soundPlayer, "Bombur", "Bombur son of Dwalin");
            dwarf1.AddToInventory(new Staff(this.consoleOut, this.soundPlayer));
            dwarf1.UseItem<Staff>();
            if (runes.Count > 0)
            {
                dwarf1.AddToInventory(runes.UseNext());
            }
            this.Friends.Add(dwarf1);
            curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(dwarf1);
        }

        public void InitEnemies(RuneCollection runes)
        {
            // Add Orcs to the maze
            var orc1 = new Orc(this.consoleOut, this.soundPlayer, "Grundle", "A nasty little orc");
            orc1.AddToInventory(new BattleAxe(this.consoleOut, this.soundPlayer));
            if (runes.Count > 0)
            {
                orc1.AddToInventory(runes.UseNext());
            }
            this.Enemies.Add(orc1);
            var curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(orc1);

            var orc2 = new Orc(this.consoleOut, this.soundPlayer, "Brundle", "A nasty little orc");
            if (runes.Count > 0)
            {
                orc2.AddToInventory(runes.UseNext());
            }
            orc2.AddToInventory(new Dagger(this.consoleOut, this.soundPlayer));
            this.Enemies.Add(orc2);
            curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(orc2);

            // Add Trolls to the maze
            var troll1 = new Troll(this.consoleOut, this.soundPlayer, "Slackfart", "An ugly, stupid troll");
            troll1.AddToInventory(runes.UseNext());
            if (runes.Count > 0)
            {
                troll1.AddToInventory(runes.UseNext());
            }
            this.Enemies.Add(troll1);
            curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(troll1);

            var troll2 = new Troll(this.consoleOut, this.soundPlayer, "Slugbutt", "An ugly, stupid troll");
            troll2.AddToInventory(runes.UseNext());
            if (runes.Count > 0)
            {
                troll2.AddToInventory(runes.UseNext());
            }
            this.Enemies.Add(troll2);
            curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(troll2);

            var troll3 = new Troll(this.consoleOut, this.soundPlayer, "Frickbar the very unpleasant", "An ugly, stupid troll");
            if (runes.Count > 0)
            {
                troll3.AddToInventory(runes.UseNext());
            }
            this.Enemies.Add(troll3);
            curRoom = this.PickRandomRoom();
            curRoom.Characters.Add(troll3);
        }

        public void InitRoomFeatures(RuneCollection runes)
        {
            // Generate a password for the escape portal
            var pwdGenerator = new PasswordGenerator();
            var escapePortalPassword = pwdGenerator.GetPassword();

            // Create a rune for each password hint and add them
            // to a list that can be passed around to distribute
            // runes throughout the maze
            int runeNum = 1;
            foreach (var curHint in escapePortalPassword.Hints)
            {
                var runeName = this.GenerateRuneName(runeNum);
                runes.Add(new Rune(this.consoleOut, this.soundPlayer, runeName, curHint));
                runeNum = runeNum + 1;
            }

            var deadpool = new DeadPool();
            this.RoomFeatures.Add(deadpool);
            var curRoom = this.PickRandomRoom();
            curRoom.Features.Add(deadpool);

            var pool1 = new WaterPool(this.consoleOut, this.soundPlayer);
            this.RoomFeatures.Add(pool1);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(pool1);

            var pool2 = new WaterPool(this.consoleOut, this.soundPlayer);
            this.RoomFeatures.Add(pool2);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(pool2);

            var pool3 = new WaterPool(this.consoleOut, this.soundPlayer);
            this.RoomFeatures.Add(pool3);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(pool3);

            var treasureChest1 = new TreasureChest(this.consoleOut, this.soundPlayer, 10);
            this.RoomFeatures.Add(treasureChest1);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(treasureChest1);

            var treasureChest2 = new TreasureChest(this.consoleOut, this.soundPlayer, 25);
            this.RoomFeatures.Add(treasureChest2);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(treasureChest2);

            var treasureChest3 = new TreasureChest(this.consoleOut, this.soundPlayer, 15);
            this.RoomFeatures.Add(treasureChest3);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(treasureChest3);

            var skeleton = new Skeleton(runes.UseNext());
            this.RoomFeatures.Add(skeleton);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(skeleton);

            var startPortal = new Portal(this.userPrompt,
                "Portal",
                "A portal that leads somewhere - hopefully better than your current location!",
                this.StartPosition,
                null,
                0);
            this.RoomFeatures.Add(startPortal);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(startPortal);

            var escapePortal = new Portal(this.userPrompt,
                "Escape Portal",
                "The only way out of this hellish maze. Hope you have the right password!",
                GameBoard.Position.Undefined,
                escapePortalPassword,
                3);
            this.RoomFeatures.Add(escapePortal);
            curRoom = this.PickRandomRoom();
            curRoom.Features.Add(escapePortal);
        }

        #endregion

        public override Position StartPosition
        {
            get { return this.startPosition; }
        }

        public override void Display(Position playerPos)
        {
            this.consoleOut.WriteLine();

            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns; j++)
                {
                    if (j == playerPos.Column && i == playerPos.Row)
                    {
                        this.consoleOut.Write("@ ");
                    }
                    else
                    {
                        var curEntry = this.Tiles[i, j];
                        curEntry.Display();
                        this.consoleOut.Write(" ");
                    }
                }
                this.consoleOut.WriteLine();
            }
        }

        protected List<Friend> Friends
        {
            get { return this.friends; }
        }

        protected List<Enemy> Enemies
        {
            get { return this.enemies; }
        }

        protected List<Weapon> Weapons
        {
            get { return this.weapons; }
        }

        protected List<RoomFeature> RoomFeatures
        {
            get { return this.roomFeatures; }
        }

        #region Private Implementation

        private void InitMazeSingleCorridor()
        {
            for (int i = this.StartPosition.Row; i > 0; i--)
            {
                var j = this.StartPosition.Column;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }
        }

        private void InitMazeCrossPattern()
        {
            for (int i = this.StartPosition.Row; i > 0; i--)
            {
                var j = this.StartPosition.Column;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            for (int j = 1; j < NumColumns - 1; j++)
            {
                var i = this.StartPosition.Row / 2;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }
        }

        private void InitMazeCrossWithSquare()
        {
            for (int i = this.StartPosition.Row; i > 0; i--)
            {
                var j = this.StartPosition.Column;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            for (int j = 1; j < NumColumns - 1; j++)
            {
                var i = (this.StartPosition.Row + 1) / 2;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            var horzMargin = (NumColumns - 1) / 4;
            var vertMargin = (NumRows - 1) / 4;

            for (int j = horzMargin; j < NumColumns - horzMargin; j++)
            {
                var i = vertMargin;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            for (int j = horzMargin; j < NumColumns - horzMargin; j++)
            {
                var i = NumRows - vertMargin;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            for (int i = vertMargin; i < NumRows - vertMargin + 1; i++)
            {
                var j = NumColumns - horzMargin;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            for (int i = vertMargin; i < NumRows - vertMargin + 1; i++)
            {
                var j = horzMargin;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }
        }

        private void InitMazeRandomCorridors()
        {
            var colRandomizer = new Random(DateTime.Now.Second);
            var rowRandomizer = new Random(DateTime.Now.Minute);
            var lenRandomizer = new Random(DateTime.Now.Second);

            // Add single long corridor from start position
            // to the other end of the maze, so that other
            // random corridors always connect.
            for (int i = this.StartPosition.Row; i > 0; i--)
            {
                var j = this.StartPosition.Column;
                var room = new Room(this.consoleOut);
                room.Name = string.Format("{0},{1}", i, j);
                this.rooms.Add(room);
                this.Tiles[i, j] = room;
            }

            const int numRowsPerBlock = 6;
            int numRandomRows = NumRows / numRowsPerBlock;
            for (var randomRowIdx = 0; randomRowIdx < numRandomRows; randomRowIdx++)
            {
                var curCorridorLen = lenRandomizer.Next(NumColumns / 2, NumColumns - 1);
                var curCorridorStartCol = (NumColumns / 2) - (curCorridorLen / 2);
                var curCorridorEndCol = curCorridorStartCol + curCorridorLen;
                var curCorridorStartRow = randomRowIdx * numRowsPerBlock;
                var curCorridorEndRow = Math.Min((randomRowIdx * numRowsPerBlock) + numRowsPerBlock - 1, NumRows - 1);
                var curRow = rowRandomizer.Next(Math.Max(1, curCorridorStartRow), Math.Min(curCorridorEndRow, NumRows - 2));
                if (curRow != this.StartPosition.Row)
                {
                    for (int j = curCorridorStartCol; j < curCorridorEndCol; j++)
                    {
                        int i = curRow;
                        var room = new Room(this.consoleOut);
                        room.Name = string.Format("{0},{1}", i, j);
                        this.rooms.Add(room);
                        this.Tiles[i, j] = room;
                    }
                }
            }

            const int numColsPerBlock = 10;
            int numRandomCols = NumColumns / numColsPerBlock;
            for (var randomColIdx = 0; randomColIdx < numRandomCols; randomColIdx++)
            {
                var curCorridorLen = lenRandomizer.Next(NumRows / 2, NumRows - 1);
                var curCorridorStartRow = (NumRows / 2) - (curCorridorLen / 2);
                var curCorridorEndRow = curCorridorStartRow + curCorridorLen;
                var curCorridorStartCol = randomColIdx * numColsPerBlock;
                var curCorridorEndCol = Math.Min((randomColIdx * numColsPerBlock) + numColsPerBlock - 1, NumColumns - 1);
                var curCol = rowRandomizer.Next(Math.Max(3, curCorridorStartCol), Math.Min(curCorridorEndCol, NumColumns - 3));
                for (int i = curCorridorStartRow; i < curCorridorEndRow; i++)
                {
                    int j = curCol;
                    var room = new Room(this.consoleOut);
                    room.Name = string.Format("{0},{1}", i, j);
                    this.rooms.Add(room);
                    this.Tiles[i, j] = room;
                }
            }
        }

        protected Room PickRandomRoom()
        {
            var randomNum = this.randomRoomGenerator.Next(this.rooms.Count);
            return this.rooms[randomNum];
        }

        private string GenerateRuneName(int runeNum)
        {
            string[] runeNames = new string[]
            {
                "Fehu",
                "Wunjo",
                "Raido",
                "Naudiz",
                "Ehwaz",
                "Dagaz",
                "Ansuz",
                "Gebo"
            };

            if (runeNum < runeNames.Length)
            {
                return runeNames[runeNum];
            }

            return string.Format("Rune{0}", runeNum);
        }

        #endregion
    }
}
