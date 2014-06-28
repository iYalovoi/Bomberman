using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Level;

namespace Assets.Script
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameObject Wall;
        public GameObject HardBlock;
        public GameObject Player;        
        public GameObject Soft;
        public GameObject Baloon;
		public GameObject Onil;
        public GameObject Door;
        public GameObject PowerUp;

        public GameObject Camera;
        public CameraFollow CameraFollow;
        public GameObject Level;

        private GameObject _currentPlayer;
		private Vector2 tileSize;

        void Start()
        {
            var enemies = new List<GameObject>();
            var softBlocks = new List<GameObject>();
            CameraFollow = Camera.GetComponent<CameraFollow>();

			tileSize = HardBlock.renderer.bounds.size;
            Level = new GameObject("Level");

			var levelDefinition = new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 6}, {EnemyTypes.Onil, 6}});
			var map = levelDefinition.GenerateMap();
			var blockMap = new Dictionary<BlockTypes, GameObject> (){{BlockTypes.Soft, Soft}, {BlockTypes.Hard, HardBlock}, {BlockTypes.Wall, Wall}};
			var enemyMap = new Dictionary<EnemyTypes, GameObject> (){{EnemyTypes.Balloon, Baloon}, {EnemyTypes.Onil, Onil}};
			for (var i = 0; i < levelDefinition.Width; i++) 
			{
				for (var j = 0; j < levelDefinition.Height; j++) 
				{
					var levelPosition = map[i, j];
					if(levelPosition.BlockType != BlockTypes.None)
						Create(blockMap[levelPosition.BlockType], i, j);
					if(levelPosition.Enemy.HasValue)
					{
						Create(enemyMap[levelPosition.Enemy.Value], i , j);
					}
					if(levelPosition.Door)
					{
						Create(Door, i, j);
					}
					if(levelPosition.PowerUp.HasValue)
					{
						//Powerup switch?
						Create(PowerUp, i, j);
					}
				}
			}

			SpawnPlayer();
			Level.transform.position = new Vector3(-tileSize.x * levelDefinition.Width / 2, -tileSize.y * levelDefinition.Height / 2);			
		}

		private GameObject Create(GameObject prototype, int x, int y)
		{
			var result = Instantiate(prototype, new Vector3(x * tileSize.x, y * tileSize.y, 0), new Quaternion()) as GameObject;
			result.name = string.Format("{2} {0}:{1}", x, y, prototype.transform.name);
			result.transform.parent = Level.transform;
			return result;
		}

        public void SpawnPlayer()
        {
			_currentPlayer = Create (Player, 1, 1);

            CameraFollow.TrackingObject = _currentPlayer.transform;
            _currentPlayer.GetComponent<Bomberman>().Level = Level;
        }

        void Update()
        {
            if (Input.GetButtonDown("Respawn") && _currentPlayer == null)
                SpawnPlayer();
        }
    }
}
