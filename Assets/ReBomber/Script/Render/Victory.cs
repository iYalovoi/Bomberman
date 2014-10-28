using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{
    public class Victory : ContainerBase
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
            GetComponent<TextMeshPro>().text = string.Format("Score: {0}!", bomberman.Score);

            _messenger = messenger;
            level.Reset();
            bomberman.Reload();
            bomberman.Reset();
        }

        void Update()
        {
            if (Input.GetKeyDown("return") || Input.GetButtonDown("Joystick Start"))
                _messenger.Signal(Signals.RestartGame);
        }

        void OnGUI()
        {
            //GUI.Label(new Rect(Screen.height / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), "Game Over", _textStyle);
        }
    }
}
