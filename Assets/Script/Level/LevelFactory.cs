using System.Collections;
using System.Collections.Generic;
using Assets.Script.Utility;
using UnityEngine;
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

        private GameObject _currentPlayer;
        private Vector2 _tileSize;
        private PowerUpFactory _powerUpFactory;
        private EnemyFactory _enemyFactory;

        private Messenger _messenger;
        private LevelModel _model;

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
        }

        private void OnInjected(Messenger messenger, LevelModel model)
        {
            _model = model;
            _messenger = messenger;                       
            _messenger.Subscribe(Signals.SpawnPontans, SpawnPontans);
        }

        public void ProduceLevel(int level)
        {
            LevelObject = new GameObject("Level");
            CurrentLevelDefinition = Build(level);
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
                        Place(_powerUpFactory.Produce(levelPosition.PowerUp.Value), i, j);
                }
            }

            AdjustPlayer();
            LevelObject.transform.position = new Vector3(-_tileSize.x * CurrentLevelDefinition.Width / 2, -_tileSize.y * CurrentLevelDefinition.Height / 2);

            if(!Camera.audio.isPlaying)
                Camera.audio.Play();
        }

        void SpawnPontans()
        {
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
            if (_currentPlayer == null)
            {
                _currentPlayer = Create(Player, 1, 1);
                //TODO - Player position should be properly set depending on the current transform
                _currentPlayer.transform.parent = LevelObject.transform;

                CameraFollow.TrackingObject = _currentPlayer.transform;
                _currentPlayer.GetComponent<Bomberman>().Level = LevelObject;
            }
            _currentPlayer.transform.localPosition = new Vector3(_tileSize.x, _tileSize.y, 0);
        }

        private LevelDefinition Build(int level)
        {
            switch (level)
            {
                case 1:
				return new LevelDefinition(Powers.Fire, new EnemyCounts() { { EnemyTypes.Balloon, 6 } });
                case 2:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts() { { EnemyTypes.Balloon, 3 }, { EnemyTypes.Onil, 3 } });
                case 3:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts() { { EnemyTypes.Balloon, 2 }, { EnemyTypes.Onil, 2 }, { EnemyTypes.Dahl, 2 } });
                case 4:
                    return new LevelDefinition(Powers.Speed, new EnemyCounts() { { EnemyTypes.Balloon, 1 }, { EnemyTypes.Onil, 1 }, { EnemyTypes.Dahl, 2 }, { EnemyTypes.Doria, 2 } });
                case 5:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts() { { EnemyTypes.Onil, 4 }, { EnemyTypes.Dahl, 3 } });
                case 6:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 2},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 0},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 7:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 2},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 8:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 4},{EnemyTypes.Minvo, 0},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 9:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 4},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 10:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 11:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 12:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 4},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 13:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 14:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 0},
							{EnemyTypes.Ovape, 7}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 15:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 16:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 4},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 17:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 5},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 18:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 3},
							{EnemyTypes.Onil, 3},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 0},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 19:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 1},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 0},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 20:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 21:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 4},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 22:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 4},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 23:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 24:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 4},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 25:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 2},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 26:
                    return new LevelDefinition(Powers.Mystery, new EnemyCounts(){{EnemyTypes.Balloon, 1},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 27:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 1},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 5},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 28:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 29:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 5}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 30:
                    return new LevelDefinition(Powers.FlamePass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 3},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 31:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 2},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 0},{EnemyTypes.Pontan, 0}});
                case 32:
                    return new LevelDefinition(Powers.BombUp, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 1},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 4},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 1},{EnemyTypes.Pontan, 0}});
                case 33:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 34:
                    return new LevelDefinition(Powers.Mystery, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 3},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 35:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 2},{EnemyTypes.Pontan, 0}});
                case 36:
                    return new LevelDefinition(Powers.FlamePass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 3},{EnemyTypes.Pontan, 0}});
                case 37:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 3},{EnemyTypes.Pontan, 0}});
                case 38:
                    return new LevelDefinition(Powers.Fire, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 2},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 3},{EnemyTypes.Pontan, 0}});
                case 39:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 4},{EnemyTypes.Pontan, 0}});
                case 40:
                    return new LevelDefinition(Powers.Mystery, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 4},{EnemyTypes.Pontan, 0}});
                case 41:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 1},{EnemyTypes.Doria, 2},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 0}, {EnemyTypes.Pass, 4},{EnemyTypes.Pontan, 0}});
                case 42:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 3},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 5},{EnemyTypes.Pontan, 0}});
                case 43:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 0}});
                case 44:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 1},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 0}});
                case 45:
                    return new LevelDefinition(Powers.Mystery, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 0}});
                case 46:
                    return new LevelDefinition(Powers.WallPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 0}});
                case 47:
                    return new LevelDefinition(Powers.BombPass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 0}});
                case 48:
                    return new LevelDefinition(Powers.RemoteControl, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 2},
							{EnemyTypes.Ovape, 1}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 1}});
                case 49:
                    return new LevelDefinition(Powers.FlamePass, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 6},{EnemyTypes.Pontan, 1}});
                case 50:
                    return new LevelDefinition(Powers.Mystery, new EnemyCounts(){{EnemyTypes.Balloon, 0},
							{EnemyTypes.Onil, 0},{EnemyTypes.Dahl, 0},{EnemyTypes.Doria, 0},{EnemyTypes.Minvo, 1},
							{EnemyTypes.Ovape, 2}, {EnemyTypes.Pass, 5},{EnemyTypes.Pontan, 2}});
            }
            return null;
        }

    }
}