using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Level;
using EnemyCounts = System.Collections.Generic.Dictionary<Assets.Script.Level.EnemyTypes, uint>;

namespace Assets.Script
{
    public class LevelFactory : MonoBehaviour
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
		private Vector2 _tileSize;

        void Start()
        {
            //DI Unity way; Shitty way; Igor.
            var powerUpFactory = FindObjectOfType<PowerUpFactory>();

            var enemies = new List<GameObject>();
            var softBlocks = new List<GameObject>();
            CameraFollow = Camera.GetComponent<CameraFollow>();

			_tileSize = HardBlock.renderer.bounds.size;
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
						//Powerup switch? Alexey
						Place(powerUpFactory.Produce(), i, j);
					}
				}
			}

			SpawnPlayer();
			Level.transform.position = new Vector3(-_tileSize.x * levelDefinition.Width / 2, -_tileSize.y * levelDefinition.Height / 2);			
		}

		private GameObject Create(GameObject prototype, int x, int y)
		{
			var result = Instantiate(prototype) as GameObject;
		    return Place(result, x, y);
		}

        private GameObject Place(GameObject target, int x, int y)
        {            
            target.name = string.Format("{2} {0}:{1}", x, y, target.name);
            target.transform.parent = Level.transform;
            target.transform.localPosition = new Vector3(x * _tileSize.x, y * _tileSize.y, 0);
            return target;
        }

        public void SpawnPlayer()
        {
			_currentPlayer = Create (Player, 1, 1);
			//TODO - Player position should be properly set depending on the current transform
			_currentPlayer.transform.parent = Level.transform;
			_currentPlayer.transform.localPosition = new Vector3(_tileSize.x, _tileSize.y, 0);

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
