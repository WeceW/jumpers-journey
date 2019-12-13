using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProps : MonoBehaviour
{
    public int points;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
