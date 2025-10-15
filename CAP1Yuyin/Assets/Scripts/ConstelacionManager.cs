using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ConstelacionManager : MonoBehaviour
{
    public static ConstelacionManager Instance { get; private set; }

    [Header("Referencias UI")]
    public Canvas canvasVictoria;

    [Header("Configuración")]
    public float retardoMostrarCanvas = 0.5f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Asegurar que el canvas esté oculto al inicio
        if (canvasVictoria != null)
        {
            canvasVictoria.enabled = false;
        }
        else
        {
            Debug.LogWarning("ADVERTENCIA: No se ha asignado el Canvas de victoria en el ConstelacionManager");
        }
    }

    // Este método es llamado cuando las líneas terminan de dibujarse
    public void OnAnimacionLineasCompletada()
    {
        Debug.Log("Animación de líneas completada - Mostrando canvas");
        Invoke(nameof(MostrarCanvasVictoria), retardoMostrarCanvas);
    }

    private void MostrarCanvasVictoria()
    {
        if (canvasVictoria != null)
        {
            canvasVictoria.enabled = true;
            Debug.Log("Canvas de victoria mostrado");
        }
        else
        {
            Debug.LogError("ERROR: Canvas de victoria no asignado en el ConstelacionManager");
        }
    }

    // Método opcional para reiniciar el juego
    public void ReiniciarJuego()
    {
        if (canvasVictoria != null)
        {
            canvasVictoria.enabled = false;
        }

        // Aquí puedes agregar más lógica de reinicio si lo necesitas
        // Por ejemplo: resetear estrellas, líneas, etc.
    }
}