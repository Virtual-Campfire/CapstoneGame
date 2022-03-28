using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{

    private FMOD.Studio.VCA VcaController;
    public string VcaName;

    private Slider _slider;

   // FMOD.Studio.EventInstance _event;

    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);

        _slider = GetComponent<Slider>();

    }

    public void SetVolume(float volume)
    {
        VcaController.setVolume(volume);
    }


    void Update()
    {
        
    }
}
