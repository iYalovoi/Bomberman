using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class ChasingPlayer: IEnemyPattern
    {
        //vision range in tiles
        public ChasingPlayer(float visionRange)
        {
            _visionRange = visionRange;
        }

        private float _visionRange;

		public void Reset()
		{
		}

        public Vector2 FindWay(GameObject gameObject)
        {
            var newWay = default(Vector2);
            //Set vision range to 2 tile between enemy and player centers
            var visionRange = MapDiscovery.GetTileSize(gameObject) * (1 + _visionRange);
            Vector2 enemyPosition = gameObject.transform.position;
            var player = GameObject.FindWithTag("Player");
            var playerPosition = player.gameObject.transform.position;
			if (player.GetComponent<Bomberman> ().Dead)
				return newWay;
            //Check if close enough
            if (Vector2.Distance(enemyPosition, playerPosition) < visionRange)
            {
                //Very simple check for visibility
                //More smart version would be to do BoxCastAll and then find 
                //projectsions of objects onto normal to BoxCast direction and see if there is not covered enemy region
                var layerMask = LayerMask.GetMask("Enemy", "Ghost", "Hit");
                if (gameObject.layer == LayerMask.NameToLayer("Ghost"))
                {
                    layerMask |= LayerMask.GetMask("Soft");
                }
                layerMask = ~layerMask;
                var hit = Physics2D.Linecast(enemyPosition, playerPosition, layerMask);
                if (hit.collider && hit.collider.tag == "Player")
                {
                    //Move towards player
                    //Two possibilities - vertical or horizontal
                    //Prioritize longest one
					var tileIndex = MapDiscovery.GetTileIndex(gameObject, gameObject.transform.localPosition);
					var tilePosition = MapDiscovery.GetTileCenter(gameObject, tileIndex);
					var direction1 = new Vector2(playerPosition.x - enemyPosition.x, 0).normalized;
                    var direction2 = new Vector2(0, playerPosition.y - enemyPosition.y).normalized;
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
					//Priority closest to player
					if(Vector2.Distance(target1, playerPosition) > Vector2.Distance(target2, playerPosition))
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
					Debug.DrawRay(enemyPosition, newWay, Color.blue);
					newWay.Normalize();
                }
            }
            return newWay;
        }
    }
}