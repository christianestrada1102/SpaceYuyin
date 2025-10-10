using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reiniciar : MonoBehaviour
{

    public void ReiniciarEscena()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {

    }

}
