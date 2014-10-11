using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public interface IPowerUpFactory
    {
        GameObject Produce(Powers power);

        Powers Produce();
    }

    public class PowerUpFactory : MonoBehaviour, IPowerUpFactory
    {
        public Sprite BombUp;
        public Sprite Fire;
        public Sprite BombPass;
        public Sprite FlamePass;
        public Sprite Mystery;
        public Sprite RemoteControl;
        public Sprite Speed;
        public Sprite WallPass;

        public GameObject Prefab;

        private Dictionary<Powers, Sprite> _map;

        void Awake()
        {
            _map = new Dictionary<Powers, Sprite>
            {
                { Powers.BombUp, BombUp },
                { Powers.Fire, Fire },
                { Powers.BombPass, BombPass },
                { Powers.FlamePass, FlamePass },
                { Powers.Immortal, Mystery },
                { Powers.RemoteControl, RemoteControl },
                { Powers.Speed, Speed },
                { Powers.WallPass, WallPass },
            };
        }

        public GameObject Produce(Powers power)
        {           
            var retObj = Instantiate(Prefab) as GameObject;
            var powerScript = retObj.GetComponent<PowerUp>();            
            powerScript.Power = power;
            powerScript.Sprite = _map[power];
            return retObj;
        }

        public Powers Produce()
        {
            var rnd = Random.Range(0, 8);
            return (Powers)(rnd == 0 ? 0 : 1 << (rnd - 1));
        }
    }
}