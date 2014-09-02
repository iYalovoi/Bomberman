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
			_statsDictionary[EnemyTypes.Balloon] = new EnemyStats{Bounty = 100, Speed = Constants.LowSpeed};
			_statsDictionary[EnemyTypes.Onil] = new EnemyStats{Bounty = 200, Speed = Constants.AverageSpeed, Uncertainty = 0.2f, PlayerVisionRange = 1.0f};
			_statsDictionary[EnemyTypes.Dahl] = new EnemyStats{Bounty = 400, Speed = Constants.AboveAverageSpeed, Uncertainty = 0.2f, PlayerVisionRange = 1.5f};
			_statsDictionary[EnemyTypes.Minvo] = new EnemyStats{Bounty = 800, Speed = Constants.HighSpeed, Uncertainty = 0.2f, PlayerVisionRange = 2.0f, BombVisionRange = 1.5f};
			_statsDictionary[EnemyTypes.Doria] = new EnemyStats{Bounty = 1000, Speed = Constants.LowestSpeed, IsGhost = true};
			_statsDictionary[EnemyTypes.Ovape] = new EnemyStats{Bounty = 2000, Speed = Constants.AverageSpeed, PlayerVisionRange = 1f, Uncertainty = 0.2f, IsGhost = true};
			_statsDictionary[EnemyTypes.Pass] = new EnemyStats{Bounty = 4000, Speed = Constants.HighSpeed, Uncertainty = 0.2f, PlayerVisionRange = 2.0f, BombVisionRange = 1.5f};
			_statsDictionary[EnemyTypes.Pontan] = new EnemyStats{Bounty = 8000, Speed = Constants.HighestSpeed, PlayerVisionRange = 4f, Uncertainty = 0.2f, IsGhost = true};
		}

	}

	public class EnemyStats
	{
		public EnemyStats()
		{
		}

		public EnemyStats(int bounty, float speed,  bool isGhost)
		{
			Bounty  = bounty;
			Speed = speed;
			IsGhost = isGhost;
			VisionRange = visionRange;
		}
		
		//This has to be some engine independent value
		public float Speed = Constants.LowSpeed;
		//Score
		public int Bounty = 100;
		//Can pass through soft blocks
		public bool IsGhost = false;
		//Probability to change direction
		public float Uncertainty = 0.1f;
		//Vision range of enemy
		public float PlayerVisionRange = 0.0f;
		//Vision range of bomb
		public float BombVisionRange = 0.0f;
	}
}

