using UnityEngine;
using System.Collections;

public class Soft : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void Explode()
    {
        _animator.SetTrigger("Explode");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
