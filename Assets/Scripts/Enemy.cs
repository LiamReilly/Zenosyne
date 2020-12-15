using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    NavMeshAgent agentNav;
    Rigidbody rb;
    Vector3 DestinationCheck;
    public float MinimumDistance;
    Animator anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    private float xMin = -0.5f, xMax = 0.5f;
    private bool walking = false;
    private GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        DestinationCheck = gameObject.transform.position;
        agentNav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agentNav.updatePosition = false;
    }

    public void SetDestination(Vector3 destination)
    {
        agentNav.SetDestination(destination);
        DestinationCheck = destination;
    }
    private void Update()
    {
        if(player != null)
        {
            SetDestination(player.transform.position);
        }
        Vector3 worldDeltaPosition = agentNav.nextPosition - transform.position;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;
        bool shouldMove = velocity.magnitude > 0.5f && agentNav.remainingDistance > agentNav.radius;
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);
        agentNav.speed = 3.5f;

    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agentNav.nextPosition;
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.tag);
        if (collision.transform.tag.Equals("Player"))
        {
            print("fuc");
            if (Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - DestinationCheck.x, 2f)) < MinimumDistance &&
            Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.y - DestinationCheck.y, 2f)) < MinimumDistance &&
            Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.z - DestinationCheck.z, 2f)) < MinimumDistance)
            {
                SetDestination(gameObject.transform.position);
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            player = other.gameObject;
            SetDestination(player.transform.position);
        }
    }

}
