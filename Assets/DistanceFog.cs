using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerDistanceFog : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Shader fogShader;

    [Header("Fog Settings")]
    public float clearRadius = 10f;
    public float fogEnd = 30f;
    [Range(0f, 1f)]
    public float fogStrength = 1f;

    [Header("Fog Appearance")]
    public Color fogColor = new Color(
        0.025f, 0.035f, 0.06f, 1f
    );

    private Material fogMaterial;

    void Start()
    {
        // Enable camera depth information.
        GetComponent<Camera>().depthTextureMode |=
            DepthTextureMode.Depth;

        if (fogShader == null)
        {
            enabled = false;
            return;
        }

        fogMaterial = new Material(fogShader);
    }

    void OnRenderImage(RenderTexture source,
                       RenderTexture destination)
    {
        if (fogMaterial == null || player == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        fogMaterial.SetVector(
            "_PlayerPosition",
            player.position
        );

        fogMaterial.SetFloat(
            "_ClearRadius", clearRadius
        );

        fogMaterial.SetFloat(
            "_FogEnd", fogEnd
        );

        fogMaterial.SetFloat(
            "_FogStrength", fogStrength
        );

        fogMaterial.SetColor(
            "_FogColor", fogColor
        );

        Graphics.Blit(
            source, destination, fogMaterial
        );
    }
}

