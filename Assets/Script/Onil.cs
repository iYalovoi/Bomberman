using System.Linq;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class Onil : MonoBehaviour, ITarget
    {
        public float MaxSpeed = 2f;
        public float Direction;
        public Direction Way;
        public bool Dead;

        private Animator _animator;

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if(!Dead)
            {

                var tileSize = renderer.bounds.size.x;
                var localPosition = gameObject.transform.localPosition;
                var currentTile = new Vector2(Mathf.Round(localPosition.x / tileSize), Mathf.Round(localPosition.y / tileSize));
                const float eps = 0.1f;
                if ((Mathf.Abs(localPosition.x - currentTile.x * tileSize) < eps) && (Mathf.Abs(localPosition.y - currentTile.y * tileSize) < eps) && Random.value > 0.9)
                {
                    var way = (Direction)Mathf.Pow(2, Random.Range(0, 4));
                    var block = MapDiscovery.BlastInDirection(transform.position, tileSize, way, 1);
                    if (block.Select(o => o.transform.gameObject).All(o => o.tag != "Wall"))
                        Way = way;                    
                }
                rigidbody2D.velocity = new Vector2(Utility.Direction.Horizontal.IsFlagSet(Way) ? Way == Utility.Direction.Left ? -MaxSpeed : MaxSpeed : 0,
                                                   Utility.Direction.Vertical.IsFlagSet(Way) ? Way == Utility.Direction.Up ? MaxSpeed : -MaxSpeed : 0);
				if (rigidbody2D.velocity.x > 0)
                {
                    _animator.SetFloat("Direction", 1);
                }
				else
                {
                    _animator.SetFloat("Direction", -1);
                }                                 
            }
            else rigidbody2D.velocity = new Vector2();
        }

        public void Die()
        {
            Dead = true;
            _animator.SetTrigger("Die");
        }

        public void OnHit(GameObject striker)
        {
            Die();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
