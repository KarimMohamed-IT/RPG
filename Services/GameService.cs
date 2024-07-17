using RPG.Data;
using RPG.Entities;
using RPG.Entities.Database;
using System.Security;

namespace RPG.Services
{
    public class GameService
    {
        private readonly GameContext _context;
        private const char emptyMapSpace = '▒';
        char[,] map = new char[10, 10];
        Dictionary<KeyValuePair<int, int>, Monster> monsters;
        private Hero hero;
        private Random random;
        private KeyValuePair<int, int> currentLocation;

        public GameService(GameContext context, Hero hero)
        {
            _context = context;
            random = new Random();
            monsters = new Dictionary<KeyValuePair<int, int>, Monster>();

            currentLocation = new KeyValuePair<int, int>(1, 1);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map[i, j] = emptyMapSpace;
                }
            }

            this.hero = hero;

            MarkCurrentLocationOnMap();
        }

        public void InGame()
        {
            Console.Clear();
            string gameStartsText = "GAME STARTS!!!";
            for (int i = 0; i < gameStartsText.Length; i++)
            {
                Console.Write(gameStartsText[i]);
                if (i < 10)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            bool ifPaused = false;

            Console.Beep();

            while (true)
            {

                if (!ifPaused)
                {
                    Console.Clear();
                    if (hero.Health <= 0)
                    {
                        hero.Health = 0;
                        PrintMap();
                        Console.WriteLine();
                        Program.ErrorMessage("YOU DIED !!!", true);
                        Console.WriteLine("GAME OVER!!!", true);
                        break;
                    }

                    PrintMap();
                }

                var input = Console.ReadKey(true).KeyChar;

                if (ifPaused)
                {
                    ifPaused = false;
                    continue;
                }

                switch (input)
                {
                    case '1':
                        Console.WriteLine("Attack");
                        ShowTargets();
                        break;

                    case '2':
                        Console.WriteLine("Move");
                        Console.WriteLine("Choose direction:");
                        GetsDirection();

                        break;

                    case '\t':

                        Console.Clear();
                        if (!ifPaused)
                        {
                            Console.WriteLine($"{emptyMapSpace} {emptyMapSpace} STATS: {emptyMapSpace} {emptyMapSpace}");
                            Console.WriteLine();
                            Console.WriteLine($"Attack Damage: {hero.Damage}");
                            Console.WriteLine();
                            Console.WriteLine($"Max Range: {hero.Range}");
                            Console.WriteLine(Environment.NewLine);
                            Console.Beep();
                            Console.WriteLine("Press any key to unpause.");

                            ifPaused = true;
                        }

                        continue;

                    default:
                        Program.ErrorMessage("Invalid input. Please choose 1 to Attack and 2 to Move", true);
                        continue;
                }

                ControlMonsters();
                SpawnMonsters();
            }
        }

        private void ControlMonsters()
        {
            if (monsters.Any())
            {
                foreach (var monster in monsters.ToList())
                {
                    int currMonsterRow = monster.Key.Key;
                    int currMonsterCol = monster.Key.Value;

                    if (Math.Abs(currMonsterRow - currentLocation.Key) <= 1 && Math.Abs(currMonsterCol - currentLocation.Value) <= 1)
                    {
                        hero.Health -= monster.Value.Damage;
                    }
                    else
                    {
                        int oldRow = currMonsterRow;
                        int oldCol = currMonsterCol;

                        if (currMonsterRow > currentLocation.Key)
                        {
                            currMonsterRow--;
                        }
                        else
                        {
                            currMonsterRow++;
                        }

                        if (currMonsterCol > currentLocation.Value)
                        {
                            currMonsterCol--;
                        }
                        else if (currMonsterCol < currentLocation.Value)
                        {
                            currMonsterCol++;
                        }

                        if (monsters.ContainsKey(new KeyValuePair<int, int>(currMonsterRow, currMonsterCol)))
                        {
                            if (monsters.ContainsKey(new KeyValuePair<int, int>(oldRow, currMonsterCol)))
                            {
                                if (monsters.ContainsKey(new KeyValuePair<int, int>(currMonsterRow, oldCol)))
                                {
                                    break;
                                }
                                else
                                {
                                    currMonsterCol = oldCol;
                                }
                            }
                            else
                            {
                                currMonsterRow = oldRow;
                            }
                        }

                        map[oldRow, oldCol] = emptyMapSpace;

                        monsters.Remove(monster.Key);

                        currMonsterRow = Math.Max(0, Math.Min(map.GetLength(0) - 1, currMonsterRow));
                        currMonsterCol = Math.Max(0, Math.Min(map.GetLength(1) - 1, currMonsterCol));

                        monsters.Add(new KeyValuePair<int, int>(currMonsterRow, currMonsterCol), monster.Value);
                        map[currMonsterRow, currMonsterCol] = monster.Value.Symbol;
                    }
                }
            }
        }

        private void SpawnMonsters()
        {
            int randomRow;
            int randomCol;
            while (true)
            {
                int rows = map.GetLength(0);
                int cols = map.GetLength(1);

                randomRow = random.Next(rows);
                randomCol = random.Next(cols);
                if (monsters.ContainsKey(new KeyValuePair<int, int>(randomRow, randomCol)) == false &&
                  (currentLocation.Key != randomRow && currentLocation.Value != randomCol))
                {
                    break;
                }
            }
            var monster = new Monster(random);
            monsters.Add(new KeyValuePair<int, int>(randomRow, randomCol), monster);
            map[randomRow, randomCol] = monster.Symbol;

        }

        private void GetsDirection()
        {
            bool breakLoop = false;
            int move = 0;
            while (true)
            {
                var direction = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (direction)
                {
                    case 'W':
                        Console.WriteLine("UP");
                        move = CheckMoves(GetAvailableMovesUp(), "Cannot move up!");
                        MoveUp(move);
                        breakLoop = true;

                        break;
                    case 'A':

                        Console.WriteLine("LEFT");
                        move = CheckMoves(GetAvailableMovesLeft(), "Cannot move left!");
                        MoveLeft(move);
                        breakLoop = true;

                        break;
                    case 'S':
                        Console.WriteLine("DOWN");
                        move = CheckMoves(GetAvailableMovesDown(), "Cannot move down!");
                        MoveDown(move);
                        breakLoop = true;

                        break;
                    case 'D':
                        Console.WriteLine("RIGHT");
                        move = CheckMoves(GetAvailableMovesRight(), "Cannot move right!");
                        MoveRight(move);
                        breakLoop = true;
                        break;
                    case 'X':
                        Console.WriteLine("DOWN-RIGHT");
                        move = CheckMoves(int.Min(GetAvailableMovesDown(), GetAvailableMovesRight()), "Cannot move down-right!");

                        MoveDown(move);
                        MoveRight(move);
                        breakLoop = true;
                        break;
                    case 'Z':
                        Console.WriteLine("DOWN-LEFT");
                        move = CheckMoves(int.Min(GetAvailableMovesDown(), GetAvailableMovesLeft()), "Cannot move down-left!");

                        MoveDown(move);
                        MoveLeft(move);
                        breakLoop = true;
                        break;
                    case 'E':
                        Console.WriteLine("UP-RIGHT");
                        move = CheckMoves(int.Min(GetAvailableMovesUp(), GetAvailableMovesRight()), "Cannot move up-right!");

                        MoveUp(move);
                        MoveRight(move);
                        breakLoop = true;
                        break;
                    case 'Q':
                        Console.WriteLine("UP-LEFT");
                        move = CheckMoves(int.Min(GetAvailableMovesUp(), GetAvailableMovesLeft()), "Cannot move up-left!");

                        MoveUp(move);
                        MoveLeft(move);
                        breakLoop = true;
                        break;

                    default:
                        Program.ErrorMessage("Invalid move!");
                        break;
                }

                if (breakLoop)
                {
                    MarkCurrentLocationOnMap();
                    break;
                }
            }
        }

        private void EmptyCurrentLocationOnMap()
        {
            map[currentLocation.Key, currentLocation.Value] = emptyMapSpace;
        }

        private void MarkCurrentLocationOnMap()
        {
            map[currentLocation.Key, currentLocation.Value] = hero.Symbol;
        }

        private int GetAvailableMovesUp()
        {
            return int.Min((map.GetLength(0) - 1) - currentLocation.Key, hero.Range);
        }
        private int GetAvailableMovesDown()
        {
            return int.Min(currentLocation.Key, hero.Range);
        }
        private int GetAvailableMovesLeft()
        {
            return int.Min(currentLocation.Value, hero.Range);
        }
        private int GetAvailableMovesRight()
        {
            return int.Min((map.GetLength(0) - 1) - currentLocation.Value, hero.Range);
        }

        private void MoveUp(int move)
        {
            EmptyCurrentLocationOnMap();
            currentLocation = new KeyValuePair<int, int>(currentLocation.Key + move, currentLocation.Value);
        }
        private void MoveDown(int move)
        {
            EmptyCurrentLocationOnMap();
            currentLocation = new KeyValuePair<int, int>(currentLocation.Key - move, currentLocation.Value);
        }
        private void MoveLeft(int move)
        {
            EmptyCurrentLocationOnMap();
            currentLocation = new KeyValuePair<int, int>(currentLocation.Key, currentLocation.Value - move);
        }
        private void MoveRight(int move)
        {
            EmptyCurrentLocationOnMap();
            currentLocation = new KeyValuePair<int, int>(currentLocation.Key, currentLocation.Value + move);

        }
        private int CheckMoves(int availableMoves, string errorText)
        {
            if (availableMoves == 0)
            {
                Program.ErrorMessage(errorText, true);
            }
            else if (availableMoves > 1)
            {
                while (true)
                {
                    Console.WriteLine($"Please choose steps between 1 and {availableMoves}");

                    int input = Program.ReturnsIntIfDigitConsoleKeyInfo(Console.ReadKey(true));
                    if (input > 0 && input <= availableMoves)
                    {
                        availableMoves = input;
                        break;
                    }
                }
            }

            return availableMoves;
        }

        public void SaveGameLog(Hero hero)
        {
            var gameLog = new GameLog
            {
                Character = hero.GetType().Name,
                Time_Created = DateTime.Now, // Or use UTC time as per best practices
                Buff_Strength_Points = hero.Strength,
                Buff_Agility_Points = hero.Agility,
                Buff_Intelligence_Points = hero.Intelligence
            };

            _context.GameLogs.Add(gameLog);
            _context.SaveChanges();

            Console.WriteLine($"Game log saved for {hero.GetType().Name}.");
        }

        private void PrintMap()
        {
            Console.WriteLine($"Health: {hero.Health}   Mana: {hero.Mana}                    Press TAB to pause.");
            Console.WriteLine(Environment.NewLine);
            for (int i = map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Choose action");
            Console.WriteLine("1) Attack");
            Console.WriteLine("2) Move");
        }

        private void ShowTargets()
        {
            var nearbyMonsters = monsters.Where(x => x.Key.Key <= currentLocation.Key + hero.Range &&
            x.Key.Key >= currentLocation.Key - hero.Range &&
            x.Key.Value <= currentLocation.Value + hero.Range &&
            x.Key.Value >= currentLocation.Value - hero.Range).ToList();

            if (nearbyMonsters.Any())
            {

                for (int i = 0; i < nearbyMonsters.Count; i++)
                {
                    Console.WriteLine($"{i}) target with remaining blood {nearbyMonsters[i].Value.Health}");
                }

                if (hero is Mage && hero.Mana >= 6)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Press 'U' for Ultimate ( cost: 6 mana.  6 damage to all monsters.)");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                while (true)
                {

                    Console.WriteLine("Which one to attack?");

                    var input = Console.ReadKey();

                    if (hero is Mage)
                    {
                        if (char.ToUpper(input.KeyChar) == 'U' && hero.Mana >= 6)
                        {
                            hero.Mana -= 6;
                            foreach (var monsterEntry in monsters)
                            {
                                monsterEntry.Value.Health -= 6;

                                // Ensure that health doesn't drop below zero
                                if (monsterEntry.Value.Health < 0)
                                {
                                    monsters.Remove(monsterEntry.Key);
                                    map[monsterEntry.Key.Key, monsterEntry.Key.Value] = emptyMapSpace;
                                }
                            }

                            break;
                        }
                    }

                    int inputDigit = Program.ReturnsIntIfDigitConsoleKeyInfo(input);
                    if (inputDigit < 0 || inputDigit >= nearbyMonsters.Count)
                    {
                        Program.ErrorMessage("Invalid input! Please select one of monsters listed above");
                    }
                    else
                    {
                        if (nearbyMonsters[inputDigit].Value.Health <= hero.Damage)
                        {
                            monsters.Remove(nearbyMonsters[inputDigit].Key);
                            map[nearbyMonsters[inputDigit].Key.Key, nearbyMonsters[inputDigit].Key.Value] = emptyMapSpace;
                        }
                        else
                        {
                            monsters[nearbyMonsters[inputDigit].Key].Health -= hero.Damage;
                        }

                        break;
                    }
                }
            }
            else
            {
                Program.ErrorMessage("No available targets in your range", true);
            }
        }
    }
}
