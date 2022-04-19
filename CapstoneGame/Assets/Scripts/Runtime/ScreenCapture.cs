using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenCapture : MonoBehaviour
{
    bool taken;

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += ScreenshotNoUI;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= ScreenshotNoUI;
    }

    // Update is called once per frame
    void Update()
    {
        // Screenshot with UI
        if (Input.GetKeyDown(KeyCode.Period))
        {
            StartCoroutine(ScreenshotUI());
        }
        // Screenshot without UI
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            taken = true;
        }
    }

    // Makes a screenshot before UI is rendered
    IEnumerator ScreenshotUI()
    {
        yield return new WaitForEndOfFrame();

        Capture();
    }

    // Makes a screenshot after UI is rendered
    void ScreenshotNoUI(ScriptableRenderContext renderContext, Camera cam)
    {
        if (taken)
        {
            taken = false;

            Capture();
        }
    }

    // Code for collecting the screenshot data
    void Capture()
    {
        int width = Screen.width;
        int height = Screen.height;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshot.ReadPixels(rect, 0, 0);
        screenshot.Apply();

        byte[] imageBytes = screenshot.EncodeToPNG();

        // Create directory for screenshots (if one does not exist already)
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Captures");
        
        Debug.Log("Attempting to save screenshot to: " + Application.dataPath + "/Captures/Screenshot" + Time.frameCount + ".png");

        // Save the data stream as a PNG (with frame count in name so as to avoid most occurrences where images would overwrite one another)
        System.IO.File.WriteAllBytes(Application.dataPath + "/Captures/Screenshot" + Time.frameCount + ".png", imageBytes);
    }
}
