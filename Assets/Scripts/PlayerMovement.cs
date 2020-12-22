using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    public float speed;
    public float gravity;
    private CharacterController CC;
    //private Rigidbody rb;
    public float jumpHeight;
    private Animator anim;
    public GameObject Camera;

    public Transform groundCheck;
    public Transform animationGroundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    Vector3 Velocity;
    public bool isGrounded;
    public float JumpWaitTime;
    private float fixedDeltaTime;
    public float propulsionForce;
    private float propulsionTime;
    public float originalPropulsionTime;

    private AudioSource audioSource;
    public AudioClip[] clips;

    public HealthBar PlayerHealth;
    public bool dead = false;
    public GameObject DeathMenu;
    public int healthLost;

    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        propulsionTime = originalPropulsionTime;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead)
        GetInput();
        Movement();
        
    }

    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void GetInput()
    {
        if (Physics.CheckSphere(animationGroundCheck.position, groundDistance, groundMask) && !isGrounded) anim.SetTrigger("fallen");
        //if (!isGrounded) anim.SetTrigger("falling");
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
            propulsionTime = originalPropulsionTime;
        }
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        if (!isGrounded)
        {
            anim.SetFloat("vely", 0f);
            anim.SetFloat("velx", 0f);
        }
        else
        {
            anim.SetFloat("vely", m_verticalInput);
            anim.SetFloat("velx", m_horizontalInput);
        }
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 0.5f;
            speed *= 2;
            gravity *= 2;
            anim.speed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1f;
            speed *= 0.5f;
            gravity *= 0.5f;
            anim.speed *= 0.5f;
        }
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;*/
    }
    private void Movement()
    {
        //var Move = new Vector3(m_horizontalInput, 0f, m_verticalInput);
        //gameObject.transform.Translate(Move);
        Vector3 Move = transform.right * m_horizontalInput + transform.forward * m_verticalInput;
        CC.Move(Move.normalized*speed*Time.deltaTime);
        //rb.AddForce(Move * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetTrigger("Jump");
            StartCoroutine(WaitForJump(JumpWaitTime));
        }
        if (Input.GetButton("Jump") && !isGrounded && propulsionTime > 0)
        {
            if(audioSource.isPlaying&& audioSource.clip.name.Equals("rocket"))
            {

            }
            else
            {
                audioSource.volume = 0.1f;
                audioSource.clip = clips[0];
                audioSource.Play();
            }
            Velocity.y = propulsionForce;
            propulsionTime -= Time.deltaTime;
            //print(propulsionTime);
        }
        Velocity.y += gravity * Time.deltaTime;
        CC.Move(Velocity * Time.deltaTime);
    }
    IEnumerator WaitForJump(float f)
    {
        yield return new WaitForSeconds(f);
        Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.transform.tag.Equals("enemyAttack"))
            {
                print("player lost health");
                PlayerHealth.ChangeHealth(-25f);
                healthLost += 25;
                if (PlayerHealth.GetValue() < 1)
                {
                    anim.SetTrigger("die");
                    dead = true;
                    Camera.GetComponent<ShoulderCam>().ChangeDead();
                    DeathMenu.SetActive(true);
                    Camera.GetComponent<ShoulderCam>().crossHairObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            if (other.transform.tag.Equals("ammopower"))
            {
                Destroy(other.gameObject);
                Camera.GetComponent<ShoulderCam>().ammoCapacity += Camera.GetComponent<ShoulderCam>().originalMagazineSize;
                Camera.GetComponent<ShoulderCam>().ammoLeft.text = Camera.GetComponent<ShoulderCam>().ammoCapacity.ToString();
            }
            if (other.transform.tag.Equals("heart"))
            {
                Destroy(other.gameObject);
                PlayerHealth.ChangeHealth(25f);
                print("gained health");
            }
            if (other.transform.tag.Equals("vial"))
            {
                Destroy(other.gameObject);
                GameObject.Find("Controller").GetComponent<LevelTwoController>().addVial();
                print("gained vial");
            }
        }
        

    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("ammopower"))
        {
            Destroy(collision.gameObject);
            Camera.GetComponent<ShoulderCam>().ammoCapacity += Camera.GetComponent<ShoulderCam>().originalMagazineSize;
            Camera.GetComponent<ShoulderCam>().ammoLeft.text = Camera.GetComponent<ShoulderCam>().ammoCapacity.ToString();
        }
    }*/
}
