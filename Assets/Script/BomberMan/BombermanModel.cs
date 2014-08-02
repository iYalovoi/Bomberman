namespace Assets.Script
{
    public class BombermanModel
    {
        //Normal
        public int BombCount = 1;
        public int Radius = 1;
        public bool FlamePass;
        public bool Invincible;
        public bool RemoteControl;
        public float Speed = Constants.BasePlayerSpeed;

        //Developer
        //public int BombCount = 100;
        //public int Radius = 100;
        //public bool FlamePass = true;
        //public bool Invincible = true;
        //public bool RemoteControl = true;
        //public float Speed = 5f;

        public void Reset()
        {
            BombCount = 1;
            Radius = 1;
            FlamePass = false;
            Invincible = false;
            RemoteControl = false;
            Speed = Constants.BasePlayerSpeed;
        }
    }
}