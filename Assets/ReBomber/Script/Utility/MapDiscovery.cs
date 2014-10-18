using UnityEngine;

namespace Assets.Script.Utility
{
    public class MapDiscovery
    {
        public static RaycastHit2D[] BlastInDirection(Vector3 position, float tileSize, Direction direction, float radius)
        {
            var isVertical = Direction.Vertical.IsFlagSet(direction);
            var isLeft = direction == Direction.Left;
            var isUp = direction == Direction.Up;
            var halfTile = tileSize / 2;
            var radiusLine = (radius - 1) * tileSize + (tileSize / 2);
            var xDelta = !isVertical ? halfTile * (isLeft ? -1 : 1) : 0;
            var yDelta = isVertical ? halfTile * (!isUp ? -1 : 1) : 0;
            var launch = new Vector2(position.x + xDelta, position.y + yDelta);
            var xRadius = !isVertical ? radiusLine * (isLeft ? -1 : 1) : 0;
            var yRadius = isVertical ? radiusLine * (!isUp ? -1 : 1) : 0;
            var hit = new Vector2(position.x + xDelta + xRadius, position.y + yDelta + yRadius);

            var hits = Physics2D.LinecastAll(launch, hit);
            Debug.DrawLine(launch, hit, Color.green, 1, false);
            return hits;
        }

        public static float GetTileSize(GameObject gameObject)
        {
            return gameObject.renderer.bounds.size.x;
        }

        public static Vector2 GetTilePosition(GameObject gameObject, Vector3 position)
        {
            //Kind of weird way to determine tile size? : Aleksey
            //Yes, Indeed : Igor
            var tileSize = GetTileSize(gameObject);
            return new Vector2(tileSize * Mathf.Round(position.x / tileSize), tileSize * Mathf.Round(position.y / tileSize));
        }

		public static Vector2 GetTileIndex(GameObject gameObject, Vector3 position)
		{
			//Kind of weird way to determine tile size? : Aleksey
			//Yes, Indeed : Igor
			var tileSize = GetTileSize(gameObject);
			return new Vector2(Mathf.Round(position.x / tileSize), Mathf.Round(position.y / tileSize));
		}
    }
}
