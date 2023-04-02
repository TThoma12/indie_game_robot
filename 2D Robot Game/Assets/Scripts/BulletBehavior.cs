using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 0.5f;
    private float xBound = 10.8f;
    private float yBound = 5.2f;

    public bool isPlayerBullet;

    private Vector2 initialPosition;

    private GameObject player;
    private CircleCollider2D bulletCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player (Placeholder)");
        bulletCollider = GetComponent<CircleCollider2D>();
        bulletCollider.enabled = false;

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        DestroyOutOfBounds();

        float distanceFromShooter = Vector2.Distance(initialPosition, transform.position);

        if (distanceFromShooter > (player.GetComponent<BoxCollider2D>().size.magnitude) + (bulletCollider.radius / 2))
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
        if (Mathf.Abs(transform.position.x) > xBound)
        {
            Destroy(gameObject);
        }
        else if (Mathf.Abs(transform.position.y) > yBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}