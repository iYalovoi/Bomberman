using System;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
		public interface IEnemyBehaviour
		{
			Direction FindWay(GameObject gameObject);
		}
}

