using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Utility;

namespace Assets.Script
{
    public class Enemy : ContainerBase, ITarget
    {
        public float MaxSpeed = Constants.LowSpeed;
        public bool Dead = false;
        public int Bounty;
        public IEnemyPattern Behaviour = new RandomRoaming();
        public EnemyTypes Type;

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
            if (!Dead)
            {
                Dead = true;
                Animator.SetTrigger("Die");
                _messenger.Signal(Bounty);
                GA.API.Design.NewEvent("Monster", (float)Type);
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

        private Vector2 _way;

        private void FixedUpdate()
        {
            if (!Dead)
            {
                var newWay = Behaviour.FindWay(gameObject);
                //Only if we change direction
                if (newWay != default(Vector2) && newWay != _way)
                    _way = newWay;
                //Velocity needs to be updated each frame, Who changes it? : Aleksey
                //Physics is a bitch : Igor
                //Adjust direction to keep monster on axis of movement
                if (_way.y == 0)
                {
                    var offset = transform.localPosition.y - MapDiscovery.GetTilePosition(gameObject, transform.localPosition).y;
                    if (Mathf.Abs(offset) > 0.05)
                        _way.y = offset > 0 ? 1 : -1;
                    else
                        _way.y = 0;
                }
                if (_way.x == 0)
                {
                    var offset = transform.localPosition.x - MapDiscovery.GetTilePosition(gameObject, transform.localPosition).x;
                    if (Mathf.Abs(offset) > 0.05)
                        _way.x = offset > 0 ? 1 : -1;
                    else
                        _way.x = 0;
                }
                rigidbody2D.velocity = MaxSpeed * _way;
                //We need to align monster to it's path
            }
            else
                rigidbody2D.velocity = new Vector2();
        }

    }
}