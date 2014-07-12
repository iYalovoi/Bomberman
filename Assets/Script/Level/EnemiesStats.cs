using System;
using System.Collections.Generic;

namespace Assets.Script.Level
{
	public class EnemiesStats
	{
		public EnemiesStats()
		{
			statsDictionary = new Dictionary<EnemyTypes, EnemyStats>();
			FillEnemyStats();
		}

		public EnemyStats GetStats(EnemyTypes enemyType)
		{
			return statsDictionary[enemyType]; 
		}
		
		private Dictionary<EnemyTypes, EnemyStats> statsDictionary;

		private void FillEnemyStats()
		{
			statsDictionary[EnemyTypes.Balloon] = new EnemyStats(100, Constants.SlowSpeed, EnemyIntelligence.Low, false);
			statsDictionary[EnemyTypes.Onil] = new EnemyStats(200, Constants.MidSpeed,  EnemyIntelligence.Average, false);
			statsDictionary[EnemyTypes.Dahl] = new EnemyStats(400, Constants.MidSpeed, EnemyIntelligence.Low, false);
			statsDictionary[EnemyTypes.Minvo] = new EnemyStats(800, Constants.FastSpeed, EnemyIntelligence.Average, false);
			statsDictionary[EnemyTypes.Doria] = new EnemyStats(1000, Constants.SlowestSpeed, EnemyIntelligence.High, true);
			statsDictionary[EnemyTypes.Ovape] = new EnemyStats(2000, Constants.SlowSpeed, EnemyIntelligence.Average, true);
			statsDictionary[EnemyTypes.Pass] = new EnemyStats(4000, Constants.FastSpeed, EnemyIntelligence.High, false);
			statsDictionary[EnemyTypes.Pontan] = new EnemyStats(8000, Constants.FastSpeed, EnemyIntelligence.High, true);
		}

	}

	public class EnemyStats
	{
		public EnemyStats(int points, float speed, EnemyIntelligence intelligence, bool isGhost)
		{
			this.Points  = points;
			this.Intelligence = intelligence;
			this.Speed = speed;
			this.isGhost = isGhost;
		}
		
		//This has to be some engine independent value
		public float Speed;
		public EnemyIntelligence Intelligence;
		public int Points;
		//Can pass through soft blocks
		public bool isGhost;
	}
}

