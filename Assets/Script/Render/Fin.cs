using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    public class Fin : ContainerBase
    {

        private TextMeshPro _textMeshPro;
        private Messenger _messenger;

        protected override void Start()
        {
            base.Start();
            _textMeshPro = GetComponent<TextMeshPro>();
        }

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
        }

        void Update()
        {
        }
    }
}
