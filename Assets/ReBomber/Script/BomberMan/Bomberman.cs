using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Bomberman : ContainerBase, ITarget
    {
        private Animator _animator;
        private readonly List<Action> _subscriptions = new List<Action>();

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bomb"), LayerMask.NameToLayer("ReBomber"), BombPass);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Soft"), LayerMask.NameToLayer("ReBomber"), WallPass);
        }

        private void OnInjected(BombermanModel model, Messenger messenger)
        {
            _messenger = messenger;
            _model = model;
//            if (Application.isEditor)
//                _model.Godlike();

            _subscriptions.Add(_messenger.Subscribe<int>((o) =>
                    {
                        _model.Score += o;
                    }));

            _subscriptions.Add(_messenger.Subscribe(Signals.SoftBlockDestroyed, () =>
                    {
                        _model.Score += 10;
                    }));

            _subscriptions.Add(_messenger.Subscribe(Signals.DoorOpened, () =>
                    {

                    }));
        }

        private BombermanModel _model;
        private Messenger _messenger;

        public GameObject Bomb;
        public GameObject Level;
        public bool Bombing;
        public bool Dead;
        public Direction Direction;
        public AudioSource FootStepsSound;
        public AudioSource DeathSound;
        public AudioSource PlaceBombSound;
        public AudioSource DenyBombSound;
        public CircleCollider2D Solid;
        public Queue<Bomb> Bombs = new Queue<Bomb>();

        //Power ups
        public int BombCount
        {
            get { return _model.BombCount; }
            set { _model.BombCount = value; }
        }

        public int Radius
        {
            get { return _model.Radius; }
            set { _model.Radius = value; }
        }

        public bool FlamePass
        {
            get { return _model.FlamePass; }
            set { _model.FlamePass = value; }
        }

        public bool Invincible
        {
            get { return _model.Invincible; }
            set { _model.Invincible = value; }
        }

        public bool RemoteControl
        {
            get { return _model.RemoteControl; }
            set { _model.RemoteControl = value; }
        }

        public float Speed
        {
            get { return _model.Speed; }
            set { _model.Speed = value; }
        }

        public bool WallPass
        {
            get { return _model.WallPass; }
            set { _model.WallPass = value; }
        }

        public bool BombPass
        {
            get { return _model.BombPass; }
            set { _model.BombPass = value; }
        }

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
                if ((Input.GetButtonDown("Bomb") || Input.GetButtonDown("Joystick Bomb")))
                {
                    //Getting proper bomb location
                    var tileSize = Bomb.renderer.bounds.size.x;
                    var localPosition = gameObject.transform.localPosition;

                    var collidersInArea = Physics2D.OverlapPointAll(gameObject.transform.position);
                    if (collidersInArea.All(o => o.gameObject.tag != "Bomb" && o.gameObject.tag != "Wall") && FindObjectsOfType<Bomb>().Count() < BombCount)
                    {
                        Bombing = true;
                        var bombTile = new Vector2(Mathf.RoundToInt(localPosition.x / tileSize),
                                           Mathf.RoundToInt(localPosition.y / tileSize));
                        var bomb = Instantiate(Bomb, new Vector3(), new Quaternion()) as GameObject;
                        bomb.name = string.Format("Bomb {0}:{1}", bombTile.x, bombTile.y);
                        bomb.transform.parent = Level.transform;
                        bomb.transform.localPosition = new Vector3(bombTile.x * tileSize, bombTile.y * tileSize);
                        var bombScript = bomb.GetComponent<Bomb>();
                        bombScript.Level = Level;
                        bombScript.Bomberman = Solid;
                        bombScript.Radius = Radius;
                        bombScript.RemoteControl = RemoteControl;
                        PlaceBombSound.Play();

                        if (RemoteControl)
                            Bombs.Enqueue(bombScript);

                        GA.API.Design.NewEvent("Bombed");
                    }
                    else
                        DenyBombSound.Play();
                }
                Bombs = new Queue<Bomb>(Bombs.Where(o => o != null));
                if ((Input.GetButtonDown("Fire") || (Input.GetButtonDown("Joystick Fire"))) && RemoteControl && Bombs.Any())
                {
                    var bomb = Bombs.Dequeue();
                    StartCoroutine(bomb.ExplodeZero());
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
                var vertical = Input.GetAxis("Vertical") + Input.GetAxis("Joystick Vertical");
                var horizontal = Input.GetAxis("Horizontal") + Input.GetAxis("Joystick Horizontal");

                if (Math.Abs(vertical) > 0.1)
                    Direction = vertical > 0 ? Direction.Up : Direction.Down;
                else if (Math.Abs(horizontal) > 0.1)
                    Direction = horizontal > 0 ? Direction.Right : Direction.Left;

                if (Bombing)
                    _animator.SetBool("Bombing", true);

                if (Restrained)
                {
                    horizontal = 0;
                    vertical = 0;
                }

                rigidbody2D.velocity = Mathf.Abs(horizontal) > 0
                    ? new Vector2(Mathf.Sign(horizontal) * Speed, rigidbody2D.velocity.y)
                    : new Vector2(0, rigidbody2D.velocity.y);
                rigidbody2D.velocity = Mathf.Abs(vertical) > 0
                    ? new Vector2(rigidbody2D.velocity.x, Mathf.Sign(vertical) * Speed)
                    : new Vector2(rigidbody2D.velocity.x, 0);

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
                else if (FootStepsSound.isPlaying)
                    FootStepsSound.Pause();
            }
        }

        private void OnDestroy()
        {
            _subscriptions.ForEach(o => o());
        }

        public void Die()
        {
            if (!Dead && !Invincible)
            {
                Dead = true;
                _animator.SetTrigger("Die");
                DeathSound.Play();
                GA.API.Design.NewEvent("Dead"); 
            }
        }

        public void Destroy()
        {
            _model.LosePower();
            _model.Lifes--;
            _messenger.Signal(_model.Lifes >= 0 ? Signals.RestartLevel : Signals.GameOver);
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            if (!FlamePass)
                Die();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Enemy" && !coll.gameObject.GetComponent<Enemy>().Dead)
                Die();
        }

        public void AcceptPower(Powers power)
        {
            GA.API.Design.NewEvent("PowerUp", (float)power); 
            switch (power)
            {
                case Powers.BombUp:
                    ++BombCount;
                    break;
                case Powers.Fire:
                    ++Radius;
                    break;
                case Powers.BombPass:
                    BombPass = true;
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bomb"), LayerMask.NameToLayer("ReBomber"), true);
                    break;
                case Powers.FlamePass:
                    FlamePass = true;
                    break;
                case Powers.Immortal:
                    Invincible = true;
                    StartCoroutine(InvincibleSpree());
                    break;
                case Powers.WallPass:
                    WallPass = true;
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Soft"), LayerMask.NameToLayer("ReBomber"), true);
                    break;
                case Powers.Speed:
                    Speed = Constants.BasePlayerSpeed + Constants.SpeedPowerUp;
                    break;
                case Powers.RemoteControl:
                    RemoteControl = true;
                    break;
                default:
                    break;
            }
            _messenger.Signal<Powers>(power);
        }

        private IEnumerator InvincibleSpree()
        {
            yield return new WaitForSeconds(30);
            Invincible = false;
            _messenger.Signal<Powers>(Powers.Immortal);
        }

    }
}
