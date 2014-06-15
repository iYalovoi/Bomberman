using UnityEngine;

namespace Assets.Script
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameObject Wall;
        public GameObject HardBlock;
        public GameObject Player;
        public GameObject Camera;
        public GameObject Soft;
        public GameObject Baloon;
        public CameraFollow CameraFollow;
        public GameObject Level;
        public int BaloonCount = 6;
        private GameObject _currentPlayer;

        void Start()
        {
            var baloonCount = BaloonCount;
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
                                }
                            }
                        }
                    }
                }            

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
            _currentPlayer.GetComponent<Player>().Level = Level;
        }

        void Update()
        {
            if (Input.GetButtonDown("Respawn") && _currentPlayer == null)
                SpawnPlayer();
        }
    }
}
