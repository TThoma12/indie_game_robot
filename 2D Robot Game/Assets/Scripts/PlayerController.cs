using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] CameraController cameraFollow;
    public float _speed = 10.0f;

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        LookAtMouse();
        HandleInteractions();
    }

    private void MovePlayer()
    {
        // Gets input from player
        Vector2 input = new Vector2(x: Input.GetAxis("Horizontal"), y: Input.GetAxis("Vertical"));

        // Moves player based on input
        transform.Translate(input * _speed * Time.deltaTime, Space.World);
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
}
