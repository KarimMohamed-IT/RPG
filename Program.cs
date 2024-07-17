using RPG.Data;
using RPG.Entities;
using RPG.Services;
using System;

namespace RPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (var context = new GameContext())
            {
                context.Database.EnsureCreated();
                Hero hero = SelectHero();

                BuffHero(hero);

                hero.SetUp();

                var gameService = new GameService(context, hero);

                gameService.SaveGameLog(hero);

                gameService.InGame();
            }
        }

        private static Hero SelectHero()
        {
            Hero hero;

            // Main Menu
            Console.WriteLine("Welcome!");
            Console.WriteLine("Press any key to play.");
            Console.ReadKey(true);

            // Character Select
            while (true)
            {
                Console.WriteLine("Choose character type:");
                Console.WriteLine("Options:");
                Console.WriteLine("1) Warrior");
                Console.WriteLine("2) Archer");
                Console.WriteLine("3) Mage");

                // Check if the user has written a number and it's between 1 and 3
                int selectHero = ReturnsIntIfDigitConsoleKeyInfo(Console.ReadKey(true));
                if (selectHero > 0 && selectHero < 4)
                {
                    hero = selectHero switch
                    {
                        1 => new Warrior(),
                        2 => new Archer(),
                        3 => new Mage(),
                        _ => throw new ArgumentException("Invalid selection")
                    };

                    Console.WriteLine($"Your pick: {hero.GetType().Name}");
                    break;
                }
                else
                {
                    // Message to the user to write correct input
                    ErrorMessage("Please write a number from 1 to 3 that matches the character you want to select.");
                }
            }

            return hero;
        }

        private static void BuffHero(Hero hero)
        {
            // Question to Buff the Character
            while (true)
            {
                Console.WriteLine("Would you like to buff up your stats before starting? (Limit: 3 points total)");
                Console.WriteLine("Response (Y/N): ");
                string buffResponse = Console.ReadKey(true).KeyChar.ToString().ToUpper();

                if (buffResponse == "Y")
                {
                    Console.WriteLine("You can buff 3 stats: Strength, Agility and Intelligence.");
                    int toBuff;

                    while (true)
                    {
                        BuffQuestions("Strength", hero.StatPoints);
                        toBuff = ReturnsIntIfDigitConsoleKeyInfo(Console.ReadKey());
                        Console.WriteLine();
                        if (hero.CheckRemainingStats(toBuff))
                        {
                            hero.AddToStrength(toBuff);
                            Console.WriteLine();
                            break;
                        }
                    }

                    while (true)
                    {
                        BuffQuestions("Agility", hero.StatPoints);
                        toBuff = ReturnsIntIfDigitConsoleKeyInfo(Console.ReadKey());
                        Console.WriteLine();
                        if (hero.CheckRemainingStats(toBuff))
                        {
                            hero.AddToAgility(toBuff);
                            Console.WriteLine();
                            break;
                        }
                    }

                    while (true)
                    {
                        BuffQuestions("Intelligence", hero.StatPoints);
                        toBuff = ReturnsIntIfDigitConsoleKeyInfo(Console.ReadKey());
                        Console.WriteLine();
                        if (hero.CheckRemainingStats(toBuff))
                        {
                            hero.AddToIntelligence(toBuff);
                            Console.WriteLine();
                            break;
                        }
                    }

                    break;
                }
                else if (buffResponse == "N")
                {
                    Console.WriteLine("Skipping Buff.");
                    break;
                }
                else
                {
                    Program.ErrorMessage("Invalid input!");
                    Console.WriteLine();
                }
            }
        }

        public static void ErrorMessage(string message, bool wait = false)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            if (wait)
            {
                Thread.Sleep(750);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static int ReturnsIntIfDigitConsoleKeyInfo(ConsoleKeyInfo consoleKeyInfo)
        {
            // Returns the number if it's a digit
            if (char.IsDigit(consoleKeyInfo.KeyChar))
            {
                return int.Parse(consoleKeyInfo.KeyChar.ToString());
            }

            // Returns -1 if it's not a digit
            return -1;
        }

        // Asks user how much they want to buff the current stat
        private static void BuffQuestions(string statToBuff, int remainingStats)
        {
            Console.WriteLine($"Please write how much you want to buff {statToBuff}.");
            Console.WriteLine($"Remaining Points: {remainingStats}");
            Console.Write($"Add to {statToBuff}: ");
        }
    }
}
