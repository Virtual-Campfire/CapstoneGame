using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleTutorialUI : MonoBehaviour
{
    public GameObject BottleScriptUI;
    public GameObject AudioManager;

    public static bool SciptInstructionsAppear = false;

    private void Awake()
    {
        BottleScriptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BottleScriptUI.SetActive(true);
            SciptInstructionsAppear = true;
            Debug.Log("Inside");
            AudioManager.SendMessage("Page");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            BottleScriptUI.SetActive(false);
            SciptInstructionsAppear = false;
            Debug.Log("Outside");
        }
    }
}
