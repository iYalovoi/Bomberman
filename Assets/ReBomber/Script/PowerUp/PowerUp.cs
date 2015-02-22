using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

namespace Assets.Script
{

    [Flags]
    public enum Powers
    {
        BombUp = 0,
        Fire = 1 << 0,
        BombPass = 1 << 1,
        FlamePass = 1 << 2,
        Immortal = 1 << 3,
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
        public AudioSource PickUp;

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponent<SpriteRenderer>().sprite = Sprite;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "ReBomber" && !IsConsumed)
            {
                var collidersInArea = Physics2D.OverlapPointAll(gameObject.transform.position);
                if (collidersInArea.All(o => o.gameObject.tag != "Wall"))
                {
                    var player = col.gameObject.GetComponent<Bomberman>();
                    player.AcceptPower(Power);
                    _animator.SetTrigger("Consume");
                    IsConsumed = true;
                    PickUp.Play();
                }
            }
        }

        IEnumerator Consumed()
        {
            while (PickUp.isPlaying)
                yield return new WaitForSeconds(0);
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            IsConsumed = true;
            _animator.SetTrigger("Consume");
        }
    }
}
