using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Bomb : MonoBehaviour, ITarget
    {
        public int Radius;
        public GameObject Blast;
        public GameObject BlastEnd;
        public GameObject Level;
        public bool IsExploded;
        public bool IsSpawned;
        public bool RemoteControl;
        public CircleCollider2D Bomberman;
        public CircleCollider2D Trigger;
        public CircleCollider2D Solid;
        private AudioSource _explosionSound;        

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
                Physics2D.IgnoreCollision(Bomberman, Solid, false);
        }

        void Start()
        {
            Physics2D.IgnoreCollision(Bomberman, Solid, true);
            _explosionSound = gameObject.GetComponent<AudioSource>();
            if (Level != null && !RemoteControl)
                StartCoroutine(Explode(3));
        }

        private IEnumerator Explode(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            StartCoroutine(ExplodeZero());
        }

        void Spawned()
        {
            IsSpawned = true;
        }

        public IEnumerator ExplodeZero()
        {
            if (!IsExploded)
            {
                IsExploded = true;
                while (!IsSpawned)
                    yield return null;                    

                var animator = GetComponent<Animator>();

                var tileSize = renderer.bounds.size.x;
                var localPosition = gameObject.transform.localPosition;
                var bombTile = new Vector2(localPosition.x / tileSize, localPosition.y / tileSize);

                var position = transform.position;

                BlastInDirection(position, tileSize, bombTile, Direction.Left, Radius);
                BlastInDirection(position, tileSize, bombTile, Direction.Up, Radius);
                BlastInDirection(position, tileSize, bombTile, Direction.Right, Radius);
                BlastInDirection(position, tileSize, bombTile, Direction.Down, Radius);
                
                animator.SetTrigger("Explode");
                _explosionSound.Play();
            }
        }

        /// <summary>
        /// Fucking blast smashing everything on it's own way!
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tileSize"></param>
        /// <param name="bombTile"></param>
        /// <param name="direction">0 - left, 1 - up, 2 - right, 3 - down</param>
        /// <param name="radius"></param>
        public void BlastInDirection(Vector3 position, float tileSize, Vector2 bombTile, Direction direction, float radius)
        {
            var isVertical = Direction.Vertical.IsFlagSet(direction);
            var isLeft = direction == Direction.Left;
            var isUp = direction == Direction.Up;
            var hits = MapDiscovery.BlastInDirection(position, tileSize, direction, radius);

            var newRadius = Radius;

            var objects = hits.Select(o => o.transform.gameObject).ToList();
            var wall = objects.FirstOrDefault(o => o.tag == "Wall");
            if (wall != null)
            {
                var delta = wall.transform.position - position;
                newRadius = Mathf.Abs(Mathf.RoundToInt((isVertical ? delta.y : delta.x) / tileSize)) - 1;
                var soft = wall.GetComponent<Soft>();
                if(soft != null)
                    soft.Explode();
            }
            var bombTileX = Mathf.RoundToInt(bombTile.x);
            var bombTileY = Mathf.RoundToInt(bombTile.y);
            for (var i = 0; i < newRadius; i++)
            {
                var xTile = bombTileX + (!isVertical ? (1 + i) * (isLeft ? -1 : 1) : 0);
                var yTile = bombTileY + (isVertical ? (1 + i) * (!isUp ? -1 : 1) : 0);
                var blast = Instantiate((i < (newRadius -1)) ? Blast : BlastEnd, new Vector3(), new Quaternion(0, 0, 0, 0)) as GameObject;
                blast.transform.parent = Level.transform;
                blast.transform.localPosition = new Vector3(xTile * tileSize, yTile * tileSize);
                if (isVertical)
                    blast.transform.Rotate(0, 0, !isUp ? -90 : 90);
                if(!isLeft)
                    blast.transform.Rotate(0, 0, 180);
            }
            var beforeTheWall = objects.TakeWhile(o => o.tag != "Wall").ToList();
            if (beforeTheWall.Any())
                beforeTheWall.Select(o => o.GetInterface<ITarget>()).ToList().ForEach(o => o.OnHit(gameObject));
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        void Update()
        {
        }

        public void OnHit(GameObject striker)
        {
            if (gameObject != striker)
                StartCoroutine(ExplodeZero());
        }
    }
}
