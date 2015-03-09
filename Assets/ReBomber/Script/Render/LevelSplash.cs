using System.Collections;
using Assets.Script.Utility;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Script
{
    public class LevelSplash : ContainerBase
    {
        private GUIStyle _textStyle;
        private GameModel _model;
        private Messenger _messenger;
        private TextMeshPro _textMeshPro;

        public UI2DSprite Hint;

        public Sprite Controls;
        public Sprite Flames;
        public Sprite Bombs;
        public Sprite BombsPass;
        public Sprite Detonator;
        public Sprite FlamePass;
        public Sprite Mystery;
        public Sprite Speed;
        public Sprite WallPass;

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (Input.anyKey)
                _messenger.Signal(Signals.LoadNextLevel);
        }

        private void OnInjected(Messenger messenger, GameModel model)
        {
            var sprites = new List<Sprite>
            {
                Controls,
                Flames,
                Bombs,
                BombsPass,
                Detonator,
                FlamePass,
                Mystery,
                Speed,
                WallPass,
            };

            _model = model;
            _messenger = messenger;

            _textMeshPro = GetComponent<TextMeshPro>();
            _textMeshPro.text = string.Format("Level {0}", _model.CurrentLevel);

            switch (_model.CurrentLevel)
            {
                case 1:
                    Hint.sprite2D = Controls;
                    break;
                case 2:
                    Hint.sprite2D = Bombs;
                    break;
                case 3:
                    Hint.sprite2D = Detonator;
                    break;
                case 4:
                    Hint.sprite2D = Speed;
                    break;
                case 7:
                    Hint.sprite2D = Flames;
                    break;
                case 9:
                    Hint.sprite2D = BombsPass;
                    break;
                case 10:
                    Hint.sprite2D = WallPass;
                    break;
                case 26:
                    Hint.sprite2D = Mystery;
                    break;
                case 30:
                    Hint.sprite2D = FlamePass;
                    break;
                default:
                    Hint.sprite2D = sprites[Random.Range(0, sprites.Count)];
                    break;
            }
        }
    }
}