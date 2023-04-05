using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] CameraController cameraFollow;
    [SerializeField] GameObject bulletPrefab;

    public TextMeshProUGUI gunTypeText;
    public TextMeshProUGUI gunAmmoText;

    private AudioSource playerAudio; 

    public AudioClip rifleFireSound;
    public AudioClip pistolFireSound;
    public AudioClip rifleReloadSound;
    public AudioClip pistolReloadSound;
    public AudioClip footstep1;
    public AudioClip footstep2;
    public AudioClip dashSound;
    
    private float playerSpeed;

    private bool isMoving = false;

    public bool isUsingPistol = true;
    public bool isReloading = false;

    public float playerHP = 100;

    private float normalSpeed = 10.0f;
    private float dashSpeed = 20.0f;

    // Fire rates for guns
    private float pistolFR = 2f;
    private float rifleFR = 5f;

    // Reload times for guns
    private float pistolReloadTime = 2f;
    private float rifleReloadTime = 3f;

    private float ammoSize;
    private float magSize;

    public float pistolAmmoSize = 12f;
    public float pistolMagSize = 100f;
    public float rifleAmmoSize = 30f;
    public float rifleMagSize = 90f;

    public float currentPistolAmmo;
    public float currentRifleAmmo;

    public float currentPistolMag;
    public float currentRifleMag;

    private bool canFire = true;

    public float fireCooldown;
    public float reloadTime;

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

        ammoSize = pistolAmmoSize;
        magSize = pistolMagSize;

        fireCooldown = 1 / pistolFR;
        reloadTime = pistolReloadTime;

        currentPistolAmmo = pistolAmmoSize;
        currentRifleAmmo = rifleAmmoSize;

        currentPistolMag = pistolMagSize;
        currentRifleMag = rifleMagSize;

        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();
        LookAtMouse();
        HandleInteractions();

        if(Input.GetMouseButtonDown(0) && canFire)
        {
            if((isUsingPistol && currentPistolAmmo > 0) || (!isUsingPistol && currentRifleAmmo > 0))
            {
                SpawnBullet();
                cameraFollow.TriggerShake();
            }
        }

        if(isUsingPistol)
        {
            gunTypeText.text = "Gun: Pistol";
            if(!isReloading)
            {
                gunAmmoText.text = currentPistolAmmo + "/" + currentPistolMag;
            }

        }
        else
        {
            gunTypeText.text = "Gun: Rifle";
            if (!isReloading)
            {
                gunAmmoText.text = currentRifleAmmo + "/" + currentRifleMag;
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isUsingPistol && currentPistolMag > 0 && currentPistolAmmo == 0)
            {
                currentPistolAmmo = pistolAmmoSize;
                currentPistolMag -= pistolAmmoSize;
                canFire = false;
                StartCoroutine(ReloadTime());
            }
            else if(!isUsingPistol && currentRifleMag > 0 && currentRifleAmmo == 0)
            {
                currentRifleAmmo = rifleAmmoSize;
                currentRifleMag -= rifleAmmoSize;
                canFire = false;
                StartCoroutine(ReloadTime());
            }
        }

        SetGunType();

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
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                playerSpeed = dashSpeed;
                dashCounter = dashLength;
                trailRenderer.emitting = true;
                playerAudio.PlayOneShot(dashSound, 1f);
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

    private void SpawnBullet()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);

        if(isUsingPistol)
        {
            playerAudio.PlayOneShot(pistolFireSound, 1f);
            currentPistolAmmo--;
        }
        else
        {
            playerAudio.PlayOneShot(rifleFireSound, 1f);
            currentRifleAmmo--;
        }

        canFire = false;
        StartCoroutine(FireCooldown());

    }

    private void SetGunType()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            isUsingPistol = true;
            Debug.Log("Gun: Pistol, Fire Rate: " + pistolFR + " bullets/sec, Reload Speed: " + pistolReloadTime + ", ammo/mag: " + currentPistolAmmo + "/" + currentPistolMag);

            reloadTime = pistolReloadTime;
            fireCooldown = 1/pistolFR;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            isUsingPistol = false;
            Debug.Log("Gun: Rifle, Fire Rate: " + rifleFR + " bullets/sec, Reload Speed: " + rifleReloadTime + ", ammo/mag: " + currentRifleAmmo + "/" + currentRifleMag);
            reloadTime = rifleReloadTime;
            fireCooldown = 1/rifleFR;
        }
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    IEnumerator ReloadTime()
    {
        gunAmmoText.text = "Reloading";
        isReloading = true;

        if(isUsingPistol)
        {
            playerAudio.PlayOneShot(pistolReloadSound, 1f);
        }
        else
        {
            playerAudio.PlayOneShot(rifleReloadSound, 1f);
        }

        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        canFire = true;
    }
}