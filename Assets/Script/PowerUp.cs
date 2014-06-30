using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public class PowerUp : MonoBehaviour, ITarget
    {

        public Powers Power;
        public bool IsConsumed;
        private Animator _animator;
        public Sprite Sprite;

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
            var rnd = Random.Range(0, 8);
            Power = (Powers)(rnd == 0 ? 0 : 1 << (rnd - 1));
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("Power", (float)Power);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player" && !IsConsumed)
            {
                var player = col.gameObject.GetComponent<Bomberman>();
                player.AcceptPower(Power);
                _animator.SetTrigger("Consume");
                IsConsumed = true;
            }
        }

        void Consumed()
        {
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            IsConsumed = true;
            _animator.SetTrigger("Consume");
        }
    }
}
