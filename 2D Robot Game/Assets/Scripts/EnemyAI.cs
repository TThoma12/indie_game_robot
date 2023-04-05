using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.XR;
using UnityEditor.UI;

public class EnemyAI : MonoBehaviour {
    public GameObject bulletPrefab;

    // The targets where we want the enemy to travel
    public Transform[] targets;

    private GameObject player;

    public int targetIndex = 0;
    
    public float speed = 5f;

    //public bool isPatrolling = true;
    public bool canFire = true;

    [SerializeField] private float enemyHP = 100f;
    [SerializeField] private float damage = 5f;

    [SerializeField] PlayerController playerController;

    private Animator anim;
    private string DYING_ANIMATION = "IsDying";

    // Rigidbody for enemy
    Rigidbody2D rb;

    private bool PlayerOnRange = false;

    //// Start is called before the first frame update
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player (Placeholder)");
        playerController = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!PlayerOnRange) {
            WalkPatrolPath();
            IsEnemyOnRange();
        } else {
            HoldAngle();
            IsEnemyOutOfRange();
        }

        if(enemyHP <= 0)
        {
            Died();
        }

    }

    private void IsEnemyOnRange() {
        PlayerOnRange = Vector2.Distance(transform.position, player.transform.position) < 5f;
        transform.up = player.transform.position - transform.position;
        // transform.up = .normalized;
    }

    private void IsEnemyOutOfRange() {
        PlayerOnRange = Vector2.Distance(transform.position, player.transform.position) < 5f;
    }

    private void HoldAngle() {
        float angle = Vector2.Angle(player.transform.position - transform.position, transform.up);
        if (angle < 10f) {
            if (canFire) {
                FireBullet();
            }
        }
    }

    private void WalkPatrolPath()
    {
        Vector2 direction = ((Vector2)targets[targetIndex].position - rb.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        

        float distance = Vector2.Distance(rb.position, (Vector2)targets[targetIndex].position);

        if (Mathf.Abs(distance) < 0.1f)
        {
            transform.position = (Vector2)targets[targetIndex].position;
            targetIndex++;

            if (targetIndex >= targets.Length)
            {
                targetIndex = 0;
            }
        }

        LookAt2D(targets[targetIndex].position);
    }

    public void FireBullet()
    {
        BulletBehavior bulletBehavior = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletBehavior>();
        bulletBehavior.bulletDamage = damage;
        canFire = false;
        StartCoroutine(BulletCooldown());
    }

    public void LookAt2D(Vector3 target)
    {
        Vector2 delta = target - transform.position;
        float radians = Mathf.Atan2(delta.y, delta.x);
        transform.rotation = Quaternion.AngleAxis(radians * Mathf.Rad2Deg - 90, Vector3.forward);
    }

    IEnumerator BulletCooldown()
    {
        yield return new WaitForSeconds(1);
        canFire = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletBehavior bulletBehavior = collision.GetComponent<BulletBehavior>();
       
        enemyHP -= bulletBehavior.bulletDamage;
    }

    private void Died() {
        anim.SetBool(DYING_ANIMATION, true);
    }
}
