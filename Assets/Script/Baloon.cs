using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Baloon : MonoBehaviour
    {

        public float MaxSpeed = 1f;
        public float Direction;
        public Direction Way;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            var tileSize = renderer.bounds.size.x;
            var localPosition = gameObject.transform.localPosition;
            var currentTile = new Vector2(Mathf.Round(localPosition.x / tileSize), Mathf.Round(localPosition.y / tileSize));
            const float eps = 0.1f;
            if ((Mathf.Abs(localPosition.x - currentTile.x * tileSize) < eps) && (Mathf.Abs(localPosition.y - currentTile.y * tileSize) < eps) && Random.value > 0.9)
            {
                var way = (Direction)Mathf.Pow(2, Random.Range(0, 4));
                var block = MapDiscovery.BlastInDirection(transform.position, tileSize, way, 1);
                if (block.Select(o => o.transform.gameObject).All(o => o.tag != "Wall"))
                    Way = way;                    
            }
            rigidbody2D.velocity = new Vector2(Utility.Direction.Horizontal.IsFlagSet(Way) ? Way == Utility.Direction.Left ? -MaxSpeed : MaxSpeed : 0,
                            Utility.Direction.Vertical.IsFlagSet(Way) ? Way == Utility.Direction.Up ? MaxSpeed : -MaxSpeed : 0);
        }
    }
}
