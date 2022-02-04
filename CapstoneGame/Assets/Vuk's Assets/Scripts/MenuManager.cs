using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{
    public GameObject[] frame;
    public GameObject startButton;
    public EventSystem ES;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        // If title card is active and escape is pressed, go to main options
        if (Input.anyKeyDown && frame[0].activeInHierarchy)
        {
            frame[0].SetActive(false);
            frame[1].SetActive(true);
            ES.SetSelectedGameObject(startButton);
        }

        // If main options are active and escape is pressed, go back to title card
        if (Input.GetKeyDown(KeyCode.Escape) && !frame[0].activeInHierarchy && !frame[2].activeInHierarchy)
        {
            frame[1].SetActive(false);
            frame[2].SetActive(false);
            frame[0].SetActive(true);
        }

        // If credits are active and escape is pressed, go back to main options
        if (Input.GetKeyDown(KeyCode.Escape) && frame[2].activeInHierarchy)
        {
            frame[1].SetActive(true);
            frame[2].SetActive(false);
            frame[0].SetActive(false);
        }
    }

    public void Credits()
    {
        frame[0].SetActive(false);
        frame[1].SetActive(false);
        frame[2].SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}