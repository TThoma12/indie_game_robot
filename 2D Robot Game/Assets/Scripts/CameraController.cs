using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 cameraOffset = new Vector3(0, 0, -10);

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;
    private float dampingSpeed = 1.0f;

    // Update is called once per frame
    void LateUpdate() {
        if (shakeDuration > 0) {
            transform.position = player.transform.position + cameraOffset + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        } else {
            shakeDuration = 0f;
            this.transform.position = player.transform.position + cameraOffset;
        }
    }

    public void TriggerShake() {
        shakeDuration = 0.2f;
    }
}
