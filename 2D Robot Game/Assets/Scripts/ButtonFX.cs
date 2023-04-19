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
        AudioManager.Instance.PlaySFX("UINavigation");
    }


    public void PressedSound()
    {
        if (this.gameObject.name.Equals("Start Button"))
        {
            AudioManager.Instance.PlaySFX("UIStart");
        }
        else
        {
            AudioManager.Instance.PlaySFX("UISelect");
        }
    }
}
