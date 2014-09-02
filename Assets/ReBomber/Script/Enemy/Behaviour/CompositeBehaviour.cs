using System;
using System.Collections.Generic;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
	public class CompositeBehaviour:IEnemyBehaviour
	{
		public CompositeBehaviour(params IEnemyBehaviour[] behaviours)
		{
			_behaviours = new List<IEnemyBehaviour>(behaviours);
		}

		public void Append(IEnemyBehaviour behaviour)
		{
			_behaviours.Add(behaviour);
		}


		private List<IEnemyBehaviour> _behaviours;

		public Direction FindWay(GameObject gameObject)
		{
			Direction result = Direction.Undefined;
			for (var i = 0; i < (_behaviours.Count) && (result == Direction.Undefined); i++) 
			{
				result = _behaviours[i].FindWay(gameObject);
			}
			return result;
		}
	}
}

