using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip hover;
    public AudioClip pressed;

    public void HoverSound()
    {
        myFX.PlayOneShot(hover, 1f);
    }


    public void PressedSound()
    {
        myFX.PlayOneShot(pressed, 1f);
    }
}
