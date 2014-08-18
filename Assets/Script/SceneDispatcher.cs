using System.Collections;
using Assets.Script.Level;
using UnityEngine;
using Assets.Script.Utility;

namespace Assets.Script
{
    public class SceneDispatcher
    {
        private readonly Messenger _messenger;
        private LevelModel _model;
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
        }

        private void RestartLevelHandler()
        {
            _dispatcher.Dispatch(() => LoadNextLevelCoroutine(_model.CurrentLevel));
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
            _model.CurrentLevel++;
            Application.LoadLevel("LevelSplash");
        }

        private void GameOverHandler()
        {
            Application.LoadLevel("GameOver");
        }
    }
}