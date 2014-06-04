using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class Bomb : MonoBehaviour
    {
        public int Radius;
        public GameObject VerticalBlast;
        public GameObject HorizontalBlast;
        public GameObject Level;

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
            if (Level != null)
                StartCoroutine(Wait(3));
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            var tileSize = renderer.bounds.size.x;
            var blastEdge = 1 + Radius * 2;
            var localPosition = gameObject.transform.localPosition;
            var bombTile = new Vector2(localPosition.x / tileSize, localPosition.y / tileSize);
            for (var i = 0; i < blastEdge; i++)
            {
                if (i != Radius)
                {
                    var blast = Instantiate(HorizontalBlast, new Vector3(), new Quaternion()) as GameObject;
                    blast.transform.parent = Level.transform;
                    blast.transform.localPosition = new Vector3((Mathf.RoundToInt(bombTile.x) - Radius + i) * tileSize, Mathf.RoundToInt(bombTile.y) * tileSize);
                }
            }
            for (var i = 0; i < blastEdge; i++)
            {
                if (i != Radius)
                {
                    var blast = Instantiate(VerticalBlast, new Vector3(), new Quaternion(0, 0, 0, 0)) as GameObject;
                    blast.transform.parent = Level.transform;
                    blast.transform.localPosition = new Vector3(Mathf.RoundToInt(bombTile.x) * tileSize, (Mathf.RoundToInt(bombTile.y) - Radius + i) * tileSize);
                    blast.transform.Rotate(0, 0, 90);
                }
            }
            var animator = GetComponent<Animator>();
            animator.SetFloat("Radius", Radius);
            animator.SetTrigger("Explode");
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        void Update()
        {

        }
    }
}
