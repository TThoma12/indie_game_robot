using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateBehavior : MonoBehaviour
{
    public float pistolBullets;
    public float rifleBullets;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.currentPistolAmmo += pistolBullets;
            player.currentRifleAmmo += rifleBullets;
        }

        Destroy(gameObject);
    }
}
