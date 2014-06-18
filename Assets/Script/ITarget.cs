using UnityEngine;

namespace Assets.Script
{
    public interface ITarget
    {
        void OnHit(GameObject striker);
    }
}