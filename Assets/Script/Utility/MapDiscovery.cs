using UnityEngine;

namespace Assets.Script.Utility
{
    public class MapDiscovery
    {
        public void BlastInDirection(Vector3 position, float tileSize, Vector2 currentTile, Direction direction, float radius)
        {
            //var isVertical = direction == Direction.Down || direction == Direction.Up;
            //var isLeft = direction == 0;
            //var isUp = direction == 1;
            //var halfTile = tileSize / 2;
            //var radiusLine = radius * tileSize;
            //var xDelta = !isVertical ? halfTile * (isLeft ? -1 : 1) : 0;
            //var yDelta = isVertical ? halfTile * (!isUp ? -1 : 1) : 0;
            //var launch = new Vector2(position.x + xDelta, position.y + yDelta);
            //var xRadius = !isVertical ? radiusLine * (isLeft ? -1 : 1) : 0;
            //var yRadius = isVertical ? radiusLine * (!isUp ? -1 : 1) : 0;
            //var hit = new Vector2(position.x + xDelta + xRadius, position.y + yDelta + yRadius);

            //var hits = Physics2D.LinecastAll(launch, hit);
        }
    }
}
