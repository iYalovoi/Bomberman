using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Script
{
    public class PowerUpsDisplay : ContainerBase
    {
        Messenger _messenger;
        private readonly List<Action> _subscriptions = new List<Action>();
        BombermanModel _model;

        public GameObject Fire;
        public GameObject Bomb;
        public GameObject Remote;
        public GameObject Speed;
        public GameObject WallPass;
        public GameObject FirePass;
        public GameObject BombPass;
        public GameObject Immortal;

        protected override void Start()
        {
            base.Start();
        }

        private void OnInjected(Messenger messenger, BombermanModel model)
        {
            _model = model;
            _messenger = messenger;
            _subscriptions.Add(_messenger.Subscribe<Powers>((o) => UpdateHUD()));
            UpdateHUD();
        }

        void UpdateHUD()
        {
            Fire.SetActive(_model.Radius > 1 ? true : false);
            Bomb.SetActive(_model.BombCount > 1 ? true : false);
            Speed.SetActive(_model.Speed > Constants.BasePlayerSpeed ? true : false);
            BombPass.SetActive(_model.BombPass);
            FirePass.SetActive(_model.FlamePass);
            Immortal.SetActive(_model.Invincible);
            Remote.SetActive(_model.RemoteControl);
            WallPass.SetActive(_model.WallPass);
        }

        private void OnDestroy()
        {
            _subscriptions.ForEach(o => o());
        }

        void Update()
        {
	
        }
    }
}