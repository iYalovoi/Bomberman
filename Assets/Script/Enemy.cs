using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class Enemy : MonoBehaviour, ITarget
    {
        public float MaxSpeed = 1f;
        public bool Dead = false;

        protected Animator animator;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Die()
        {
            Dead = true;
            animator.SetTrigger("Die");
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
