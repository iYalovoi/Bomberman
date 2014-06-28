using System;
using System.Collections;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Bomberman : MonoBehaviour, ITarget
    {
        private Animator _animator;

        // Use this for initialization
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public float MaxSpeed = 5f; // The fastest the player can travel in the axis.
        public Powers? Powers;
        public GameObject Bomb;
        public int BombCount = 1;
        public GameObject Level;
        public bool Bombing;
        public bool Dead;
        public Direction Direction;
        public AudioSource FootStepsSound;
        public AudioSource DeathSound;
        public AudioSource PlaceBombSound;
        public CircleCollider2D Solid;


        private bool _restrained;
        public bool Restrained
        {
            get { return Bombing || Dead || _restrained; }
            set { _restrained = value; }
        }        

        // Update is called once per frame
        private void Update()
        {
            if (Level != null && !Dead)
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
                        var bombScript = bomb.GetComponent<Bomb>();
                        bombScript.Level = Level;
                        bombScript.Bomberman = Solid;
                        PlaceBombSound.Play();
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

                if (Math.Abs(vertical) > 0.1)
                    Direction = vertical > 0 ? Direction.Up : Direction.Down;
                else if (Math.Abs(horizontal) > 0.1)
                    Direction = horizontal > 0 ? Direction.Right : Direction.Left;

                if (Bombing)
                    _animator.SetBool("Bombing", true);
                if (!Restrained)
                {
                    rigidbody2D.velocity = Mathf.Abs(horizontal) > 0 ? new Vector2(Mathf.Sign(horizontal) * MaxSpeed, rigidbody2D.velocity.y) : new Vector2(0, rigidbody2D.velocity.y);
                    rigidbody2D.velocity = Mathf.Abs(vertical) > 0 ? new Vector2(rigidbody2D.velocity.x, Mathf.Sign(vertical) * MaxSpeed) : new Vector2(rigidbody2D.velocity.x, 0);

                    _animator.SetFloat("Horizontal", 0);
                    _animator.SetFloat("Vertical", 0);

                    switch (Direction)
                    {
                        case Direction.Left:
                            _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? -1f : -0.1f);
                            break;
                        case Direction.Up:
                            _animator.SetFloat("Vertical", Math.Abs(vertical) > 0.1 ? 1f : 0.1f);
                            break;
                        case Direction.Right:
                            _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? 1f : 0.1f);
                            break;
                        case Direction.Down:
                            _animator.SetFloat("Vertical", Math.Abs(vertical) > 0.1 ? -1f : -0.1f);
                            break;
                    }

                    if (Mathf.Abs(vertical) > 0.1 || Mathf.Abs(horizontal) > 0.1)
                    {
                        if (!FootStepsSound.isPlaying)
                            FootStepsSound.Play();
                    }
                    else if (FootStepsSound.isPlaying) FootStepsSound.Pause();
                }
            }
        }

        public void Die()
        {
            if(!Dead)
            {
                Dead = true;
                _animator.SetTrigger("Die");
                DeathSound.Play();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            Die();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if(coll.gameObject.tag == "Enemy")
                Die();
        }

        public void AcceptPower(Powers power)
        {
        }
    }
}
