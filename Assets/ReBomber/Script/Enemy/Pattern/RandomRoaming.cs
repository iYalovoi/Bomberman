using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class RandomRoaming:IEnemyPattern
    {
        float turnProbability;

        public RandomRoaming(float turnProbability = 0.1f)
        {
            this.turnProbability = turnProbability;
			target = null;
		}

		private Vector2? target;

		public void Reset()
		{
			target = null;
		}


		private Vector2 ChooseRandomTarget(GameObject gameObject)
		{
			Vector2 result = Vector2.zero;
			var tileIndex = MapDiscovery.GetTileIndex(gameObject, gameObject.transform.localPosition);
			var tileSize = MapDiscovery.GetTileSize(gameObject);
			//Need to find new target look left, right, top, bottom
			var tiles = new[]{tileIndex - Vector2.right, tileIndex - Vector2.up ,
				tileIndex + Vector2.right, tileIndex + Vector2.up};
			var tilePositions = tiles.Select(x=>gameObject.transform.parent.TransformPoint(tileSize * x));
			//Filter out unreachable positions
			var validTargets = tilePositions.Where(x=>MapDiscovery.CanReach(gameObject, x)).ToArray();
			if (validTargets.Length != 0) 
			{
				//Choose random way to follow
				result = validTargets [Random.Range (0, validTargets.Length)];
			}
			else 
			{
				//Stuck, move towards center of current tile
				result = gameObject.transform.parent.TransformPoint(tileSize * tileIndex);
			}
			return result;
		}

		private Vector2 FollowPath(GameObject gameObject)
		{
			var result = Vector2.zero;
			var tileIndex = MapDiscovery.GetTileIndex(gameObject, gameObject.transform.localPosition);
			var tileSize = MapDiscovery.GetTileSize(gameObject);
			var velocity = gameObject.rigidbody2D.velocity;
			if (velocity == Vector2.zero)
				result = ChooseRandomTarget (gameObject);
			else 
			{
				var targetTile = Vector2.zero;
				var oppositeTile = Vector2.zero;
				if(Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
				{
					targetTile = tileIndex + Mathf.Sign(velocity.x) * Vector2.right;
					oppositeTile = tileIndex - Mathf.Sign(velocity.x) * Vector2.right;
				}
				else
				{
					targetTile = tileIndex + Mathf.Sign(velocity.y) * Vector2.up;
					oppositeTile = tileIndex - Mathf.Sign(velocity.y) * Vector2.up;
				}
				var targetTilePosition = gameObject.transform.parent.TransformPoint(tileSize * targetTile);
				if(MapDiscovery.CanReach(gameObject, targetTilePosition))
				{
					result = targetTilePosition;
				}
				else
				{
					var oppositeTilePosition = gameObject.transform.parent.TransformPoint(tileSize * oppositeTile);
					if(MapDiscovery.CanReach(gameObject, oppositeTilePosition))
					{
						result = oppositeTilePosition;
					}
					else
					{
						result = ChooseRandomTarget (gameObject);
					}
				}
			}

			return result;
		}
		
		public Vector2 FindWay(GameObject gameObject)
		{
			Vector2 position = gameObject.transform.position;
			var tileIndex = MapDiscovery.GetTileIndex(gameObject, gameObject.transform.localPosition);
			var tileSize = MapDiscovery.GetTileSize(gameObject);
			var newWay = default(Vector2);
			if (target != null) 
			{
				//Check if we arrived close to target
				const float eps = 0.021f;
				if(Vector2.Distance(target.Value, position) < eps)
				{
					//Arrived to the tile
					if(Random.value < turnProbability)
					{					
						//With certain probability random new direction
						target = ChooseRandomTarget(gameObject);
						newWay = target.Value - position;
					}
					else
					{
						//Try to follow current direction
						target = FollowPath(gameObject);
						newWay = target.Value - position;
					}
				}
				else
				{
					//Check if target is still reachable, ie user could have placed bomb
					if(MapDiscovery.CanReach(gameObject, target.Value))
					{
						//Continue move towards current target
						newWay = target.Value - position;
					}
					else
					{
						//Move to the center of the current tile
						target = gameObject.transform.parent.TransformPoint(tileSize * tileIndex);
						newWay = target.Value - position;
					}
				}
			} 
			else 
			{
				target = FollowPath(gameObject);
				newWay = target.Value - position;
			}
			Debug.DrawRay(position, target.Value - position, Color.green);

			//TODO - instead of this check there should be proper movement to center for the stuck tile
			if(newWay.magnitude < 0.005f)
				newWay = Vector2.zero; 
			else
		    	newWay.Normalize ();
			return newWay;
		}

    }
}

