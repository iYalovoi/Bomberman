using UnityEngine.SocialPlatforms.Impl;

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
        public int Lifes = 2;
        public int Score;

        public void Godlike()
        {
            BombCount = 100;
            Radius = 100;
            FlamePass = true;
            Invincible = true;
            RemoteControl = true;
            Speed = 5f;
        }

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