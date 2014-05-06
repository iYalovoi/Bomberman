using UnityEngine;

namespace Assets.Script
{
    public class Bomb : MonoBehaviour
    {

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var colider = gameObject.GetComponent<CircleCollider2D>();
                colider.isTrigger = false;
            }
        }

        void Start()
        {
            Destroy(gameObject, 3);
        }

        void Update()
        {

        }
    }
}
