using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Utility;

namespace Assets.Script
{
    public class Enemy : MonoBehaviour, ITarget
    {
        public float MaxSpeed = Constants.SlowSpeed;
		public bool Dead = false;

        protected Animator Animator;

        // Use this for initialization
        void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public void Die()
        {
			Dead = true;
			Animator.SetTrigger("Die");
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
				//Kind of weird way to determine tile size? : Alexey
				//Yes, Indeed : Igor
				var tileSize = renderer.bounds.size.x;
				//Local position seems to be bottom left corner? : Alexey
				//That is true : Igor
				var localPosition = gameObject.transform.localPosition;
				//
				var currentTilePosition = new Vector2(tileSize * Mathf.Round(localPosition.x / tileSize), tileSize * Mathf.Round(localPosition.y / tileSize));
				const float eps = 0.1f;
				var newWay = Direction.Undefined;
				//current position is close to the bottom left corner of the tile?
				if ((Mathf.Abs(localPosition.x - currentTilePosition.x) < eps) && (Mathf.Abs(localPosition.y - currentTilePosition.y) < eps) )
				{
					//Change direction with 10% probability
					if(Random.value > 0.9)
					{
						var randomWay = (Direction)Mathf.Pow(2, Random.Range(0, 4));
						var block = MapDiscovery.BlastInDirection(transform.position, tileSize, randomWay, 1);
						//do we check against bomb here as well?
						//If the way is blocked we might sit here for a while until proper direction is randomed?
						if (block.Select(o => o.transform.gameObject).All(o => (o.tag != "Wall")))
							newWay = randomWay;   
					}                 
				}
				//Only if we change direction
				if(newWay != Direction.Undefined && newWay != _way)
				{
					if(_way == Direction.Left)
						Animator.SetFloat("Direction", 1);
					if(_way == Direction.Right)
						Animator.SetFloat("Direction", -1);
					_way = newWay;
				}
				//Velocity needs to be updated each frame, Who changes it? : Alexey
				//Physics is a bitch : Igor
				rigidbody2D.velocity = MaxSpeed * _way.ToVector2();
			}
			else rigidbody2D.velocity = new Vector2();
		}

    }
}
