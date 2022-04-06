using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTextureMove : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material;
    float xOffset;
    float yOffset;
    public float xOffsetSpeed = -0.01f;
    public float yOffsetSpeed = 0.01f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = GetComponent<Material>();
    }
    void Update()
    {
        xOffset += xOffsetSpeed * Time.deltaTime;
        yOffset += yOffsetSpeed * Time.deltaTime;

        meshRenderer.material.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
}
