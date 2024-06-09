using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMain : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if(!player)
        {
            MainMenu();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
