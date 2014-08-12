using Assets.Script.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Script
{

    public enum StartSelection
    {
        Game,
        Credits,
        Exit
    }

    public class StartScene : ContainerBase
    {
        private Messenger _messenger;
        public StartSelection Selection;
        private TextMeshPro _textMeshPro;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            _textMeshPro = GetComponent<TextMeshPro>();
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
            if (Input.GetKeyDown("up"))
                Selection = Selection > 0 ? Selection - 1 : Selection;
            if (Input.GetKeyDown("down"))
                Selection = Selection < StartSelection.Exit ? Selection + 1 : Selection;
        }

        void FixedUpdate()
        {
            switch (Selection)
            {
                case StartSelection.Game:
                    _textMeshPro.text = @">>>>>>Game\n\tGredits\n\tExit";
                    break;
                case StartSelection.Credits:
                    _textMeshPro.text = @"\tGame\n>>>>>>Gredits\n\tExit";
                    break;
                case StartSelection.Exit:
                    _textMeshPro.text = @"\tGame\n\tGredits\n>>>>>>Exit";
                    break;
            }
        }
    }
}
