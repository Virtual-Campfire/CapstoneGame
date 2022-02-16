using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public Vector2 normalisedMousePosition;
    public float currentAngle;
    public int selection;
    private int previousSelection;

    public GameObject[] menuItems;

    private PauseMenuItemScript menuItemSc;
    private PauseMenuItemScript previousMenuItemSc;

    void Update()
    {
        normalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.width / 2);
        currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

        currentAngle = (currentAngle + 360) % 360;

        selection = (int) currentAngle/45;

        if (selection != previousSelection)
        {
            previousMenuItemSc = menuItems[previousSelection].GetComponent<PauseMenuItemScript>();
            previousMenuItemSc.DeSelect();
            previousSelection = selection;

            menuItemSc = menuItems[selection].GetComponent<PauseMenuItemScript>();
            menuItemSc.Select();
        }

        Debug.Log(selection);
    }
}
