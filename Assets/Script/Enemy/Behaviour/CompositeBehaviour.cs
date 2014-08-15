using System;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
	public class CompositeBehaviour:IEnemyBehaviour
	{
		public CompositeBehaviour(params IEnemyBehaviour[] behaviours)
		{
			_behaviours = behaviours;
		}

		private IEnemyBehaviour[] _behaviours;

		public Direction FindWay(GameObject gameObject)
		{
			Direction result = Direction.Undefined;
			for (var i =0; i < (_behaviours.Length) && (result == Direction.Undefined); i++) 
			{
				result = _behaviours[i].FindWay(gameObject);
			}
			return result;
		}
	}
}

