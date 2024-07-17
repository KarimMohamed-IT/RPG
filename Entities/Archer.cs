namespace RPG.Entities
{
    public class Archer : Hero
    {
        public override int Strength { get; protected set; } = 2;
        public override int Agility { get; protected set; } = 4;
        public override int Intelligence { get; protected set; } = 0;
        public override int Range { get; set; } = 2;
        public override char Symbol { get;  set; } = '#';
    }
}
