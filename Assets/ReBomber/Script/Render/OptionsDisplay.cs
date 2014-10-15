using System;
using Assets.Script;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class OptionsDisplay: ContainerBase
    {
        Messenger _messenger;
        private readonly List<Action> _subscriptions = new List<Action>();

        public GameObject Panel;

        protected override void Start()
        {
            base.Start();
        }

        private void OnInjected(Messenger messenger)
        {
            _messenger = messenger;
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
            if (Input.GetButtonDown("Options"))
                ToogleMenu();
        }
    }
}

