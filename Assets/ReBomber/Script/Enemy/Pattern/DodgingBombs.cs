using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class DodgingBombs: IEnemyPattern
    {
        //vision range in tiles
        public DodgingBombs(float visionRange)
        {
            _visionRange = visionRange;
        }

		public void Reset()
		{
		}

        private float _visionRange;

        public Vector2 FindWay(GameObject gameObject)
        {
            var newWay = default(Vector2);
            //Set vision range to 2 tile between enemy and player centers
            var visionRange = MapDiscovery.GetTileSize(gameObject) * (1 + _visionRange);
            Vector2 enemyPosition = gameObject.transform.position;

            //Find closest visible bomb
            var bombs = GameObject.FindGameObjectsWithTag("Bomb");
            GameObject closestVisibleBomb = null;
            var minMagnitude = Mathf.Infinity; 

            //Layer mask for vision
            var layerMask = LayerMask.GetMask("Enemy", "Ghost", "Hit", "Player");
            if (gameObject.layer == LayerMask.NameToLayer("Ghost"))
            {
                layerMask |= LayerMask.GetMask("Soft");
            }
            layerMask = ~layerMask;
            foreach (var bomb in bombs)
            {
				Vector2 bombPositon = bomb.transform.position;
				var magnitude = (bombPositon - enemyPosition).magnitude;
                if (magnitude < visionRange)
                {
					if (magnitude < minMagnitude)
                    {
                        //Check if we actually see the bomb
                        var hit = Physics2D.Linecast(enemyPosition, bomb.transform.position, layerMask);
                        if (hit.collider && hit.collider.gameObject == bomb)
                        {
                            closestVisibleBomb = bomb;
							minMagnitude = magnitude;
                        }
                    }
                }
            }
            if (closestVisibleBomb)
            {
				var bombPosition = closestVisibleBomb.transform.position;
				var tileIndex = MapDiscovery.GetTileIndex(gameObject, gameObject.transform.localPosition);
				var tilePosition = MapDiscovery.GetTileCenter(gameObject, tileIndex);
				var direction1 = new Vector2(enemyPosition.x - bombPosition.x, 0).normalized;
				var direction2 = new Vector2(0, enemyPosition.y - bombPosition.y).normalized;
				var target1 = tilePosition;
				var target2 = tilePosition;
				const float eps = 0.02f;
				if(direction1.x * (enemyPosition.x - tilePosition.x) > -eps)
				{
					target1 = MapDiscovery.GetTileCenter(gameObject, tileIndex + direction1.x * Vector2.right);
				}
				if(direction2.y * (enemyPosition.y - tilePosition.y) > -eps)
				{
					target2 = MapDiscovery.GetTileCenter(gameObject, tileIndex + direction2.y * Vector2.up);
				}
				//Make sure targets valid
				if(Mathf.Abs(enemyPosition.y - target1.y) > eps)
					target1 = tilePosition;
				if(Mathf.Abs(enemyPosition.x - target2.x) > eps)
					target2 = tilePosition;
				//Priority far from bomb
				if(Vector2.Distance(target1, bombPosition) < Vector2.Distance(target2, bombPosition))
				{
					var tmp = target1;
					target1 = target2;
					target2 = tmp;
				}
				if(MapDiscovery.CanReach(gameObject, target1))
				{
					newWay = target1 - enemyPosition;
				}
				else
				{
					if(MapDiscovery.CanReach(gameObject, target2))
						newWay = target2 - enemyPosition;
				}
				Debug.DrawRay(enemyPosition, newWay, Color.red);
				newWay.Normalize();
            }
            return newWay;
        }
    }
}

