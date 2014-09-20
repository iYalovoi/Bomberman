using System;
using System.Collections.Generic;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class CompositePattern:IEnemyPattern
    {
        public CompositePattern(params IEnemyPattern[] behaviours)
        {
            _behaviours = new List<IEnemyPattern>(behaviours);
        }

        public void Append(IEnemyPattern behaviour)
        {
            _behaviours.Add(behaviour);
        }

        private List<IEnemyPattern> _behaviours;

        public Vector2 FindWay(GameObject gameObject)
        {
            var result = default(Vector2);
            for (var i = 0; i < (_behaviours.Count) && (result == default(Vector2)); i++)
            {
                result = result + _behaviours[i].FindWay(gameObject);
                result.Normalize();
            }
            return result;
        }
    }
}

