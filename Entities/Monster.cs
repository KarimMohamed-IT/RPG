namespace RPG.Entities
{
    public class Monster : Pawn
    {

        public Monster(Random random)
        {
            Strength = random.Next(1, 4);
            Agility = random.Next(1, 4);
            Intelligence = random.Next(1, 4);
            SetUp();
        }

        public override int Strength { get; protected set; }
        public override int Agility { get; protected set; }
        public override int Intelligence { get; protected set; }
        public override int Range { get; set; } = 1;
        public override char Symbol { get; set; } = '¤';
            }
}
