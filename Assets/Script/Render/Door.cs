using System.Collections;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Door : ContainerBase, ITarget
    {
        private Animator _animator;
        private LevelFactory _levelUpFactory;
        private Messenger _messenger;

        protected override void Start()
        {
            base.Start();
            //DI Unity way; Shitty way; Igor.
            _levelUpFactory = FindObjectOfType<LevelFactory>();
            _animator = GetComponent<Animator>();
        }

        private void OnInjected(Messenger messenger)
        {
            Debug.Log(messenger);
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
            if (col.gameObject.tag == "Player" && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                _animator.SetTrigger("Open");
                var bomberman = col.gameObject.GetComponent<Bomberman>();
                bomberman.Restrained = true;
            }                
        }

        public void OnHit(GameObject striker)
        {
        }
    }
}
