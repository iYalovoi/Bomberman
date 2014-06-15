using System;
using UnityEngine;

namespace Assets.Script
{
    public class Baloon : MonoBehaviour
    {

        public float MaxSpeed = 3f;
        public float Direction;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {

            var tileSize = renderer.bounds.size.x;
            var localPosition = gameObject.transform.localPosition;
            var currentTile = new Vector2(Mathf.Round(localPosition.x / tileSize), Mathf.Round(localPosition.y / tileSize));

            if (Mathf.Abs(localPosition.x % tileSize) < 0.1 && Mathf.Abs(localPosition.y % tileSize) < 0.1)
            {

                //var hits = Physics2D.LinecastAll(launch, hit);

                //rigidbody2D.velocity = Mathf.Abs(horizontal) > 0 ? new Vector2(Mathf.Sign(horizontal) * MaxSpeed, rigidbody2D.velocity.y) : new Vector2(0, rigidbody2D.velocity.y);
                //rigidbody2D.velocity = Mathf.Abs(vertical) > 0 ? new Vector2(rigidbody2D.velocity.x, Mathf.Sign(vertical) * MaxSpeed) : new Vector2(rigidbody2D.velocity.x, 0);

                //_animator.SetFloat("Horizontal", 0);
                //_animator.SetFloat("Vertical", 0);

                //switch (Direction)
                //{
                //    case 0:
                //        _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? -1f : -0.1f);
                //        break;
                //    case 1:
                //        _animator.SetFloat("Vertical", Math.Abs(vertical) > 0.1 ? 1f : 0.1f);
                //        break;
                //    case 2:
                //        _animator.SetFloat("Horizontal", Math.Abs(horizontal) > 0.1 ? 1f : 0.1f);
                //        break;
                //    case 3:
                //        _animator.SetFloat("Vertical", Math.Abs(vertical) > 0.1 ? -1f : -0.1f);
                //        break;
                //}   
            }            
        }
    }
}
