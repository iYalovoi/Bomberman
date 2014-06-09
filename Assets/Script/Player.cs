using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Script
{
    public class Player : MonoBehaviour
    {

        private Animator _animator;

        // Use this for initialization
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public GameObject Bomb;
        public float MaxSpeed = 5f; // The fastest the player can travel in the axis.
        public float MoveForce = 365f; // Amount of force added to move the player left and right.
        public GameObject Level;
        public bool Bombing;


        private bool _restrained;
        public bool Restrained
        {
            get { return Bombing || _restrained; }
            set { _restrained = value; }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Level != null)
            {
                // If the fire button is pressed...
                if (Input.GetButtonDown("Bomb"))
                {
                    //Getting proper bomb location
                    var tileSize = Bomb.renderer.bounds.size.x;
                    var localPosition = gameObject.transform.localPosition;

                    var collidersInArea = Physics2D.OverlapCircleAll(gameObject.transform.position, tileSize / 2);
                    if (collidersInArea.All(o => o.gameObject.tag != "Bomb"))
                    {
                        Bombing = true;
                        var bombTile = new Vector2(Mathf.RoundToInt(localPosition.x / tileSize), Mathf.RoundToInt(localPosition.y / tileSize));
                        var bomb = Instantiate(Bomb, new Vector3(), new Quaternion()) as GameObject;
                        bomb.name = string.Format("Bomb {0}:{1}", bombTile.x, bombTile.y);
                        bomb.transform.parent = Level.transform;
                        bomb.transform.localPosition = new Vector3(bombTile.x * tileSize, bombTile.y * tileSize);
                        bomb.GetComponent<Bomb>().Level = Level;
                    }
                }
            }
        }

        private void Bombed()
        {
            Bombing = false;
            _animator.SetBool("Bombing", false);
        }

        private void FixedUpdate()
        {
            if (Level != null)
            {
                var vertical = Input.GetAxis("Vertical");
                var horizontal = Input.GetAxis("Horizontal");


                if (Bombing)
                    _animator.SetBool("Bombing", true);
                if (!Restrained)
                {
                    if (horizontal * rigidbody2D.velocity.x < MaxSpeed)
                        rigidbody2D.AddForce(Vector2.right * horizontal * MoveForce);

                    if (Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed)
                        rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * MaxSpeed, rigidbody2D.velocity.y);

                    if (vertical * rigidbody2D.velocity.y < MaxSpeed)
                        rigidbody2D.AddForce(Vector2.up * vertical * MoveForce);

                    if (Mathf.Abs(rigidbody2D.velocity.y) > MaxSpeed)
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * MaxSpeed);

                    _animator.SetFloat("Horizontal", rigidbody2D.velocity.x / MaxSpeed * 10);
                    _animator.SetFloat("Vertical", rigidbody2D.velocity.y / MaxSpeed * 10);
                }
            }
        }

        public void Die()
        {
            _restrained = true;
            _animator.SetTrigger("Die");
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
