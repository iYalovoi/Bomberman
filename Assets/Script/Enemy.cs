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

        protected Animator Animator;

        // Use this for initialization
        void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public void Die()
        {
            Dead = true;
            Animator.SetTrigger("Die");
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
