using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player : MonoBehaviour
{
    [Header("Configuraci�n")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Energ�a")]
    public float energyPerCoin = 10f;

    private Rigidbody2D rb2D;
    private Animator animator;
    private float move;
    private bool isGrounded;
    private bool facingRight = true;

    // TAMA�O FIJO - Cambia estos n�meros si quieres m�s grande o peque�o
    private Vector3 fixedScale = new Vector3(3f, 3f, 1f);

    [Header("Doble Salto")]
    public bool tieneDoubleJump = false;
    private int saltosRestantes = 1;
    private float tiempoDoubleJump = 0f;

    [HideInInspector]
    public bool enPlataformaMovil = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Configurar f�sica
        if (rb2D != null)
        {
            rb2D.gravityScale = 3f;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Establecer tama�o inicial
        transform.localScale = fixedScale;

        Debug.Log("Player listo. Tama�o fijado en: " + fixedScale);
    }

    void Update()
    {
        // Movimiento
        move = Input.GetAxisRaw("Horizontal");

        // Voltear
        if (move > 0 && !facingRight)
        {
            facingRight = true;
        }
        else if (move < 0 && facingRight)
        {
            facingRight = false;
        }

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isGrounded)
            {
                // Salto normal desde el suelo
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                saltosRestantes = tieneDoubleJump ? 2 : 1;
            }
            else if (tieneDoubleJump && saltosRestantes > 0)
            {
                // Doble salto en el aire
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                saltosRestantes--;
            }
        }

        // Resetear saltos al tocar el suelo
        if (isGrounded)
        {
            saltosRestantes = tieneDoubleJump ? 2 : 1;
        }

        // Verificar si el tiempo del power-up termin�
        if (tieneDoubleJump && tiempoDoubleJump > 0f)
        {
            tiempoDoubleJump -= Time.deltaTime;
            if (tiempoDoubleJump <= 0f)
            {
                tieneDoubleJump = false;
                Debug.Log("Power-up de doble salto terminado");
            }
        }

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(move));
            animator.SetFloat("VerticalVelocity", rb2D.velocity.y);
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento
        rb2D.velocity = new Vector2(move * speed, rb2D.velocity.y);

        // Detectar suelo
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }
    }

    // ESTO FUERZA EL TAMA�O CORRECTO DESPU�S DE CADA FRAME
    void LateUpdate()
    {
        // Solo forzar el tama�o si NO est� en plataforma m�vil
        if (!enPlataformaMovil)
        {
            Vector3 scale = fixedScale;

            // Solo cambiar la direcci�n X seg�n donde mire (INVERTIDO)
            if (facingRight)
            {
                scale.x = -Mathf.Abs(fixedScale.x);
            }
            else
            {
                scale.x = Mathf.Abs(fixedScale.x);
            }

            transform.localScale = scale;
        }
    }

    // Ver el GroundCheck en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            // Agregar energ�a cuando recoge el rayo
            if (EnergyManager.instance != null)
            {
                EnergyManager.instance.AddEnergy(energyPerCoin);
            }

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            if (GameOverManager.instance != null)
            {
                GameOverManager.instance.MostrarGameOver();
            }
        }
    }

    public void ActivarDoubleJump(float duracion)
    {
        tieneDoubleJump = true;
        tiempoDoubleJump = duracion;
        saltosRestantes = 2;

        Debug.Log("�Doble salto activado! Duraci�n: " + (duracion > 0 ? duracion + " segundos" : "Permanente"));
    }
}