using System.Collections;
using System.Collections.Generic;
using Assets.Script.Utility;
using UnityEngine;
using Assets.Script;
using Assets.Script.Level;
using EnemyCounts = System.Collections.Generic.Dictionary<Assets.Script.Level.EnemyTypes, uint>;

namespace Assets.Script
{
    public class LevelFactory : ContainerBase
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
        public LevelDefinition CurrentLevelDefinition;
        private int _currentLevel = 0;

        private GameObject _currentPlayer;
		private Vector2 _tileSize;        
        private PowerUpFactory _powerUpFactory;
        private EnemyFactory _enemyFactory;

        private Messenger _messenger;

        void Awake()
        {
            ImmortalBook.Add(this);
        }

        protected override void Start()
        {
            base.Start();
            //DI Unity way; Shitty way; Igor.
            _powerUpFactory = FindObjectOfType<PowerUpFactory>();
			_enemyFactory = FindObjectOfType<EnemyFactory>();
            CameraFollow = Camera.GetComponent<CameraFollow>();
			_tileSize = HardBlock.renderer.bounds.size;
            StartCoroutine(ProduceLevel());
        }

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
            _messenger.Subscribe(Signals.DoorOpened, () => StartCoroutine(DoorOpened()));
        }

        private IEnumerator DoorOpened()
        {
            Application.LoadLevel("Battle");
            yield return new WaitForSeconds(0);
            StartCoroutine(ProduceLevel());
        }

        public IEnumerator ProduceLevel()
        {
            Debug.Log(_currentLevel);
            LevelObject = new GameObject("Level");
            CurrentLevelDefinition = Build(++_currentLevel);
            var map = CurrentLevelDefinition.GenerateMap();
            var blockMap = new Dictionary<BlockTypes, GameObject>() { { BlockTypes.Soft, Soft }, { BlockTypes.Hard, HardBlock }, { BlockTypes.Wall, Wall } };
            for (var i = 0; i < CurrentLevelDefinition.Width; i++)
            {
                for (var j = 0; j < CurrentLevelDefinition.Height; j++)
                {
                    var levelPosition = map[i, j];
                    if (levelPosition.BlockType != BlockTypes.None)
                        Create(blockMap[levelPosition.BlockType], i, j);
                    if (levelPosition.Enemy.HasValue)
                        Place(_enemyFactory.Produce(levelPosition.Enemy.Value), i, j);
                    if (levelPosition.Door)
                        Create(Door, i, j);
                    if (levelPosition.PowerUp.HasValue)
                        Place(_powerUpFactory.Produce(), i, j);
                }
            }

            AdjustPlayer();
            LevelObject.transform.position = new Vector3(-_tileSize.x * CurrentLevelDefinition.Width / 2, -_tileSize.y * CurrentLevelDefinition.Height / 2);

            yield return new WaitForSeconds(CurrentLevelDefinition.TimeLimit);

            //Spawn 20 pontans when time hits 0:00
            for (var i = 0; i < 20; i++)
                Place(_enemyFactory.Produce(EnemyTypes.Pontan), Random.Range(1, CurrentLevelDefinition.Width - 1), Random.Range(1, CurrentLevelDefinition.Height - 1));
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

        public void AdjustPlayer()
        {
            Debug.Log(_currentPlayer);
            if (_currentPlayer == null)
            {
                _currentPlayer = Create(Player, 1, 1);
                //TODO - Player position should be properly set depending on the current transform
                _currentPlayer.transform.parent = LevelObject.transform;
                

                CameraFollow.TrackingObject = _currentPlayer.transform;
                _currentPlayer.GetComponent<Bomberman>().Level = LevelObject;
            }
            _currentPlayer.transform.localPosition = new Vector3(_tileSize.x, _tileSize.y, 0);
            Debug.Log(_currentPlayer);
        }

        private LevelDefinition Build(int level)
        {
            switch (level)
            {
                case 1:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 6}});
                case 2:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 3},{EnemyTypes.Onil, 3}});
                case 3:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 2}, {EnemyTypes.Onil, 2},{EnemyTypes.Dahl, 2}});
                case 4:
                    return new LevelDefinition(Powers.Speed, new EnemyCounts(){{EnemyTypes.Balloon, 1},{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2}});
                case 5:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Onil, 4},{EnemyTypes.Dahl, 3}});
                case 6:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 1},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 1}});
            }
            return null;
        }

        void Update()
        {
            if (Input.GetButtonDown("Respawn"))
                AdjustPlayer();
        }
    }
}
