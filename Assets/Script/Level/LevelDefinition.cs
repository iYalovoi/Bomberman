using UnityEngine;
using System.Collections;
using Assets.Script;
using System.Collections.Generic;

namespace Assets.Script.Level
{

	//ToDo - Add validation of data(ie softblock count < map size etc)
	public class LevelDefinition 
	{
		public LevelDefinition(Powers? powerup, EnemyCounts enemyCounts)
		{
			this.powerUp = powerup;
			this.enemyCounts = enemyCounts;
		}

		public int Height = 13;
		public int Width = 31;

		public int SoftBlocksCount = 70;

		public EnemyCounts enemyCounts;
		public Powers? powerUp;

		private T PopRandom<T>(List<T> source)
		{
			var index = Random.Range (0, source.Count);
			T result = source[index];
			source.RemoveAt(index);
			return result;
		}

		public LevelPosition[,] GenerateMap()
		{
			var map = new LevelPosition[Width, Height];
			var freePositions = new List<LevelPosition> ();
			var softBlocks = new List<LevelPosition> ();
			//Walls and hard blocks
			for (var i = 0; i < Width; i++) 
			{
				for (var j = 0; j < Height; j++) 
				{
					var levelPosition = new LevelPosition();
					map[i, j] = levelPosition;
					var isWall = (i == 0) || (i == Width - 1) || (j == 0) || (j == Height - 1);
					if(isWall)
						levelPosition.BlockType = BlockTypes.Wall;
					else
					{
						var isHardBlock = ((i % 2) == 0) && ((j % 2) == 0);
						levelPosition.BlockType = isHardBlock?BlockTypes.Hard:BlockTypes.None;
					}
					//Exclude several reserved positions
					var isReserved = (i == 1 && j == 1) || (i == 2 && j == 1) || (i == 1 && j == 2);
					if(!isReserved && levelPosition.BlockType == BlockTypes.None)
						freePositions.Add(levelPosition);
				}
			}
			//Player
			map[1, 1].Player = true;
			//Soft blocks
			for (var i = 0; i < SoftBlocksCount; i++) 
			{
				var levelPosition = PopRandom(freePositions);
				levelPosition.BlockType = BlockTypes.Soft;
				softBlocks.Add(levelPosition);
			}
			//Powerup
			if(powerUp.HasValue)
			{
				PopRandom(softBlocks).PowerUp = powerUp;
			}
			//Door
			PopRandom(softBlocks).Door = true;
			//Enemies
			foreach(var enemyCount in enemyCounts)
			{
				for(var i = 0;i < enemyCount.Value;i++)
				{
					PopRandom(freePositions).Enemy = enemyCount.Key;
				}
			}
			return map;
		}
	}

	public class EnemyCounts:Dictionary<EnemyTypes, uint> 
	{

	}
		
	public class LevelPosition
	{
		public LevelPosition()
		{
		}

		public BlockTypes BlockType = BlockTypes.None;
		public EnemyTypes? Enemy = null;
		public Powers? PowerUp = null;
		public bool Door = false;
		public bool Player = false;
	}
}

