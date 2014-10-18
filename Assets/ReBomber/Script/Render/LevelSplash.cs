using System.Collections;
using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    public class LevelSplash : ContainerBase
    {
        private GUIStyle _textStyle;
        private LevelModel _model;
        private Messenger _messenger;
        private TextMeshPro _textMeshPro;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(WaitAndLoadNewLevel());
        }

        private IEnumerator WaitAndLoadNewLevel()
        {
            yield return new WaitForSeconds(3);
            _messenger.Signal(Signals.LoadNextLevel);
        }

        private void OnInjected(Messenger messenger, LevelModel model)
        {
            _model = model;
            _messenger = messenger;

            _textMeshPro = GetComponent<TextMeshPro>();
            _textMeshPro.text = string.Format("Level {0}", _model.CurrentLevel);
        }

        void Update()
        {
        }
    }
}
