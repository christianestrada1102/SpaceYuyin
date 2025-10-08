using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform puntoA; // Punto de inicio
    public Transform puntoB; // Punto de destino
    public float velocidad = 2f;

    private Vector3 objetivo;

    void Start()
    {
        // Empezar moviéndose hacia el punto B
        objetivo = puntoB.position;
    }

    void Update()
    {
        // Mover la plataforma hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

        // Si llegó al objetivo, cambiar de dirección
        if (Vector3.Distance(transform.position, objetivo) < 0.1f)
        {
            // Cambiar objetivo
            if (objetivo == puntoA.position)
                objetivo = puntoB.position;
            else
                objetivo = puntoA.position;
        }
    }

    // Hacer que el jugador se mueva con la plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
