using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthImageController : MonoBehaviour
{
    public Sprite[] playerHealthSprites;

    public PlayerController playerController;
    public Image image;

    private float healthValue;

    // Start is called before the first frame update
    void Start()
    {

        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthValue = playerController.playerHP;

        if (healthValue > 75)
        {
            image.sprite = playerHealthSprites[0];
        }
        else if (healthValue > 50)
        {
            image.sprite = playerHealthSprites[1];
        }
        else if (healthValue > 25)
        {
            image.sprite = playerHealthSprites[2];
        }
        else if (healthValue > 0)
        {
            image.sprite = playerHealthSprites[3];
        }
        else
        {
            image.sprite = playerHealthSprites[4];
        }
    }
}
