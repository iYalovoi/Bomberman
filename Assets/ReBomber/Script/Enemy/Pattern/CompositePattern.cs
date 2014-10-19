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
		private IEnemyPattern lastPattern;

		public void Reset()
		{
			_behaviours.ForEach(x=>x.Reset());
		}

        public Vector2 FindWay(GameObject gameObject)
        {
            var result = default(Vector2);
			IEnemyPattern pattern = null;
            for (var i = 0; (i < _behaviours.Count) && (pattern == null); i++)
            {
                result = _behaviours[i].FindWay(gameObject);
				if(result != default(Vector2))
				{
					pattern = _behaviours[i];
				}
            }
			if (pattern != lastPattern) 
			{
				if(lastPattern != null)
					lastPattern.Reset();
				lastPattern = pattern;
			}
            return result;
        }
    }
}

