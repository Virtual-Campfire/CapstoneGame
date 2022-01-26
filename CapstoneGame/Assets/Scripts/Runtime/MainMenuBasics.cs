using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Adam B.
// Script to handle basic main menu actions, like starting the game and quitting it
public class MainMenuBasics : MonoBehaviour
{
    public void StartGame()
    {
        // Load the first scene in the scene index (main menu should be 0)
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // Quit the game
        Application.Quit(0);
    }
}
