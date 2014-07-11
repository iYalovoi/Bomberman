using UnityEngine;
using System.Collections;
using Assets.Script.Level;

namespace Assets.Script
{
	public interface IEnemyFactory
	{
		GameObject Produce(EnemyTypes power);
	}

	public class EnemyFactory : MonoBehaviour 
	{
		public GameObject Prefab;

		public GameObject Produce(EnemyTypes enemyType)
		{           
			var retObj = Instantiate(Prefab) as GameObject;
			var animationSkin = retObj.GetComponent<ReSkinAnimation>();
			animationSkin.enemyType = enemyType;
			//Need somehow to set other enemy parameters, like speed and intellingence.
			//May be add them to level description or something.
			return retObj;
		}
	}
}
