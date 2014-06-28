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
            if (col.gameObject.tag == "Player" && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                _animator.SetTrigger("Open");
                var bomberman = col.gameObject.GetComponent<Bomberman>();
                bomberman.Restrained = true;
            }                
        }

        public void OnHit(GameObject striker)
        {
        }
    }
}
