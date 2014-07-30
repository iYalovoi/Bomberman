using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Assets.Script
{
    public class UI : MonoBehaviour
    {
        private LevelFactory _levelFactory;
        private GUIStyle _textStyle;
        private int _timeLeft;

        IEnumerator Start()
        {
            //DI Unity way; Shitty way; Igor.
            _levelFactory = FindObjectOfType<LevelFactory>();
            _textStyle = new GUIStyle { fontSize = 40 };
            _timeLeft = _levelFactory.CurrentLevel.TimeLimit;

            while (true)
            {
                _timeLeft--;
                yield return new WaitForSeconds(1);
            }
        }

        void Update()
        {
        }

        void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 100, 40), _timeLeft > 0 ? _timeLeft.ToString(CultureInfo.InvariantCulture) : "Run!", _textStyle);
        }
    }
}