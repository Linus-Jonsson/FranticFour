using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AudioSource>().PlayDelayed(2.3f); //FIX: Magic number...
    }
}
