using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CameraController cameraFollow;

    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private TrailRenderer trailRenderer;

    public float playerSpeed;

    private float normalSpeed = 10.0f;
    private float dashSpeed = 20.0f;

    private Rigidbody2D playerRb;

    private float dashLength = 0.3f;
    private float dashCooldownTime = 0.5f;

    private float dashCounter;
    private float dashCoolCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerSpeed = normalSpeed;
    }

    // Update is called once per frame
    private void Update()
    {

        MovePlayer();
        LookAtMouse();
        HandleInteractions();
    }

    private void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Gets input from player
        Vector2 input = new Vector2(x: horizontalInput, y: verticalInput);

        // Moves player based on input
        transform.Translate(input * playerSpeed * Time.deltaTime, Space.World);

        // If player presses shift, and both counters are timed out, player can dash. We reset the timer 
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                playerSpeed = dashSpeed;
                dashCounter = dashLength;
                trailRenderer.emitting = true;
            }
        }

        // While the dash timer is greater than 0, we update the timer. 
        if(dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            //If it is runs out during that frame, player dash stops, and we set the cooldown timer
            if (dashCounter <= 0 )
            {
                playerSpeed = normalSpeed;
                dashCoolCounter = dashCooldownTime;
                trailRenderer.emitting = false;
            }
        }

        // While the cooldown timer is greater than 0, we update it
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void LookAtMouse()
    {
        // Get mouse position
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Player faces mouse
        this.transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }
    //TriggerShake

    private void HandleInteractions() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            cameraFollow.TriggerShake();
        }
    }

    private void Dash()
    {
        
    }
}
