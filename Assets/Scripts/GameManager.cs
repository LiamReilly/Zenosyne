using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Options;
    private int sens;
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
}
