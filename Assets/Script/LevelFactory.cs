using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
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
		public GameObject Enemy;
        public GameObject Door;
        public GameObject PowerUp;

        public GameObject Camera;
        public CameraFollow CameraFollow;
        public GameObject LevelObject;
        public LevelDefinition CurrentLevel;

        private GameObject _currentPlayer;
		private Vector2 _tileSize;        

        IEnumerator Start()
        {
            //DI Unity way; Shitty way; Igor.
            var powerUpFactory = FindObjectOfType<PowerUpFactory>();
			var enemyFactory = FindObjectOfType<EnemyFactory>();
            var softBlocks = new List<GameObject>();
            CameraFollow = Camera.GetComponent<CameraFollow>();

			_tileSize = HardBlock.renderer.bounds.size;
            LevelObject = new GameObject("Level");

			CurrentLevel = new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 1},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 1}});
			var map = CurrentLevel.GenerateMap();
			var blockMap = new Dictionary<BlockTypes, GameObject> (){{BlockTypes.Soft, Soft}, {BlockTypes.Hard, HardBlock}, {BlockTypes.Wall, Wall}};
			for (var i = 0; i < CurrentLevel.Width; i++) 
			{
				for (var j = 0; j < CurrentLevel.Height; j++) 
				{
					var levelPosition = map[i, j];
					if(levelPosition.BlockType != BlockTypes.None)
						Create(blockMap[levelPosition.BlockType], i, j);
					if(levelPosition.Enemy.HasValue)
						Place(enemyFactory.Produce(levelPosition.Enemy.Value), i , j);
					if(levelPosition.Door)
						Create(Door, i, j);
					if(levelPosition.PowerUp.HasValue)
						Place(powerUpFactory.Produce(), i, j);
				}
			}

			SpawnPlayer();
			LevelObject.transform.position = new Vector3(-_tileSize.x * CurrentLevel.Width / 2, -_tileSize.y * CurrentLevel.Height / 2);

            yield return new WaitForSeconds(CurrentLevel.TimeLimit);

            //Spawn 20 pontans when time hits 0:00
            for (var i = 0; i < 20; i++)
                Place(enemyFactory.Produce(EnemyTypes.Pontan), Random.Range(1, CurrentLevel.Width - 1), Random.Range(1, CurrentLevel.Height - 1));
		}

		private GameObject Create(GameObject prototype, int x, int y)
		{
			var result = Instantiate(prototype) as GameObject;
		    return Place(result, x, y);
		}

        private GameObject Place(GameObject target, int x, int y)
        {            
            target.name = string.Format("{2} {0}:{1}", x, y, target.name);
            target.transform.parent = LevelObject.transform;
            target.transform.localPosition = new Vector3(x * _tileSize.x, y * _tileSize.y, 0);
            return target;
        }

        public void SpawnPlayer()
        {
			_currentPlayer = Create (Player, 1, 1);
			//TODO - Player position should be properly set depending on the current transform
			_currentPlayer.transform.parent = LevelObject.transform;
			_currentPlayer.transform.localPosition = new Vector3(_tileSize.x, _tileSize.y, 0);

            CameraFollow.TrackingObject = _currentPlayer.transform;
            _currentPlayer.GetComponent<Bomberman>().Level = LevelObject;
        }

        void Update()
        {
            if (Input.GetButtonDown("Respawn") && _currentPlayer == null)
                SpawnPlayer();
        }
    }
}
