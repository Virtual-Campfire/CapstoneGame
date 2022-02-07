using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Adam B.
// Temporary script for returning to main menu, bypassing any pause menus
public class EscToMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // When escape is pressed, return to title screen
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
