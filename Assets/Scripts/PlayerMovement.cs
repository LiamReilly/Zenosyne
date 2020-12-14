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

    public Transform groundCheck;
    public Transform animationGroundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    Vector3 Velocity;
    public bool isGrounded;
    public float JumpWaitTime;
    private float fixedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        anim.SetFloat("vely", m_verticalInput);
        anim.SetFloat("velx", m_horizontalInput);
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
    private void Movement()
    {
        //var Move = new Vector3(m_horizontalInput, 0f, m_verticalInput);
        //gameObject.transform.Translate(Move);
        Vector3 Move = transform.right * m_horizontalInput + transform.forward * m_verticalInput;
        CC.Move(Move*speed*Time.deltaTime);
        //rb.AddForce(Move * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetTrigger("Jump");
            StartCoroutine(WaitForJump(JumpWaitTime));
        }
        Velocity.y += gravity * Time.deltaTime;
        CC.Move(Velocity * Time.deltaTime);
    }
    IEnumerator WaitForJump(float f)
    {
        yield return new WaitForSeconds(f);
        Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("ground"))
        {
            anim.SetTrigger("falling");
        }
    }
}
