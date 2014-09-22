namespace Assets.Script
{
    public class LevelModel
    {
        public int CurrentLevel = 0;
        public int LevelCap = 50;

        public void Reset()
        {
            CurrentLevel = 0;
        }
    }
}