using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class Bomb : MonoBehaviour
    {
        public int Radius;

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var colider = gameObject.GetComponent<CircleCollider2D>();
                colider.isTrigger = false;
            }
        }

        void Start()
        {
            StartCoroutine(Wait(3));
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            var animator = GetComponent<Animator>();
            animator.SetFloat("Radius", Radius);
            animator.SetTrigger("Explode");
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        void Update()
        {

        }
    }
}
