using System.Collections;
using Assets.Script.Level;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class LevelSplash : ContainerBase
    {
        private GUIStyle _textStyle;
        private LevelModel _model;
        private Messenger _messenger;

        protected override void Start()
        {
            base.Start();
            _textStyle = new GUIStyle { fontSize = 96 };

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
        }

        void Update()
        {
        }

        void OnGUI()
        {
            GUI.Label(new Rect(Screen.height / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), string.Format("Level {0}", _model.CurrentLevel), _textStyle);
        }
    }
}
