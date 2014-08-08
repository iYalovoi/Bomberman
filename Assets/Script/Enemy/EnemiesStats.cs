using System;
using System.Collections.Generic;

namespace Assets.Script.Level
{
	public class EnemiesStats
	{
		public EnemiesStats()
		{
			_statsDictionary = new Dictionary<EnemyTypes, EnemyStats>();
			FillEnemyStats();
		}

		public EnemyStats GetStats(EnemyTypes enemyType)
		{
			return _statsDictionary[enemyType]; 
		}
		
		private readonly Dictionary<EnemyTypes, EnemyStats> _statsDictionary;

		private void FillEnemyStats()
		{
			_statsDictionary[EnemyTypes.Balloon] = new EnemyStats(100, Constants.SlowSpeed, EnemyIntelligence.Low, false);
			_statsDictionary[EnemyTypes.Onil] = new EnemyStats(200, Constants.MidSpeed,  EnemyIntelligence.Average, false);
			_statsDictionary[EnemyTypes.Dahl] = new EnemyStats(400, Constants.MidSpeed, EnemyIntelligence.Low, false);
			_statsDictionary[EnemyTypes.Minvo] = new EnemyStats(800, Constants.FastSpeed, EnemyIntelligence.Average, false);
			_statsDictionary[EnemyTypes.Doria] = new EnemyStats(1000, Constants.SlowestSpeed, EnemyIntelligence.High, true);
			_statsDictionary[EnemyTypes.Ovape] = new EnemyStats(2000, Constants.SlowSpeed, EnemyIntelligence.Average, true);
			_statsDictionary[EnemyTypes.Pass] = new EnemyStats(4000, Constants.FastSpeed, EnemyIntelligence.High, false);
			_statsDictionary[EnemyTypes.Pontan] = new EnemyStats(8000, Constants.FastSpeed, EnemyIntelligence.High, true);
		}

	}

	public class EnemyStats
	{
		public EnemyStats(int bounty, float speed, EnemyIntelligence intelligence, bool isGhost)
		{
			Bounty  = bounty;
			Intelligence = intelligence;
			Speed = speed;
			IsGhost = isGhost;
		}
		
		//This has to be some engine independent value
		public float Speed;
		public EnemyIntelligence Intelligence;
		public int Bounty;
		//Can pass through soft blocks
		public bool IsGhost;
	}
}

