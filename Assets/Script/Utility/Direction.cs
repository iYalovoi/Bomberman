using System;
using UnityEngine;

namespace Assets.Script.Utility
{
    [Flags]
    public enum Direction
    {
        Undefined = 0,
        Left = 1 << 0,
        Up = 1 << 1,
        Right = 1 << 2,
        Down = 1 << 3,

        Vertical = Up | Down,
        Horizontal = Left | Right,
    }

	public static class DirectionExtensions
	{
		public static Vector2 ToVector2(this Direction direction)
		{
			var result = new Vector2
			{
			    x = (direction.IsFlagSet(Direction.Right) ? 1 : 0) - (direction.IsFlagSet(Direction.Left) ? 1 : 0),
			    y = (direction.IsFlagSet(Direction.Up) ? 1 : 0) - (direction.IsFlagSet(Direction.Down) ? 1 : 0)
			};
		    return result;
		}

		public static Direction FromVector2(Vector2 vector)
		{
			Direction result = Direction.Undefined;
			if (Mathf.Abs (vector.x) > Mathf.Abs (vector.y)) 
			{
				if(vector.x < 0)
					result = Direction.Left;
				if(vector.x > 0)
					result = Direction.Right;
			}
			else 
			{
				if(vector.y < 0)
					result = Direction.Down;
				if(vector.y > 0)
					result = Direction.Up;
			}
			return result;
		}

	}
}