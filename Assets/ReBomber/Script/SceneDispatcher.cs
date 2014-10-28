using System.Collections;
using UnityEngine;
using Assets.Script.Utility;

namespace Assets.Script
{
    public class SceneDispatcher
    {
        private readonly Messenger _messenger;
        private readonly LevelModel _model;
        private readonly IDispatcher _dispatcher;

        public SceneDispatcher(Messenger messenger, LevelModel model, IDispatcher dispatcher)
        {
            _model = model;
            _dispatcher = dispatcher;

            _messenger = messenger;
            _messenger.Subscribe(Signals.GameOver, GameOverHandler);
            _messenger.Subscribe(Signals.DoorOpened, LoadLevelSplash);
            _messenger.Subscribe(Signals.LoadNextLevel, LoadNextLevelHandler);
            _messenger.Subscribe(Signals.RestartLevel, RestartLevelHandler);
            _messenger.Subscribe(Signals.RestartGame, RestartGameHandler);
            _messenger.Subscribe(Signals.Credits, CreditsHandler);
        }

        private void CreditsHandler()
        {
            Application.LoadLevel("Credits");
        }

        private void RestartGameHandler()
        {
            Application.LoadLevel("Start");
        }

        private void RestartLevelHandler()
        {
            Application.LoadLevel("LevelSplash");
        }

        private void LoadNextLevelHandler()
        {
            _dispatcher.Dispatch(() => LoadNextLevelCoroutine(_model.CurrentLevel));
        }

        private IEnumerator LoadNextLevelCoroutine(int level)
        {
            Application.LoadLevel("Battle");
            yield return new WaitForSeconds(0);
            var levelFactory = Object.FindObjectOfType<LevelFactory>();
            levelFactory.ProduceLevel(level);
        }

        private void LoadLevelSplash()
        {
            if (_model.CurrentLevel < _model.LevelCap)
            {
                _model.CurrentLevel++;
//                _model.CurrentLevel = Random.Range(0, _model.LevelCap);
                GA.API.Design.NewEvent("Level", _model.CurrentLevel);
                Application.LoadLevel("LevelSplash");
            }
            else
                Application.LoadLevel("Victory");
        }

        private void GameOverHandler()
        {
            Application.LoadLevel("GameOver");
        }
    }
}