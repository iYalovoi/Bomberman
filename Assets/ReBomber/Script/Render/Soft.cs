using UnityEngine;

namespace Assets.Script
{
    public class Soft : ContainerBase, ITarget
    {
        private Animator _animator;


        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
        }

        private Messenger _messenger;

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
        }

        public void Explode()
        {
            _animator.SetTrigger("Explode");
            _messenger.Signal(Signals.SoftBlockDestroyed);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void OnHit(GameObject striker)
        {
            Explode();
        }
    }
}
