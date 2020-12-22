using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelOneController : MonoBehaviour
{
    private GameManager GM;
    public ShoulderCam SC;
    public Transform[] SpawnPoints;
    public GameObject Parasite;
    public Transform Player;
    public Text Timer;
    public Text MessagePlayer;
    private float TimeLeft = 120f;
    private float ParasiteInterval = 2f;
    public InputField SensInput;
    public GameObject OptionButtons;
    public Color hoverColor;
    public Color baseColor;
    public bool paused = false;
    public GameObject PauseOptions;
    public bool end = false;
    public GameObject endPlane;
    public Text EnemiesKilled;
    public Text HealthLost;
    public GameObject blackBackground;
    public GameObject ContinueButton;
    private AudioSource audioSource;
    public Color baseWhiteColor;
    private float fixedDeltaTime;
    public GameObject barrier;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        SC.setSens(GM.GetSens());
        Timer.text = string.Format("{0:N0}", TimeLeft);
        MessagePlayer.text = "Survive for two minutes to move on!";
        MessagePlayer.gameObject.SetActive(true);
        StartCoroutine(DisableAfterTime(3f, MessagePlayer.gameObject));
        StartCoroutine(StartParasite());
        audioSource = GetComponent<AudioSource>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !paused)
        {
            Time.timeScale = 0;
            PauseOptions.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
        }
        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            Timer.text = string.Format("{0:N0}", TimeLeft);
        }
        if(TimeLeft<0&& !end)
        {
            end = true;
            EndLevel();
        }
        
    }
    IEnumerator DisableAfterTime(float f, GameObject Obj)
    {
        yield return new WaitForSeconds(f);
        Obj.SetActive(false);
    }
    IEnumerator StartParasite()
    {
        yield return new WaitForSeconds(ParasiteInterval);
        MakeParasite();
    }
    public void MakeParasite()
    {
        var curr = Instantiate(Parasite, SpawnPoints[UnityEngine.Random.Range(0, 4)].position, Quaternion.identity);
        curr.GetComponent<EnemyAI>().target = Player;
        print("parasite made");
        if (TimeLeft > 0) StartCoroutine(StartParasite());
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Options()
    {
        OptionButtons.SetActive(true);
        Player.gameObject.GetComponent<PlayerMovement>().DeathMenu.SetActive(false);
        PauseOptions.SetActive(false);
    }
    public void QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void setSens()
    {
        var Level = SensInput.text;
        if (Level.Length == 0)
        {
            Level = "100";
        }
        GM.GetComponent<GameManager>().setSens(int.Parse(Level));
        SC.setSens(GM.GetSens());
    }
    public void CloseOptions()
    {
        OptionButtons.SetActive(false);
        Player.gameObject.GetComponent<PlayerMovement>().DeathMenu.SetActive(true);
    }
    public void ClosePauseOptions()
    {
        OptionButtons.SetActive(false);
        PauseOptions.SetActive(true);
    }
    public void ButtonHover(Text button)
    {
        button.color = hoverColor;
    }

    public void ButtonHoverExit(Text button)
    {
        button.color = baseColor;
    }
    public void ButtonHoverExitBlackScreen(Text button)
    {
        button.color = baseWhiteColor;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        PauseOptions.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        paused = false;
    }
    public void EndLevel()
    {
        MessagePlayer.text = "Escape!";
        MessagePlayer.gameObject.SetActive(true);
        StartCoroutine(DisableAfterTime(3f, MessagePlayer.gameObject));
        endPlane.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            barrier.SetActive(true);
            Player.gameObject.GetComponent<PlayerMovement>().dead = true;
            SC.ChangeDead();
            blackBackground.SetActive(true);
            EnemiesKilled.text = "Enemies killed: " + SC.EnemiesKilled.ToString();
            HealthLost.text = "Health lost: " + Player.gameObject.GetComponent<PlayerMovement>().healthLost.ToString();
            StartCoroutine(WaitAndShow());
            GM.addKilled(SC.EnemiesKilled);
            GM.addHealthLost(Player.gameObject.GetComponent<PlayerMovement>().healthLost);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(1.5f);
        audioSource.Play();
        EnemiesKilled.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        audioSource.Play();
        HealthLost.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        audioSource.Play();
        ContinueButton.SetActive(true);
    }
    public void Continue()
    {
        SceneManager.LoadScene("LevelTwo");
    }
}
