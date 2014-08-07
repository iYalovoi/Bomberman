using System.Collections;
using System.Globalization;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class BattleHUD : ContainerBase
    {
        private LevelFactory _levelFactory;
        private GUIStyle _textStyle;
        private int _timeLeft;
        private BombermanModel _model;
        private Messenger _messenger;

        void Awake()
        {
        }

        private void OnInjected(BombermanModel model, Messenger messenger)
        {
            _model = model;
            _messenger = messenger;
        }

        protected override void Start()
        {
            base.Start();

            //DI Unity way; Shitty way; Igor.
            _levelFactory = FindObjectOfType<LevelFactory>();
            _textStyle = new GUIStyle { fontSize = 40 };            

            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {            
            yield return new WaitForSeconds(0);
            _timeLeft = _levelFactory.CurrentLevelDefinition.TimeLimit;
            while (_timeLeft > 0)
            {
                _timeLeft--;
                yield return new WaitForSeconds(1);
                if(_timeLeft == 0)
                    _messenger.Signal(Signals.SpawnPontans);
            }
        }

        void Update()
        {
        }

        void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 100, 40), _timeLeft > 0 ? _timeLeft.ToString(CultureInfo.InvariantCulture) : "Run!", _textStyle);
            GUI.Label(new Rect(140, 20, 100, 40), _model.Lifes.ToString(CultureInfo.InvariantCulture), _textStyle);
        }
    }
}