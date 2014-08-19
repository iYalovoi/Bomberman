using System.Collections;
using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    public class Credits : ContainerBase
    {
        private Messenger _messenger;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(JumpToMain());
        }

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
        }

        private IEnumerator JumpToMain()
        {
            yield return new WaitForSeconds(20);
            _messenger.Signal(Signals.RestartGame);
        }

        void Update()
        {
        }
    }
}
