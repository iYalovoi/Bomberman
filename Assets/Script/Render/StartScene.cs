using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class StartScene : ContainerBase
    {
        private Messenger _messenger;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }


        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("return"))
                _messenger.Signal(Signals.LoadNextLevel);
        }
    }
}
