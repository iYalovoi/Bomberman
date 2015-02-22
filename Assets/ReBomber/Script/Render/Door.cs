using System.Collections;
using Assets.Script.Utility;
using UnityEngine;
using System.Linq;

namespace Assets.Script
{
    public class Door : ContainerBase, ITarget
    {
        private Animator _animator;
        private LevelFactory _levelUpFactory;
        private Messenger _messenger;
        public AudioSource OpenSound;

        protected override void Start()
        {
            base.Start();
            //DI Unity way; Shitty way; Igor.
            _levelUpFactory = FindObjectOfType<LevelFactory>();
            _animator = GetComponent<Animator>();
        }

        private void OnInjected(Messenger messenger, LevelModel model)
        {
            _messenger = messenger;
        }

        void Update()
        {
        }

        void Opened()
        {
            _messenger.Signal(Signals.DoorOpened);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            var collidersInArea = Physics2D.OverlapPointAll(gameObject.transform.position).ToList();
            if (col.gameObject.tag == "ReBomber" && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && collidersInArea.All(o => o.gameObject.tag != "Wall"))
            {
                _animator.SetTrigger("Open");
                var bomberman = col.gameObject.GetComponent<Bomberman>();
                bomberman.Restrained = true;
                OpenSound.Play();
            }
        }

        public void OnHit(GameObject striker)
        {
            _messenger.Signal(Signals.DoorHit);
        }
    }
}