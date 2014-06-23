using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Onil : Enemy
    {
        public Onil()
        {
            MaxSpeed = 1f;
        }

		private Direction way;

        private void FixedUpdate()
        {
			if(!Dead)
			{
				//Kind of weird way to determine tile size?
				var tileSize = renderer.bounds.size.x;
				//Local position seems to be bottom left corner?
				var localPosition = gameObject.transform.localPosition;
				//
				var currentTilePosition = new Vector2(tileSize * Mathf.Round(localPosition.x / tileSize), tileSize * Mathf.Round(localPosition.y / tileSize));
				const float eps = 0.1f;
				Direction newWay = Direction.Undefined;
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
				if(newWay != Direction.Undefined && newWay != way)
				{
					if(way == Direction.Left)
					{
						animator.SetFloat("Direction", 1);
					}
					if(way == Direction.Right)
					{
						animator.SetFloat("Direction", -1);
					}
					way = newWay;
				}
				//Velocity needs to be updated each frame, Who changes it?
				rigidbody2D.velocity = MaxSpeed * way.ToVector2();
			}
			else rigidbody2D.velocity = new Vector2();
        }

    }
}
