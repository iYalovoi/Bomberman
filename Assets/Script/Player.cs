﻿using System.Collections;
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
        private bool _bombing;

        // Update is called once per frame
        private void Update()
        {
            // If the fire button is pressed...
            if (Input.GetButtonDown("Bomb"))
            {
                _bombing = true;
                //Getting proper bomb location
                var tileSize = Bomb.renderer.bounds.size.x;
                var playerLocation = gameObject.transform.localPosition;
                var bombTile = new Vector2(playerLocation.x/tileSize, playerLocation.y/tileSize);
                var bomb = Instantiate(Bomb, new Vector3(), new Quaternion()) as GameObject;
                bomb.transform.parent = Level.transform;
                bomb.transform.localPosition = new Vector3(Mathf.RoundToInt(bombTile.x) * tileSize, Mathf.RoundToInt(bombTile.y) * tileSize);
                StartCoroutine(Bombing());
            }
        }

        IEnumerator Bombing()
        {
            yield return new WaitForSeconds(0.5f);
            _bombing = false;
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

            var angle = Vector2.Angle(rigidbody2D.velocity, new Vector2(1, 0));
            var cross = Vector3.Cross(rigidbody2D.velocity, new Vector2(1, 0));
            if (cross.z > 0)
                angle = 360 - angle;

            _animator.SetBool("Bombing", _bombing);
            if (!_bombing)
            {
                if (Mathf.Abs(rigidbody2D.velocity.magnitude) < 0.1f)
                    _animator.SetInteger("Move", 0);
                else
                {
                    if (angle > 315 || angle <= 45)
                        _animator.SetInteger("Move", 2);
                    if (angle > 45 && angle <= 135)
                        _animator.SetInteger("Move", 1);
                    if (angle > 135 && angle <= 225)
                        _animator.SetInteger("Move", 4);
                    if (angle > 225 && angle <= 315)
                        _animator.SetInteger("Move", 3);
                }
            }

            Debug.Log(angle);
        }
    }
}
