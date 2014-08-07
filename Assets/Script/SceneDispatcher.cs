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
        private LevelFactory _levelFactory;

        public SceneDispatcher(Messenger messenger, LevelModel model)
        {
            _model = model;

            _messenger = messenger;
            _messenger.Subscribe(Signals.GameOver, GameOverHandler);
            _messenger.Subscribe(Signals.DoorOpened, DoorOpenedHandler);
            _messenger.Subscribe(Signals.LoadNextLevel, LoadNextLevelHandler);
            _messenger.Subscribe(Signals.RestartLevel, RestartLevelHandler);

            _levelFactory = Object.FindObjectOfType<LevelFactory>();
        }

        private void RestartLevelHandler()
        {
            _levelFactory.StartCoroutine(LoadNextLevelCoroutine(_model.CurrentLevel));
        }

        private void LoadNextLevelHandler()
        {
            _levelFactory.StartCoroutine(LoadNextLevelCoroutine(_model.CurrentLevel));
        }

        private IEnumerator LoadNextLevelCoroutine(int level)
        {
            Application.LoadLevel("Battle");
            yield return new WaitForSeconds(0);
            _levelFactory.ProduceLevel(level);
        }

        private void DoorOpenedHandler()
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