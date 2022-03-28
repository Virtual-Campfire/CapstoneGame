using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCAController : MonoBehaviour
{
    // Start is called before the first frame update

    private FMOD.Studio.VCA VcaController;
    public string VcaName;

    FMOD.Studio.EventInstance _event;

    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
