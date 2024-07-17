namespace RPG.Entities
{
    public abstract class Pawn
    {
        public abstract int Strength { get; protected set; }

        public abstract int Agility { get; protected set; }

        public abstract int Intelligence { get; protected set; }

        public abstract int Range { get; set; }

        public abstract char Symbol { get; set; }

        public int Health { get; set; }

        public int Mana { get; set; }

        public int Damage { get; set; }

        public void SetUp()
        {
            Health = Strength * 5;
            Mana = Intelligence * 3;
            Damage = Agility * 2;
        }
    }
}
