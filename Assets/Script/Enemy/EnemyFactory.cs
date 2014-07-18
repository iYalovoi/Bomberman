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

		private EnemiesStats _enemiesStats;

		void Awake()
		{
			_enemiesStats = new EnemiesStats();
		}

		public GameObject Produce(EnemyTypes enemyType)
		{           
			var enemyInstance = Instantiate(Prefab) as GameObject;

			var animationSkin = enemyInstance.GetComponent<ReSkinAnimation>();
			animationSkin.enemyType = enemyType;

			var enemyStats = _enemiesStats.GetStats(enemyType);
			var enemyBehaviour = enemyInstance.GetComponent<Enemy>();
			enemyBehaviour.MaxSpeed = enemyStats.Speed;

			if (enemyStats.IsGhost) 
				enemyInstance.layer = LayerMask.NameToLayer("Ghost");

			return enemyInstance;
		}
	}
}
