using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    public class GameOver : ContainerBase
    {
        private TextMeshPro _textMeshPro;
        private Messenger _messenger;

        protected override void Start()
        {
            base.Start();
            _textMeshPro = GetComponent<TextMeshPro>();
        }

        private void OnInjected(Messenger messenger, LevelModel level, BombermanModel bomberman)
        {
            _messenger = messenger;
            level.Reset();
            bomberman.Reload();
        }

        void Update()
        {
            if (Input.GetKeyDown("return"))
                _messenger.Signal(Signals.RestartGame);
        }

        void OnGUI()
        {
            //GUI.Label(new Rect(Screen.height / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), "Game Over", _textStyle);
        }
    }
}
