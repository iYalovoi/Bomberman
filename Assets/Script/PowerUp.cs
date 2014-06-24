using System;
using UnityEngine;

namespace Assets.Script
{

    [Flags]
    public enum Powers
    {
        BombUp = 0,
        Fire = 1 << 0,
        BombPass = 1 << 1,
        FlamePass = 1 << 2,
        Mystery = 1 << 3,
        RemoteControl = 1 << 4,
        Speed = 1 << 5,
        WallPass = 1 << 6,
    }

    public class PowerUp : MonoBehaviour
    {

        public Powers Power;
        private Animator _animator;
        
        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("Power", (float)Power);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var player = col.gameObject.GetComponent<Bomberman>();
                player.Powers |= Power;
                Debug.Log("Pouserlksdjf");
            }
        }
    }
}
