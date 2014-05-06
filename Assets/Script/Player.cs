using UnityEngine;

namespace Assets.Script
{
    public class Player : MonoBehaviour
    {

        public GameObject Bomb;

        // Use this for initialization
        private void Start()
        {

        }

        public float MaxSpeed = 5f; // The fastest the player can travel in the axis.
        public float MoveForce = 365f; // Amount of force added to move the player left and right.
        public GameObject Level;

        // Update is called once per frame
        private void Update()
        {
            // If the fire button is pressed...
            if (Input.GetButtonDown("Bomb"))
            {
                //Getting proper bomb location
                var tileSize = Bomb.renderer.bounds.size.x;
                var playerLocation = gameObject.transform.localPosition;
                var bombTile = new Vector2(playerLocation.x/tileSize, playerLocation.y/tileSize);
                var bomb = Instantiate(Bomb, new Vector3(), new Quaternion()) as GameObject;
                bomb.transform.parent = Level.transform;
                bomb.transform.localPosition = new Vector3(Mathf.RoundToInt(bombTile.x) * tileSize, Mathf.RoundToInt(bombTile.y) * tileSize);
            }
        }

        private void FixedUpdate()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            if (horizontal * rigidbody2D.velocity.x < MaxSpeed)
                rigidbody2D.AddForce(Vector2.right * horizontal * MoveForce);

            if (Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed)
                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * MaxSpeed, rigidbody2D.velocity.y);

            if (vertical * rigidbody2D.velocity.y < MaxSpeed)
                rigidbody2D.AddForce(Vector2.up * vertical * MoveForce);

            if (Mathf.Abs(rigidbody2D.velocity.y) > MaxSpeed)
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * MaxSpeed);
        }
    }
}
