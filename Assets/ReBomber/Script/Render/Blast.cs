using UnityEngine;

namespace Assets.Script
{
    public class Blast : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        private void Destory()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            var components = col.gameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component is ITarget)
                    (component as ITarget).OnHit(gameObject);
            }
        }
    }
}
