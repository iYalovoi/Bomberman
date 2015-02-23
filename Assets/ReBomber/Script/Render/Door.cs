using System.Collections;
using Assets.Script.Utility;
using UnityEngine;
using System.Linq;

namespace Assets.Script
{
    public class Door : ContainerBase, ITarget
    {
        private Animator _animator;
        //private LevelFactory _levelUpFactory;
        private Messenger _messenger;
        public AudioSource OpenSound;

		private bool timeIsOver = false; 
		private System.Action countdownSubscription;

        protected override void Start()
        {
            base.Start();
            //DI Unity way; Shitty way; Igor.
            //_levelUpFactory = FindObjectOfType<LevelFactory>();
            _animator = GetComponent<Animator>();
        }

        private void OnInjected(Messenger messenger, LevelModel model)
        {
            _messenger = messenger;
			countdownSubscription = _messenger.Subscribe(Signals.CountdownOver, ()=>timeIsOver = true);
        }

		private void OnDestroy()
		{
			countdownSubscription();
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
			var openCondition = (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) || timeIsOver;
            if (col.gameObject.tag == "ReBomber" && openCondition && collidersInArea.All(o => o.gameObject.tag != "Wall"))
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