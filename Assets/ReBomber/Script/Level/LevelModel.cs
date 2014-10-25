namespace Assets.Script
{
    public class LevelModel
    {
        public int CurrentLevel = 49;
        public int LevelCap = 50;

        public void Reset()
        {
            CurrentLevel = 0;
        }
    }
}