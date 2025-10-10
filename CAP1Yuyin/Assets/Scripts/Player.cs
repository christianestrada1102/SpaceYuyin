using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    //-----------Comienza la declaración de las variables
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private bool limitarAPantalla = true;

    public float fuerzaVelocidad;
    public GameObject Reiniciar;
    public GameObject GameOver;
    //public Image GameOver;
    public TextMeshProUGUI puntuacionText; // ← CAMBIADO: Text por TextMeshProUGUI

    private int puntuacion;
    private Rigidbody2D rb;
    private Vector2 movimiento;
    private Vector2 limitesPantalla;
    private float anchoNave;
    private float altoNave;
    //-----------Fin de la declaración de las variables

    void Start()
    {
        puntuacion = 3;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Camera cam = Camera.main;
        limitesPantalla = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        GameOver.SetActive(false);
        Reiniciar.SetActive(false);

        // Obtener el tamaño de la nave
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            anchoNave = sprite.bounds.extents.x;
            altoNave = sprite.bounds.extents.y;
        }

        // Actualizar texto inicial
        if (puntuacionText != null)
        {
            puntuacionText.text = puntuacion.ToString();
        }
    }

    void Update()
    {
        // Capturar input (WASD o Flechas)
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        // Normalizar para velocidad consistente en diagonal
        movimiento = movimiento.normalized;
    }

    void FixedUpdate()
    {
        // Calcular nueva posición
        Vector2 nuevaPosicion = rb.position + movimiento * velocidad * Time.fixedDeltaTime;

        // Limitar la nave dentro de la pantalla
        if (limitarAPantalla)
        {
            nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, -limitesPantalla.x + (anchoNave - 0.5f), limitesPantalla.x - (anchoNave - 0.5f));
            nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, -limitesPantalla.y + (altoNave - 0.5f), limitesPantalla.y - (altoNave - 0.5f));
        }

        // Mover la nave
        rb.MovePosition(nuevaPosicion);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("as shokao");

        // Destruir el meteorito
        Destroy(collision.gameObject);

        // Restar vida
        puntuacion--;

        // Actualizar texto
        if (puntuacionText != null)
        {
            puntuacionText.text = puntuacion.ToString();
        }

        // Game Over cuando llegue a 0
        if (puntuacion <= 0)
        {
            Time.timeScale = 0f;
            GameOver.SetActive(true);
            Reiniciar.SetActive(true);
        }
    }
}