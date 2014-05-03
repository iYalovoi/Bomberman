using UnityEngine;

namespace Assets.Script
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameObject Wall;
        public GameObject HardBlock;
        public GameObject Player;
        public GameObject Camera;

        void Start()
        {
            var size = HardBlock.renderer.bounds.size;
            var level = new GameObject("Level");

            const int rowCount = 13;
            const int columnCount = 31;

            //Level pillars
            for (var i = 0; i < rowCount; i++)
                for (var j = 0; j < columnCount; j++)
                {
                    var isWall = i == 0 || i == 12 || j == 0 || j == 30;
                    if (isWall || ((i % 2) == 0 && (j % 2) == 0))
                    {
                        var newBlock = Instantiate(isWall ? Wall : HardBlock, new Vector3(j * size.x, i * size.y, 0), new Quaternion()) as GameObject;
                        newBlock.transform.parent = level.transform;
                    }
                }         
   
            //Create player
            var player = Instantiate(Player, new Vector3(size.x, size.y, 0), new Quaternion()) as GameObject;
            player.transform.parent = level.transform;

            var cameraFollow = Camera.GetComponent<CameraFollow>();
            cameraFollow.TrackingObject = player.transform;

            level.transform.position = new Vector3(-size.x * columnCount / 2, -size.y * rowCount / 2);
        }

        void Update()
        {

        }
    }
}
