using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    public GameObject bulletPrefab;

    // The targets where we want the enemy to travel
    public Transform[] targets;

    private GameObject player;

    public int targetIndex = 0;
    
    public float speed = 5f;

    //public bool isPatrolling = true;
    public bool canFire = true;

    public float enemyHP = 100f;

    [SerializeField] PlayerController playerController;


    // Rigidbody for enemy
    Rigidbody2D rb;

    //// Start is called before the first frame update
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        WalkPatrolPath();

        float angle = 10;
        if (Vector3.Angle(player.transform.forward, transform.position - player.transform.position) < angle)
        {
            if (canFire)
            {
                FireBullet();
            }
        }

        // Holding-angle enemy code? To face player
        //Vector3 dir = player.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if(enemyHP <= 0)
        {
            Destroy(gameObject);
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
        Instantiate(bulletPrefab, transform.position, transform.rotation);
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
        if(playerController.isUsingPistol)
        {
            enemyHP -= 8f;
        }
        else
        {
            enemyHP -= 15f;
        }
    }
}
