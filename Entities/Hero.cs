namespace RPG.Entities
{
    public abstract class Hero : Pawn
    {
        private int statPoints;

        public Hero()
        {
            statPoints = 3;
        }

        public int StatPoints => statPoints;


        public bool CheckRemainingStats(int number)
        {
            if (number < 0)
            {
                Program.ErrorMessage("Please select a number equal or higher than 0.");
                return false;
            }
            else if (number > StatPoints)
            {
                Program.ErrorMessage($"Insufficient amount! You have {StatPoints} point left.");
                return false;
            }

            return true;
        }

        public void AddToStrength(int stat)
        {
            Strength += stat;
            statPoints -= stat;
        }

        public void AddToAgility(int stat)
        {
            Agility += stat;
            statPoints -= stat;
        }

        public void AddToIntelligence(int stat)
        {
            Intelligence += stat;
            statPoints -= stat;
        }
    }
}
