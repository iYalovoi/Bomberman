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
		    enemyInstance.name = enemyType.ToString();

			var animationSkin = enemyInstance.GetComponent<ReSkinAnimation>();
			animationSkin.enemyType = enemyType;

			var enemyStats = _enemiesStats.GetStats(enemyType);

			var enemy = enemyInstance.GetComponent<Enemy>();
			enemy.MaxSpeed = enemyStats.Speed;
			enemy.Bounty = enemyStats.Bounty;
			switch (enemyType) 
			{
				case EnemyTypes.Onil:
					enemy.Behaviour = new CompositeBehaviour(new ChasingPlayer(2f), new RandomRoaming());
					break;
				case EnemyTypes.Minvo:
					enemy.Behaviour = new CompositeBehaviour(new DodgingBombs(2f), new RandomRoaming());
					break;
			}

			if (enemyStats.IsGhost) 
				enemyInstance.layer = LayerMask.NameToLayer("Ghost");

			return enemyInstance;
		}
	}
}
