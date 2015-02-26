namespace Assets.Script
{
    public class GameModel
    {
        public int CurrentLevel = 0;
        public int LevelCap = 50;
        public bool IsEasy;

        public void Reset()
        {
            CurrentLevel = 0;
            IsEasy = false;
        }
    }
}