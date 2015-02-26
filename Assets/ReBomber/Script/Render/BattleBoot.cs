using UnityEngine;
using System.Collections;


namespace Assets.Script
{
    public class BattleBoot : ContainerBase
    {

        private void OnInjected(Messenger _messenger, GameModel model)
        {
            if (model.CurrentLevel == 0)
                _messenger.Signal(Signals.DoorOpened);
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}