using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    private float xBound = 10.8f;
    private float yBound = 5.2f;

    private bool isPistolBullet;

    public float bulletDamage;

    private float pistolDamage = 8f;
    private float rifleDamage = 15f;

    private Vector2 initialPosition;

    private GameObject player;
    private BoxCollider2D bulletCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player (Placeholder)");
        bulletCollider = GetComponent<BoxCollider2D>();
        bulletCollider.enabled = false;

        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        DestroyOutOfBounds();

        float distanceFromShooter = Vector2.Distance(initialPosition, transform.position);

        if (distanceFromShooter > 1)
        {
            bulletCollider.enabled = true;
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }

    private void DestroyOutOfBounds()
    {
        float distanceFromInitialPos = Vector2.Distance(initialPosition, transform.position);
        if (distanceFromInitialPos > 20f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}