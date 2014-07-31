using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script
{
    public class ContainerLauncher : MonoBehaviour
    {
        private Container _container;

        void Awake()
        {
            ImmortalBook.Add(this);
        }

        void Start()
        {
            _container = new Container();

            _container.Single<BombermanModel, BombermanModel>();
            _container.Single<Messenger, Messenger>();
        }
    }
}