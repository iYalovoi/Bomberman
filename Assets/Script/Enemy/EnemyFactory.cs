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

		private EnemiesStats enemiesStats;

		void Awake()
		{
			enemiesStats = new EnemiesStats();
		}

		public GameObject Produce(EnemyTypes enemyType)
		{           
			var enemyInstance = Instantiate(Prefab) as GameObject;

			var animationSkin = enemyInstance.GetComponent<ReSkinAnimation>();
			animationSkin.enemyType = enemyType;

			var enemyStats = enemiesStats.GetStats(enemyType);
			var enemyBehaviour = enemyInstance.GetComponent<Enemy>();
			enemyBehaviour.MaxSpeed = enemyStats.Speed;

			if (enemyStats.isGhost) 
			{
				enemyInstance.layer = LayerMask.NameToLayer("Ghost");
			}

			return enemyInstance;
		}
	}
}
