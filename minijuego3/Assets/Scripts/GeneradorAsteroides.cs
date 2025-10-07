using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorAsteroides : MonoBehaviour
{

    public GameObject meteoro;
    private float randomX, randomY;
    private int asteroidesEnJuego;
    // Start is called before the first frame update
    void Start()
    {
        asteroidesEnJuego = 0;
        StartCoroutine(OleadaAsteroides());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CrearAsteroide()
    {
        randomX = Random.Range(-9.15f, 9.24f);
        randomY = Random.Range(7.82f, 5.95f);
        GameObject nuevoMeteoro = Instantiate(meteoro) as GameObject;
        nuevoMeteoro.transform.position = new Vector3(randomX, randomY, 0f);

        nuevoMeteoro.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(7f, -150));

        asteroidesEnJuego++;
    }

    IEnumerator OleadaAsteroides()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            //Limitacion de obstaculos que se presentan
            if (asteroidesEnJuego < 15)
            {
                CrearAsteroide();
            }
        }
    }
}