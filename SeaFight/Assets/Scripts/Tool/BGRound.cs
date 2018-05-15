using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRound : MonoBehaviour
{
    protected Material textureToAnimate;

    public Vector2 uvAnimationRate = new Vector2(0.3f, 0.3f);
    public float speed;
    public string textureName = "_MainTex";
    Renderer spRender;

    [SerializeField]
    protected bool resetPositionToZero = true;

    protected void Start()
    {
        spRender = GetComponent<Renderer>();
        spRender.material.mainTextureOffset = Vector2.zero;
    }

    protected void Update()
    {
        Vector2 uvOffset = spRender.material.mainTextureOffset + Time.deltaTime * uvAnimationRate * speed;
        spRender.material.mainTextureOffset = uvOffset;
    }
}
