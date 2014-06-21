using UnityEngine;

namespace Assets.Script
{
    public class Door : MonoBehaviour, ITarget
    {
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
        }

        void Opened()
        {
            Application.LoadLevel("Battle");
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                _animator.SetTrigger("Open");                
        }

        public void OnHit(GameObject striker)
        {
        }
    }
}
