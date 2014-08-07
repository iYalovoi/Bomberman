using UnityEngine;

namespace Assets.Script
{
    public class GameOver : MonoBehaviour
    {
        private GUIStyle _textStyle;

        void Start()
        {
            _textStyle = new GUIStyle { fontSize = 96 };
        }

        void Update()
        {
        }

        void OnGUI()
        {
            GUI.Label(new Rect(Screen.height / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), "Game Over", _textStyle);
        }
    }
}
