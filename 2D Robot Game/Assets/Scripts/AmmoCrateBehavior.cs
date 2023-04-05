using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class AmmoCrateBehavior : MonoBehaviour
{
    public float pistolBullets;
    public float rifleBullets;

    public PlayerController player;

    public AudioClip rifleRefillSound;
    public AudioClip pistolRefillSound;
    public AudioSource crateAudio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            HandlePistolBullets();
            HandleRifleBullets();

            if(pistolBullets > 0)
            {
                crateAudio.PlayOneShot(pistolRefillSound, 1f);
            }
            if(rifleBullets > 0)
            {
                crateAudio.PlayOneShot(rifleRefillSound, 1f);
            }
        }

        Destroy(gameObject);
    }

    private void HandlePistolBullets()
    {
        if (player.currentPistolAmmo + pistolBullets + player.currentPistolMag <= player.pistolMagSize + player.pistolAmmoSize)
        {
            player.currentPistolAmmo += pistolBullets;

            while (player.currentPistolAmmo > player.pistolAmmoSize)
            {
                player.currentPistolAmmo -= player.pistolAmmoSize;
                if (player.currentPistolMag + player.pistolAmmoSize <= player.pistolMagSize)
                {
                    player.currentPistolMag += player.pistolAmmoSize;
                }
                else
                {
                    player.currentPistolMag = player.pistolMagSize;
                }
            }
        }
        else
        {
            player.currentPistolAmmo = player.pistolAmmoSize;
            player.currentPistolMag = player.pistolMagSize;
        }
    }

    private void HandleRifleBullets()
    {
        if (player.currentRifleAmmo + rifleBullets + player.currentRifleMag <= player.rifleMagSize + player.rifleAmmoSize)
        {
            player.currentRifleAmmo += rifleBullets;

            while (player.currentRifleAmmo > player.rifleAmmoSize)
            {
                player.currentRifleAmmo -= player.rifleAmmoSize;
                if (player.currentRifleMag + player.rifleAmmoSize <= player.rifleMagSize)
                {
                    player.currentRifleMag += player.rifleAmmoSize;
                }
                else
                {
                    player.currentRifleMag = player.rifleMagSize;
                }
            }
        }
        else
        {
            player.currentRifleAmmo = player.rifleAmmoSize;
            player.currentRifleMag = player.rifleMagSize;
        }
    }
}
