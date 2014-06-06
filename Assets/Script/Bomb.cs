using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script
{
    public class Bomb : MonoBehaviour
    {
        public int Radius;
        public GameObject Blast;
        public GameObject Level;

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var colider = gameObject.GetComponent<CircleCollider2D>();
                colider.isTrigger = false;
            }
        }

        void Start()
        {
            if (Level != null)
                StartCoroutine(Explode(3));
        }

        private IEnumerator Explode(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            var tileSize = renderer.bounds.size.x;
            var localPosition = gameObject.transform.localPosition;
            var bombTile = new Vector2(localPosition.x / tileSize, localPosition.y / tileSize);

            var position = transform.position;

            //left
            BlastInDirection(position, tileSize, bombTile, 0);

            //up            
            BlastInDirection(position, tileSize, bombTile, 1);

            //right
            BlastInDirection(position, tileSize, bombTile, 2);

            //down
            BlastInDirection(position, tileSize, bombTile, 3);

            var animator = GetComponent<Animator>();
            animator.SetTrigger("Explode");
        }


        /// <summary>
        /// Fucking blast smashing everything on it's own way!
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tileSize"></param>
        /// <param name="bombTile"></param>
        /// <param name="direction">0 - left, 1 - up, 2 - right, 3 - down</param>
        public void BlastInDirection(Vector3 position, float tileSize, Vector2 bombTile, int direction)
        {
            var isVertical = direction == 1 || direction == 3;
            var isLeft = direction == 0;
            var isUp = direction == 1;
            var halfTile = tileSize/2;
            var radiusLine = Radius*tileSize;
            var xDelta = !isVertical ? halfTile * (isLeft ? -1 : 1) : 0;
            var yDelta = isVertical ? halfTile * (!isUp ? - 1: 1) : 0;
            var launch = new Vector2(position.x + xDelta, position.y + yDelta);
            var xRadius = !isVertical ? radiusLine * (isLeft ? -1 : 1) : 0;
            var yRadius = isVertical ? radiusLine * (!isUp ? -1 : 1) : 0;
            var hit = new Vector2(position.x + xDelta + xRadius, position.y + yDelta + yRadius);

            var hits = Physics2D.LinecastAll(launch, hit);
            Debug.DrawLine(launch, hit, Color.green, 1, false);

            var newRadius = Radius;

            var objects = hits.Select(o => o.transform.gameObject).ToList();
            var wall = objects.FirstOrDefault(o => o.tag == "Wall");
            if (wall != null)
            {
                var delta = wall.transform.position - position;
                newRadius = Mathf.Abs(Mathf.RoundToInt((isVertical ? delta.y : delta.x) / tileSize)) - 1;
            }
            Debug.Log(newRadius);
            var bombTileX = Mathf.RoundToInt(bombTile.x);
            var bombTileY = Mathf.RoundToInt(bombTile.y);
            for (var i = 0; i < newRadius; i++)
            {
                var xTile = bombTileX + (!isVertical ? (1 + i) * (isLeft ? -1 : 1) : 0);
                var yTile = bombTileY + (isVertical ? (1 + i) * (!isUp ?  - 1 : 1) : 0);
                var blast = Instantiate(Blast, new Vector3(), new Quaternion(0, 0, 0, 0)) as GameObject;
                blast.transform.parent = Level.transform;
                blast.transform.localPosition = new Vector3(xTile * tileSize, yTile * tileSize);
                if(direction == 1 || direction == 3)
                    blast.transform.Rotate(0, 0, 90);
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        void Update()
        {

        }
    }
}
