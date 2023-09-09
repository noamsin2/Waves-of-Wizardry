using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    private int amount;
    
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dropSound;
    //private AudioSource audioSource;
    private void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {

        //audioSource.volume = AudioManager.Instance.GetEffectsVolume();
        //audioSource.Play();
        AudioSource.PlayClipAtPoint(dropSound, transform.position, AudioManager.Instance.GetFinalVolume());
        rb.AddForce(new Vector2(0, 2));
    }
    
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    public int GetAmount()
    {
        AudioSource.PlayClipAtPoint(pickupSound, transform.position, AudioManager.Instance.GetFinalVolume());
        return amount;
    }
}
