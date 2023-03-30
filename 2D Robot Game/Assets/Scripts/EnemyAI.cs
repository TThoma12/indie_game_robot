using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private Rigidbody2D rb;
    private RaycastHit2D ray2d;
    public bool isMoving;
    private bool reachedTarget;
    private float speed = 4f;
    private float arrived;
    public Rigidbody2D target;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate() {
        if (isMoving) {
            arrived = Vector2.Distance(rb.position, target.position);
            if (arrived <= 0.7f) {
                target.gameObject?.GetComponent<PlayerController>()?.Attack();
            } else reachedTarget = false;

            if (!reachedTarget) {
                Vector2 rayDir = target.position - rb.position;
                ray2d = Physics2D.Raycast(rb.position, rayDir, 2f);
                if (ray2d.collider != null) {
                    if (ray2d.distance <= 0.5f) {
                        rayDir = Vector2.Perpendicular(rayDir);
                        rb.MovePosition(Vector2.MoveTowards(rb.position, rb.position + rayDir, speed * Time.fixedDeltaTime));
                        Debug.DrawRay(rb.position, rayDir);
                    }
                } else {
                    rb.MovePosition(Vector2.MoveTowards(rb.position, rb.position + rayDir, speed * Time.fixedDeltaTime));
                    Debug.DrawRay(rb.position, rayDir);
                }
            } else {
                isMoving = false;
            }
        }
    }

}
