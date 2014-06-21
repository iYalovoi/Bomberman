using UnityEngine;

namespace Assets.Script
{
    public class Door : MonoBehaviour, ITarget
    {

        void Start()
        {

        }

        void Update()
        {
        }

        void Opened()
        {
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                Application.LoadLevel("Battle");
        }

        public void OnHit(GameObject striker)
        {
        }
    }
}
