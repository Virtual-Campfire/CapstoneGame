using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PauseMenuManagerScript : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject GameMapUI;

    FMOD.Studio.Bus MasterBus;

    void Awake()
    {
        // Get the master audio bus
        MasterBus = RuntimeManager.GetBus("Bus:/");

        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        GameMapUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        GameMapUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ExitToTitle()
    {
        //Stop playing music
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        // Go to title screen
        SceneManager.LoadScene(0);
    }
}
