using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
		public class RandomRoaming:IEnemyBehaviour
		{
				float turnProbability;

				public RandomRoaming(float turnProbability = 0.1f)
				{
					this.turnProbability = turnProbability;
				}

				protected float GetTileSize(GameObject gameObject)
				{
					return gameObject.renderer.bounds.size.x;
				}
				
				protected Vector2 GetTilePosition(GameObject gameObject, Vector3 position)
				{
					//Kind of weird way to determine tile size? : Aleksey
					//Yes, Indeed : Igor
					var tileSize = GetTileSize(gameObject);
					return new Vector2 (tileSize * Mathf.Round (position.x / tileSize), tileSize * Mathf.Round (position.y / tileSize));
				}

				/*
				 * Should bounce of the bombs and walls into opposite direction and also randomly change direction
				 */
				public Direction FindWay(GameObject gameObject)
				{
					var tileSize = GetTileSize(gameObject);
					var localPosition = gameObject.transform.localPosition;
					var currentTilePosition = GetTilePosition(gameObject, localPosition);
					const float eps = 0.1f;
					var newWay = Direction.Undefined;			
					//current position is close to the bottom left corner of the tile?
					if ((Mathf.Abs(localPosition.x - currentTilePosition.x) < eps) && (Mathf.Abs(localPosition.y - currentTilePosition.y) < eps) )
					{
						//Change direction with probability
						if(Random.value > (1f - turnProbability))
						{
							var randomWay = (Direction)Mathf.Pow(2, Random.Range(0, 4));
							//Do we really need to do these checks?
							//var block = MapDiscovery.BlastInDirection(transform.position, tileSize, randomWay, 1);
							//do we check against bomb here as well?
							//If the way is blocked we might sit here for a while until proper direction is randomed?
							//if (block.Select(o => o.transform.gameObject).All(o => (o.tag != "Wall")))
								newWay = randomWay;   
						}                 
					}
					return newWay;
				}

		}
}

