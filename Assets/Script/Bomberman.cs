using System;
using System.Collections;
using System.Linq;
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

        public GameObject Bomb;
        public float MaxSpeed = 5f; // The fastest the player can travel in the axis.
        public GameObject Level;
        public bool Bombing;
        public int Direction;
        public AudioSource FootStepsSound;
        public AudioSource DeathSound;
        public AudioSource PlaceBombSound;


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
                    Direction = vertical > 0 ? 1 : 3;
                else if (Math.Abs(horizontal) > 0.1)
                    Direction = horizontal > 0 ? 2 : 0;                

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
                        case 0:
                            _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? -1f : -0.1f);
                            break;
                        case 1:
                            _animator.SetFloat("Vertical", Math.Abs(vertical) > 0.1 ? 1f : 0.1f);
                            break;
                        case 2:
                            _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? 1f : 0.1f);
                            break;
                        case 3:
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
            _restrained = true;
            _animator.SetTrigger("Die");
            DeathSound.Play();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            Die();
        }
    }
}
