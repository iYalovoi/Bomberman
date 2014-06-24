using System.Collections.Generic;
using UnityEngine;

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
        public int BaloonCount = 6;
		public int OnilCount = 6;

        private GameObject _currentPlayer;

        void Start()
        {
            var baloonCount = BaloonCount;
			var onilCount = OnilCount;
            var enemies = new List<GameObject>();
            var softBlocks = new List<GameObject>();
            CameraFollow = Camera.GetComponent<CameraFollow>();

            var size = HardBlock.renderer.bounds.size;
            Level = new GameObject("Level");

            const int rowCount = 13;
            const int columnCount = 31;

            //Level pillars
            for (var i = 0; i < rowCount; i++)
                for (var j = 0; j < columnCount; j++)
                {
                    var isWall = i == 0 || i == 12 || j == 0 || j == 30;
                    if (isWall || ((i % 2) == 0 && (j % 2) == 0))
                    {
                        var hard = Instantiate(isWall ? Wall : HardBlock, new Vector3(j * size.x, i * size.y, 0), new Quaternion()) as GameObject;
                        hard.name = string.Format("{2} {0}:{1}", i, j, isWall ? "Wall" : "HardBlock");
                        hard.transform.parent = Level.transform;
                    }
                    else
                    {
                        if (Random.value >= 0.7 && !(i == 1 && j == 1) && !(i == 2 && j == 1) && !(i == 1 && j == 2))
                        {
                            var soft = Instantiate(Soft, new Vector3(j*size.x, i*size.y, 0), new Quaternion()) as GameObject;
                            soft.name = string.Format("Soft {0}:{1}", i, j);
                            soft.transform.parent = Level.transform;
                            softBlocks.Add(soft);
                        }
                        else
                        {
                            if (baloonCount > 0)
                            {
                                if (Random.value >= 0.95)
                                {
                                    var baloon = Instantiate(Baloon, new Vector3(j * size.x, i * size.y, 0), new Quaternion()) as GameObject;
                                    baloon.name = string.Format("Baloon {0}:{1}", i, j);
                                    baloon.transform.parent = Level.transform;
                                    baloonCount--;
                                    enemies.Add(baloon);
                                }
                            }
							if (onilCount > 0)
							{
								if (Random.value >= 0.95)
								{
									var onil = Instantiate(Onil, new Vector3(j * size.x, i * size.y, 0), new Quaternion()) as GameObject;
									onil.name = string.Format("Onil {0}:{1}", i, j);
									onil.transform.parent = Level.transform;
									onilCount--;
									enemies.Add(onil);
								}
							}
                        }
                    }
                }

            //Add door
            var door = Instantiate(Door, softBlocks[Random.Range(0, softBlocks.Count)].transform.position, new Quaternion()) as GameObject;
            door.transform.parent = Level.transform;

            //Add powerUp
            var powerUp = Instantiate(PowerUp, softBlocks[Random.Range(0, softBlocks.Count)].transform.position, new Quaternion()) as GameObject;
            powerUp.transform.parent = Level.transform;

            Level.transform.position = new Vector3(-size.x * columnCount / 2, -size.y * rowCount / 2);
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            var size = HardBlock.renderer.bounds.size;
            //Create player
            _currentPlayer = Instantiate(Player, new Vector3(), new Quaternion()) as GameObject;
            _currentPlayer.transform.parent = Level.transform;
            _currentPlayer.transform.localPosition = new Vector3(size.x, size.y, 0);

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
