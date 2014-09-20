using System;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public interface IEnemyPattern
    {
        Vector2 FindWay(GameObject gameObject);
    }
}

