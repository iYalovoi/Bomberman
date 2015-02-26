using System.Collections;
using System.Globalization;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class BattleHUD : ContainerBase
    {
        private LevelFactory _levelFactory;
        private GUIStyle _textStyleRed;
        private GUIStyle _textStyleGreen;
        GameModel _gameModel;
        private BombermanModel _model;
        private Messenger _messenger;

        public Font UIFont;
        public Texture _frame;


        void Awake()
        {
        }

        private void OnInjected(BombermanModel model, GameModel level, Messenger messenger)
        {
            _gameModel = level;
            _model = model;
            _messenger = messenger;
        }

        protected override void Start()
        {
            base.Start();

            //DI Unity way; Shitty way; Igor.
            _levelFactory = FindObjectOfType<LevelFactory>();
            _textStyleRed = new GUIStyle { fontSize = 20 };
            _textStyleRed.normal.textColor = Color.red;
            _textStyleRed.font = UIFont;
            _textStyleRed.alignment = TextAnchor.MiddleCenter;

            _textStyleGreen = new GUIStyle { fontSize = 20 };
            _textStyleGreen.normal.textColor = Color.green;
            _textStyleGreen.font = UIFont;
            _textStyleGreen.alignment = TextAnchor.MiddleCenter;

            if (!_gameModel.IsEasy)
                StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {            
            _messenger.Signal(Signals.CountdownStart);
            yield return new WaitForSeconds(0);
            _gameModel.Time = _levelFactory.levDef.TimeLimit;
            while (_gameModel.Time > 0)
            {
                _gameModel.Time--;
                yield return new WaitForSeconds(1);
                if (_gameModel.Time == 0)
                    _messenger.Signal(Signals.CountdownOver);
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            }
        }

        void OnGUI()
        {
//            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _frame);
            var relativeUnitX = Screen.width / 1280f;
            var relativeUnitY = Screen.height / 720f;

            GUI.Label(new Rect(relativeUnitX * 53, relativeUnitY * 44, relativeUnitX * 100, relativeUnitY * 23), _model.Lifes.ToString(CultureInfo.InvariantCulture), _textStyleGreen);
            GUI.Label(new Rect(relativeUnitX * 187, relativeUnitY * 44, relativeUnitX * 100, relativeUnitY * 23), _gameModel.Time > 0 ? _gameModel.Time.ToString(CultureInfo.InvariantCulture) : "Run!", _textStyleRed);
            GUI.Label(new Rect(relativeUnitX * 322, relativeUnitY * 44, relativeUnitX * 100, relativeUnitY * 23), _gameModel.CurrentLevel.ToString(CultureInfo.InvariantCulture), _textStyleGreen);
            GUI.Label(new Rect(relativeUnitX * 858, relativeUnitY * 44, relativeUnitX * 87, relativeUnitY * 23), _model.Score.ToString(CultureInfo.InvariantCulture), _textStyleGreen);

        }
    }
}