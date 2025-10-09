using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Vector3 objetivo;
    private Rigidbody2D rb2D;
    private Vector3 escalaJugador;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        objetivo = puntoB.position;
    }

    void FixedUpdate()
    {
        Vector3 siguientePosicion = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.fixedDeltaTime);
        rb2D.MovePosition(siguientePosicion);

        if (Vector3.Distance(transform.position, objetivo) < 0.1f)
        {
            objetivo = (objetivo == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "idle_down")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // Guardar escala GLOBAL (no local)
                escalaJugador = collision.transform.lossyScale;
                player.enPlataformaMovil = true;

                // Hacer hijo
                collision.transform.SetParent(transform);

                // Calcular escala local correcta basada en la escala de la plataforma
                Vector3 escalaLocal = new Vector3(
                    escalaJugador.x / transform.lossyScale.x,
                    escalaJugador.y / transform.lossyScale.y,
                    escalaJugador.z / transform.lossyScale.z
                );

                collision.transform.localScale = escalaLocal;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "idle_down")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.enPlataformaMovil = false;

                // Quitar parentesco primero
                collision.transform.SetParent(null);

                // Restaurar escala global guardada
                collision.transform.localScale = escalaJugador;
            }
        }
    }
}