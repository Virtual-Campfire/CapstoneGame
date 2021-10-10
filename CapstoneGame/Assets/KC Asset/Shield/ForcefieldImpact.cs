using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ForcefieldDemo
{
    public class ForcefieldImpact : MonoBehaviour
    {

        // time until impact ripple dissipates
        [Range(0.1f, 5f)]
        [SerializeField] private float dampenTime = 1.5f;

        // maximum displacement on impact
        [Range(0.005f,0.5f)]
        [SerializeField] private float impactRippleAmplitude = 0.05f;
        [Range(0.05f, 1f)]
        [SerializeField] private float impactRippleMaxRadius = 0.35f;

        // allow mouse clicks for testing 
        [SerializeField] private bool clickToImpact;

        //// slight delay between clicks to prevent spamming
        private const float coolDownMax = 0.25f;
        private float coolDownWindow;

        // main camera
        public Camera cam;

        // reference to this MeshRenderer
        private MeshRenderer meshRenderer;

        void Start()
        {
            if (cam == null && Camera.main != null)
            {
                cam = Camera.main;
            }

            meshRenderer = GetComponent<MeshRenderer>();

            coolDownWindow = 0;

        }

        #region DIAGNOSTIC 
        private void UpdateMouse()
        {
            coolDownWindow -= Time.deltaTime;

            if (coolDownWindow <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickToImpact();
                }
            }
        }

        // allow mouse clicks to test forcefield - useful for diagnostic
        private void ClickToImpact()
        {
            if (cam == null)
                return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitXform = hit.transform;

                if (hitXform == this.transform)
                {
                    coolDownWindow = coolDownMax;

                    ApplyImpact(hit.point, hit.normal);
                }
                else
                {
                    // for debugging if we hit another object instead
                    Debug.Log("Hit " + hitXform.name);
                }
            }
        }
        #endregion



        // impact Forcefield, passing in hit point and direction
        public void ApplyImpact(Vector3 hitPoint, Vector3 rippleDirection)
        {
            if (meshRenderer != null)
            {

                meshRenderer.material.SetFloat("_ImpactMaxR", impactRippleMaxRadius);
                meshRenderer.material.SetFloat("_ImpactAmplitude", impactRippleAmplitude);
                meshRenderer.material.SetVector("_ImpactDirection", rippleDirection);
                meshRenderer.material.SetVector("_ImpactPoint", hitPoint);

            }
        }

        // impact Forcefield, passing in RaycastHit
        public void ApplyImpact(RaycastHit hit)
        {
            if (meshRenderer != null)
            {

                meshRenderer.material.SetFloat("_ImpactMaxR", impactRippleMaxRadius);
                meshRenderer.material.SetFloat("_ImpactAmplitude", impactRippleAmplitude);
                meshRenderer.material.SetVector("_ImpactDirection", hit.normal);
                meshRenderer.material.SetVector("_ImpactPoint", hit.point);

            }
        }

        // gradually slow ripple motion 
        private void Dampen()
        {
            if (meshRenderer != null)
            {
                // get the current amplitude
                float currentAmplitude = meshRenderer.material.GetFloat("_ImpactAmplitude");

                // decrement by a small amount per frame
                float newAmplitude = currentAmplitude - (impactRippleAmplitude * Time.deltaTime / dampenTime);

                // clamp to positive values
                newAmplitude = Mathf.Clamp(newAmplitude, 0f, newAmplitude);

                // if negative, disable the ripple; otherwise, set the new amplitude
                if (newAmplitude <= 0)
                {

                }
                else
                {
                    meshRenderer.material.SetFloat("_ImpactAmplitude", newAmplitude);
                }
            }
        }

        void Update()
        {
            if (clickToImpact)
            {
                UpdateMouse();
            }

            Dampen();
        }
    }
}