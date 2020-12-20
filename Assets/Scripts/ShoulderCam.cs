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
    public ParticleSystem Particles;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        gunSound = GetComponent<AudioSource>();
        //InteractText.text = "Press E to interact";
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, cameraClampLow, cameraClampHigh);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (Input.GetMouseButtonDown(0)&&canShoot)
        {
            ShootWithRaycast();
            gunSound.Play();
            canShoot = false;
            StartCoroutine(pauseFire(1f));
        }
    }
    private void ShootWithRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, bulletRange))
        {
            print("target hit");
            if (hit.collider.gameObject.tag.Equals("enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyAI>().Die();
                var particle = Instantiate(Particles, hit.point, Quaternion.identity);
                StartCoroutine(DestroyAfterWait(1f, particle.gameObject));
            }
            
            //print(hit.collider.name);
            
        }
            
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
}
