namespace RPG.Entities.Database
{
    public class GameLog
    {
        public int Id { get; set; }
        public string Character { get; set; }
        public DateTime Time_Created { get; set; }
        public int Buff_Strength_Points { get; set; }
        public int Buff_Agility_Points { get; set; }
        public int Buff_Intelligence_Points { get; set; }
    }
}
