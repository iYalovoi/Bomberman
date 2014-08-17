using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Utility;

namespace Assets.Script
{
    public class Enemy : ContainerBase, ITarget
    {
        public float MaxSpeed = Constants.SlowSpeed;
		public bool Dead = false;
        public int Bounty;
		public IEnemyBehaviour Behaviour = new RandomRoaming();

        protected Animator Animator;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            Animator = GetComponent<Animator>();
        }

        private Messenger _messenger;

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
        }

        public void Die()
        {
            if(!Dead)
            {
                Dead = true;
                Animator.SetTrigger("Die");
                _messenger.Signal(Bounty);
            }
        }

        public void OnHit(GameObject striker)
        {
            Die();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

		private Direction _way;


        private void FixedUpdate()
		{
			if(!Dead)
			{
				var newWay = Behaviour.FindWay(gameObject);
				//Only if we change direction
				if(newWay != Direction.Undefined && newWay != _way)
				{
					if(_way == Direction.Left)
						Animator.SetFloat("Direction", 1);
					if(_way == Direction.Right)
						Animator.SetFloat("Direction", -1);
					_way = newWay;
				}
				//Velocity needs to be updated each frame, Who changes it? : Aleksey
				//Physics is a bitch : Igor
				rigidbody2D.velocity = MaxSpeed * _way.ToVector2();
			}
			else 
				rigidbody2D.velocity = new Vector2();
		}

    }
}
