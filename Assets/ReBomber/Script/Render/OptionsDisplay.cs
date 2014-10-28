using System;
using Assets.Script;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class OptionsDisplay: ContainerBase
    {
        Messenger _messenger;
        LevelModel _level;
        BombermanModel _bomberman;
        private readonly List<Action> _subscriptions = new List<Action>();

        public GameObject Panel;

        protected override void Start()
        {
            base.Start();
        }

        private void OnInjected(Messenger messenger, LevelModel level, BombermanModel bomberman)
        {
            _messenger = messenger;
            _level = level;
            _bomberman = bomberman;
        }

        private void OnDestroy()
        {
            _subscriptions.ForEach(o => o());
        }

        public void Resume()
        {
            ToogleMenu();
        }

        public void Restart()
        {
            ToogleMenu();
            _level.Reset();
            _bomberman.Reload();
            _bomberman.Reset();
            _messenger.Signal(Signals.DoorOpened);
        }

        public void Quit()
        {
            Application.Quit();
        }

        void ToogleMenu()
        {
            Panel.SetActive(!Panel.activeSelf);
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }

        void Update()
        {
            if (Input.GetButtonDown("Options") || Input.GetButtonDown("Joystick Options"))
                ToogleMenu();
        }
    }
}

