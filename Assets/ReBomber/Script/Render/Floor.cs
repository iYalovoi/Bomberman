using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{
    public Sprite[] sprites;
    // Use this for initialization
    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
