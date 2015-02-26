using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{

    public enum StartSelection
    {
        Easy,
        Normal,
        Credits,
        Exit
    }

    public class StartScene : ContainerBase
    {
        private Messenger _messenger;
        private GameModel _gameModel;
        public StartSelection Selection;
        private TextMeshPro _textMeshPro;

        // Use this for initialization
        protected override void Start()
        {
            Screen.SetResolution(1280, 720, false);
            base.Start();
            _textMeshPro = GetComponent<TextMeshPro>();
        }

        private void OnInjected(Messenger messenger, GameModel gameModel)
        {
            _messenger = messenger;
            _gameModel = gameModel;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("return") || Input.GetButtonDown("Joystick Start"))
            {
                switch (Selection)
                {
                    case StartSelection.Easy:
                        _gameModel.IsEasy = true;
                        _messenger.Signal(Signals.DoorOpened);
                        break;
                    case StartSelection.Normal:
                        _messenger.Signal(Signals.DoorOpened);
                        break;
                    case StartSelection.Credits:
                        _messenger.Signal(Signals.Credits);
                        break;
                    case StartSelection.Exit:
                        Application.Quit();
                        break;
                }
            }
            if (Input.GetKeyDown("up") || Input.GetAxis("Joystick Vertical") > 0)
                Selection = Selection > 0 ? Selection - 1 : Selection;
            if (Input.GetKeyDown("down") || Input.GetAxis("Joystick Vertical") < 0)
                Selection = Selection < StartSelection.Exit ? Selection + 1 : Selection;
        }

        void FixedUpdate()
        {
            switch (Selection)
            {
                case StartSelection.Easy:
                    _textMeshPro.text = @">>Easy\n Normal\n Gredits\n Exit";
                    break;
                case StartSelection.Normal:
                    _textMeshPro.text = @" Easy\n>>Normal\n Gredits\n Exit";
                    break;
                case StartSelection.Credits:
                    _textMeshPro.text = @" Easy\n Normal\n>>Gredits\n Exit";
                    break;
                case StartSelection.Exit:
                    _textMeshPro.text = @" Easy\n Normal\n Gredits\n>>Exit";
                    break;
            }
        }
    }
}