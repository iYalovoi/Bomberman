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
}