using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Renderer fondo1;
    public Renderer fondo2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimacionFondo();
    }


    public void AnimacionFondo()
    {
        fondo1.material.mainTextureOffset = fondo1.material.mainTextureOffset + new Vector2(0, 0.015f) * Time.deltaTime;
        fondo2.material.mainTextureOffset = fondo2.material.mainTextureOffset + new Vector2(0, 0.05f) * Time.deltaTime;
    }



}
