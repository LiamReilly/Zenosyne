using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoulderCam : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;
    //public LayerMask interactable;
    public float bulletRange;
    public Text InteractText;
    public bool canShoot = true;
    public float cameraClampLow, cameraClampHigh;
    private AudioSource gunSound;
    public AudioClip[] clips;
    public ParticleSystem Particles;
    public int EnemiesKilled = 0;

    [Header("Gun Stuff")]
    private int magazineSize;
    public int originalMagazineSize;
    public int ammoCapacity;
    private int  BaseAmmoCapacity;
    private bool reloading;
    public Text bulletsInMag;
    public Text ammoLeft;
    public Text reloadText;
    private Image crossHair;
    public GameObject crossHairObject;
    private Animator crossAnim;
    public Text noAmmo;
    private bool dead = false;
    public ParticleSystem MuzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        BaseAmmoCapacity = ammoCapacity;
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        gunSound = GetComponent<AudioSource>();
        magazineSize = originalMagazineSize;
        bulletsInMag.text = magazineSize.ToString();
        ammoLeft.text = ammoCapacity.ToString();
        crossAnim = crossHairObject.GetComponent<Animator>();
        crossHair = crossHairObject.GetComponent<Image>();
        MuzzleFlash.Stop();
        //InteractText.text = "Press E to interact";
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            playerBody.Rotate(Vector3.up * mouseX);
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, cameraClampLow, cameraClampHigh);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            if (Input.GetMouseButtonDown(0) && canShoot && magazineSize > 0 &&Time.timeScale != 0)
            {
                ShootWithRaycast();
                gunSound.clip = clips[0];
                gunSound.Play();
                canShoot = false;
                StartCoroutine(pauseFire(1f));
                magazineSize -= 4;
                bulletsInMag.text = magazineSize.ToString();
            }
            /*if(Input.GetMouseButtonDown(0) && magazineSize <= 0 && !reloading&&ammoCapacity>0)
            {
                gunSound.clip = clips[1];
                gunSound.Play();
                reloading = true;
                reloadText.enabled = false;
                crossHair.enabled = true;
                crossAnim.SetTrigger("reload");
                StartCoroutine(reload(3f));
            }*/
            if (Input.GetKeyDown(KeyCode.R) && !reloading && ammoCapacity > 0 && magazineSize != originalMagazineSize)
            {
                gunSound.clip = clips[1];
                gunSound.Play();
                reloading = true;
                reloadText.enabled = false;
                crossHair.enabled = true;
                crossAnim.SetTrigger("reload");
                if (magazineSize > 0) ammoCapacity += magazineSize;
                StartCoroutine(reload(3f));
            }
            if (magazineSize < 1 && !reloading && ammoCapacity > 0)
            {
                crossHair.enabled = false;
                reloadText.enabled = true;
            }
            if (ammoCapacity < 1 && magazineSize < 1)
            {
                crossHair.enabled = false;
                noAmmo.enabled = true;
            }
            else
            {
                noAmmo.enabled = false;
            }
        }
        
    }
    private void ShootWithRaycast()
    {
        MuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, bulletRange))
        {
            //print("target hit");
            if (hit.collider.gameObject.tag.Equals("enemy"))
            {
                EnemiesKilled++;
                hit.collider.gameObject.GetComponent<EnemyAI>().Damage();
                var particle = Instantiate(Particles, hit.point, Quaternion.identity);
                StartCoroutine(DestroyAfterWait(1f, particle.gameObject));
            }
            
            //print(hit.collider.name);
            
        }
            
    }
    IEnumerator reload(float f)
    {
        yield return new WaitForSeconds(f);
        if (ammoCapacity > 11)
        {
            magazineSize = originalMagazineSize;
            ammoCapacity -= originalMagazineSize;
        }
        else
        {
            magazineSize = ammoCapacity;
            ammoCapacity -= ammoCapacity;
        }
        reloading = false;
        ammoLeft.text = ammoCapacity.ToString();
        bulletsInMag.text = magazineSize.ToString();
    }
    IEnumerator pauseFire(float f)
    {
        yield return new WaitForSeconds(f);
        canShoot = true;
    }
    IEnumerator DestroyAfterWait(float f, GameObject obj)
    {
        yield return new WaitForSeconds(f);
        Destroy(obj);
    }
    public void ChangeDead()
    {
        dead = !dead;
    }
    public void setSens(float f)
    {
        mouseSensitivity = f;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("wall"))
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("wall"))
        other.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    public void fillAmmo()
    {
        if (ammoCapacity < BaseAmmoCapacity) ammoCapacity = BaseAmmoCapacity + originalMagazineSize;
        else
        {
            ammoCapacity = BaseAmmoCapacity;
        }
        ammoLeft.text = ammoCapacity.ToString();
    }
}
