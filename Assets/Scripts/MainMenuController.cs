using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject ParasiteGoal;
    public GameObject ParasiteStart;
    public GameObject Parasite;
    private GameObject CurrentParasite;
    public ParticleSystem Particles;
    public Camera cam;
    public Color hoverColor;
    public Color baseColor;
    // Start is called before the first frame update
    void Start()
    {
        CurrentParasite = Instantiate(Parasite, ParasiteStart.transform.position, Quaternion.identity);
        CurrentParasite.GetComponent<EnemyAI>().target = ParasiteGoal.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ViewportPointToRay(cam.ScreenToViewportPoint(Input.mousePosition));
            RaycastHit hit;
            //var location = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Debug.DrawRay(location, transform.TransformDirection(Vector3.forward), Color.magenta);
                if (hit.collider.gameObject.tag.Equals("enemy"))
                {
                    print("target hit");
                    hit.collider.gameObject.GetComponent<EnemyAI>().Damage();
                    var particle = Instantiate(Particles, hit.point, Quaternion.identity);
                    StartCoroutine(DestroyAfterWait(1f, particle.gameObject));
                }
            }
        }
        if (CurrentParasite.GetComponent<EnemyAI>().getDeadStatus())
        {
            CurrentParasite = Instantiate(Parasite, ParasiteStart.transform.position, Quaternion.identity);
            CurrentParasite.GetComponent<EnemyAI>().target = ParasiteGoal.transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CurrentParasite.transform.position = ParasiteStart.transform.position;
    }
    IEnumerator DestroyAfterWait(float f, GameObject obj)
    {
        yield return new WaitForSeconds(f);
        Destroy(obj);
    }
    public void ButtonHover(Text button)
    {
        button.color = hoverColor;
    }

    public void ButtonHoverExit(Text button)
    {
        button.color = baseColor;
    }
}
