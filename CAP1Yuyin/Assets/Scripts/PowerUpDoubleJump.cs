using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpDoubleJump : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Buscar por nombre "idle_down" en vez de tag
        if (collision.gameObject.name == "idle_down")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.ActivarDoubleJump(duracion);
                Destroy(gameObject);
            }
        }
    }
}

