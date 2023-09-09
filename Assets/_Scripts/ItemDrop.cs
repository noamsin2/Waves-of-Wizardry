using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private AudioClip pendantDrop;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        AudioSource.PlayClipAtPoint(pendantDrop, transform.position, 1f);
        rb.AddForce(new Vector2(0, 2));
    }
}
