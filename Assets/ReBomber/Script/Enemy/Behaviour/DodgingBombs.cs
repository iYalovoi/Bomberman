using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
	public class DodgingBombs: IEnemyBehaviour
	{
		//vision range in tiles
		public DodgingBombs(float visionRange)
		{
			_visionRange = visionRange;
		}

		private float _visionRange;

		protected float GetTileSize(GameObject gameObject)
		{
			return gameObject.renderer.bounds.size.x;
		}

		public Direction FindWay(GameObject gameObject)
		{
			var newWay = Direction.Undefined;
			//Set vision range to 2 tile between enemy and player centers
			var visionRange = GetTileSize(gameObject) * (1 + _visionRange);
			var enemyPosition = gameObject.transform.position;

			//Find closest visible bomb
			var bombs = GameObject.FindGameObjectsWithTag("Bomb");
			GameObject closestVisibleBomb = null;
			var minSqrMagnitude = Mathf.Infinity; 

			//Layer mask for vision
			var layerMask = LayerMask.GetMask("Enemy", "Ghost", "Hit", "Player");
			if(gameObject.layer == LayerMask.NameToLayer("Ghost"))
			{
				layerMask |= LayerMask.GetMask("Soft");
			}
			layerMask = ~layerMask;
			foreach (var bomb in bombs) 
			{
				var sqrMagnitude = Vector2.SqrMagnitude(bomb.transform.position - enemyPosition);
				if(sqrMagnitude < visionRange)
				{
					if(sqrMagnitude < minSqrMagnitude)
					{
						//Check if we actually see the bomb
						var hit = Physics2D.Linecast(enemyPosition, bomb.transform.position, layerMask);
						if(hit.collider && hit.collider.gameObject == bomb)
						{
							closestVisibleBomb = bomb;
							minSqrMagnitude = sqrMagnitude;
						}
					}
				}
			}
			if (closestVisibleBomb) 
			{
				var bombPosition = closestVisibleBomb.transform.position;
				var bombSize = closestVisibleBomb.renderer.bounds.size;		
				Debug.DrawLine(enemyPosition, bombPosition, Color.green, 1, false);
				//Move away from the bomb
				//Two possibilities - vertical or horizontal
				//Prioritize shortest one to try change course from direct line
				var direction1 = new Vector2(enemyPosition.x - bombPosition.x , 0);
				var direction2 = new Vector2(0, enemyPosition.y - bombPosition.y);
				if(Mathf.Abs(direction1.x) > Mathf.Abs(direction2.y) && Mathf.Abs(direction2.y) != 0)
				{
					var tmp = direction1;
					direction1 = direction2;
					direction2 = tmp;
				}
				//Check if we can move in direction 1
				//Which distance to chose here?
				var hit = Physics2D.Raycast(enemyPosition, direction1.normalized, bombSize.x * 0.5f + 0.1f, layerMask);
				if(hit.collider && hit.collider.tag != "Bomb")
				{
					direction1 = direction2;
				}	
				Debug.DrawRay(enemyPosition, direction1, Color.blue, 1, false);
				newWay = DirectionExtensions.FromVector2(direction1);
			}
			return newWay;
		}
	}
}

