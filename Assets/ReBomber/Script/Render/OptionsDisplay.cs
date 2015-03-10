using System;
using Assets.Script;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class OptionsDisplay: ContainerBase
    {
        Messenger _messenger;
        GameModel _level;
        BombermanModel _bomberman;
        private readonly List<Action> _subscriptions = new List<Action>();

        public GameObject Panel;
        public AudioSource Music;

        protected override void Start()
        {
            base.Start();
        }

        private void OnInjected(Messenger messenger, GameModel level, BombermanModel bomberman)
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
            ToggleMenu();
        }

        public void ToggleMusic()
        {
            if (Music.isPlaying)
                Music.Pause();
            else
                Music.Play();
        }

        public void Restart()
        {
            ToggleMenu();
            _level.Reset();
            _bomberman.Reload();
            _bomberman.Reset();
            _messenger.Signal(Signals.DoorOpened);
        }

        public void Quit()
        {
            Application.Quit();
        }

        void ToggleMenu()
        {
            Panel.SetActive(!Panel.activeSelf);
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }

        void Update()
        {
            if (Input.GetButtonDown("Options") || Input.GetButtonDown("Joystick Options"))
                ToggleMenu();
        }
    }
}

