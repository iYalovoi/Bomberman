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

        public Vector2 FindWay(GameObject gameObject)
        {
            var newWay = default(Vector2);
            //Set vision range to 2 tile between enemy and player centers
            var visionRange = MapDiscovery.GetTileSize(gameObject) * (1 + _visionRange);
            var enemyPosition = gameObject.transform.position;
            var enemySize = gameObject.renderer.bounds.size;		
            var player = GameObject.FindWithTag("Player");
            var playerPosition = player.gameObject.transform.position;
            //assume circle collider
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
                    //Enemy spoted player
                    Debug.DrawLine(enemyPosition, playerPosition, Color.green, 1, false);
                    //Move towards player
                    //Two possibilities - vertical or horizontal
                    //Prioritize longest one
                    var direction1 = new Vector2(playerPosition.x - enemyPosition.x, 0);
                    var direction2 = new Vector2(0, playerPosition.y - enemyPosition.y);
                    if (Mathf.Abs(direction1.x) < Mathf.Abs(direction2.y))
                    {
                        var tmp = direction1;
                        direction1 = direction2;
                        direction2 = tmp;
                    }
                    //Check if we can move in direction 1
                    //Which distance to chose here?
                    hit = Physics2D.Raycast(enemyPosition, direction1.normalized, enemySize.x * 0.5f + 0.1f, layerMask);
                    if (hit.collider && hit.collider.tag != "Player")
                        direction1 = direction2;
                    Debug.DrawRay(enemyPosition, direction1, Color.blue, 1, false);
                    newWay = direction1.normalized;
                }
                else
                {
                    Debug.DrawLine(enemyPosition, playerPosition, Color.red, 1, false);
                }
            }
            return newWay;
        }
    }
}