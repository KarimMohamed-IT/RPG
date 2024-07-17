namespace RPG.Entities
{
    public class Mage : Hero
    {
        public override int Strength { get; protected set; } = 2;
        public override int Agility { get; protected set; } = 1;
        public override int Intelligence { get; protected set; } = 3;
        public override int Range { get; set; } = 3;
        public override char Symbol { get; set; } = '*';

    }
}
