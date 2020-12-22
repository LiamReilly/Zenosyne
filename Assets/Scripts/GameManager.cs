using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Options;
    private int sens = 100;
    private int enemiesKilled = 0;
    private int healthLost = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Preloading"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenOptions()
    {
        Options.SetActive(true);
    }
    public void CloseOptions()
    {
        Options.SetActive(false);
    }
    public void setSens(int i)
    {
        sens = i;
        print(sens);
    }
    public float GetSens()
    {
        return sens;
    }
    public void addKilled(int i)
    {
        enemiesKilled += i;
    }
    public void addHealthLost(int i)
    {
        healthLost += i;
    }
    public int GetKilled()
    {
        return enemiesKilled;
    }
    public int GetHealthLost()
    {
        return healthLost;
    }
}
