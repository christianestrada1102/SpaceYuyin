using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Fungus;

public class GameManager : MonoBehaviour
{
    [Header("Sistema de Destinos")]
    public List<Transform> todosLosDestinos;

    [Header("Referencias de Constelación")]
    public ConstellationLines constellationLines;

    [Header("Referencias UI - Panel de Victoria")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoVictoria;
    public TextMeshProUGUI textoTimer;

    [Header("Configuración de Victoria")]
    [TextArea(3, 6)]
    public string mensajeVictoria = "¡Felicidades!\n¡Has completado la Osa Mayor!";

    // NUEVO: Opción para elegir el comportamiento
    public enum TipoTransicion { CambiarEscena, EjecutarBloqueFungus }
    public TipoTransicion tipoTransicion = TipoTransicion.CambiarEscena;

    public string nombreEscenaSiguiente = "Miniuego3.1";
    public string nombreBloqueFungus = "SalidaSaul"; // ← NUEVO

    public float tiempoAntesDeAparecer = 0.5f;
    public float tiempoAntesDecambiarEscena = 5f;

    private Dictionary<Transform, bool> estadoDestinos;

    void Start()
    {
        Debug.Log(">>> GameManager.Start() iniciando...");

        // Inicializa el sistema de seguimiento de destinos
        estadoDestinos = new Dictionary<Transform, bool>();

        if (todosLosDestinos == null || todosLosDestinos.Count == 0)
        {
            Debug.LogError("ERROR: La lista 'todosLosDestinos' está vacía.");
            return;
        }

        // Busca ConstellationLines si no está asignado
        if (constellationLines == null)
        {
            constellationLines = FindObjectOfType<ConstellationLines>();
        }

        // CRÍTICO: Oculta el panel de victoria al inicio
        if (panelVictoria != null)
        {
            Debug.Log(">>> Ocultando panel de victoria al inicio");
            panelVictoria.SetActive(false);
        }
        else
        {
            Debug.LogError("ERROR: panelVictoria es NULL en Start()!");
        }

        // Ya no necesitamos configurar el botón

        // Inicializa todos los destinos como LIBRES
        foreach (Transform destino in todosLosDestinos)
        {
            estadoDestinos.Add(destino, false);
        }

        Debug.Log(">>> GameManager.Start() completado");
    }

    void Update()
    {
        // Removido el código de diagnóstico temporal
    }

    public Transform GetClosestAvailableDestination(Vector3 starPosition)
    {
        Transform mejorDestino = null;
        float menorDistancia = float.MaxValue;

        var destinosLibres = estadoDestinos.Where(pair => pair.Value == false).Select(pair => pair.Key);

        foreach (Transform destino in destinosLibres)
        {
            float distancia = Vector3.Distance(starPosition, destino.position);

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                mejorDestino = destino;
            }
        }

        return mejorDestino;
    }

    public void MarkDestinationAsOccupied(Transform destino)
    {
        if (estadoDestinos.ContainsKey(destino))
        {
            estadoDestinos[destino] = true;
        }

        // Comprobar victoria
        if (estadoDestinos.All(pair => pair.Value == true))
        {
            Debug.Log("¡JUEGO GANADO!");

            if (constellationLines != null)
            {
                StartCoroutine(ActivarAnimacionConVictoria());
            }
            else
            {
                OnAnimacionLineasCompletada();
            }
        }
    }

    private IEnumerator ActivarAnimacionConVictoria()
    {
        yield return new WaitForSeconds(0.5f);

        // Dibuja las líneas Y ESPERA a que termine
        constellationLines.DibujarConstelacion();

        // NO mostramos el panel aquí - lo hará ConstellationLines
    }

    // Método que ConstellationLines llamará al terminar la animación
    public void OnAnimacionLineasCompletada()
    {
        Debug.Log(">>> Animación de líneas completada, mostrando panel");
        StartCoroutine(MostrarPanelVictoria());
    }

    private IEnumerator MostrarPanelVictoria()
    {
        yield return new WaitForSeconds(tiempoAntesDeAparecer);

        if (textoVictoria != null)
        {
            textoVictoria.text = mensajeVictoria;
        }

        if (panelVictoria != null)
        {
            Debug.Log(">>> ACTIVANDO PANEL DE VICTORIA");
            panelVictoria.SetActive(true);

            // Asegura que el panel esté centrado en pantalla
            RectTransform rectTransform = panelVictoria.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = Vector2.zero;
            }

            panelVictoria.transform.localScale = Vector3.zero;
            StartCoroutine(AnimarAparicionPanel());

            // Iniciar el timer automático
            StartCoroutine(CambiarEscenaConTimer());
        }
    }

    private IEnumerator AnimarAparicionPanel()
    {
        float duracion = 0.3f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float escala = Mathf.Lerp(0f, 1f, tiempo / duracion);
            panelVictoria.transform.localScale = Vector3.one * escala;
            yield return null;
        }

        panelVictoria.transform.localScale = Vector3.one;
    }

    private IEnumerator CambiarEscenaConTimer()
    {
        float tiempoRestante = tiempoAntesDecambiarEscena;

        while (tiempoRestante > 0)
        {
            // Actualizar el texto del timer si existe
            if (textoTimer != null)
            {
                textoTimer.text = $"Siguiente escena en: {Mathf.Ceil(tiempoRestante)}s";
            }

            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        // Limpiar el texto del timer
        if (textoTimer != null)
        {
            textoTimer.text = "Cargando...";
        }

        // Cambiar de escena
        Debug.Log(">>> Timer completado, cambiando de escena");
        CambiarEscena();
    }

    // CAMBIO CRÍTICO: Ahora es PUBLIC para que el botón pueda llamarlo
    public void CambiarEscena()
    {
        Debug.Log(">>> ========================================");
        Debug.Log(">>> ¡¡¡BOTÓN CONTINUAR PRESIONADO!!!");
        Debug.Log(">>> ========================================");
        Debug.Log($">>> Tipo de transición configurado: {tipoTransicion}");

        // Verificar qué tipo de transición usar
        if (tipoTransicion == TipoTransicion.EjecutarBloqueFungus)
        {
            Debug.Log(">>> ENTRANDO EN MODO FUNGUS");
            Debug.Log($">>> Nombre del bloque a ejecutar: '{nombreBloqueFungus}'");
            Debug.Log($">>> Nombre de escena siguiente: '{nombreEscenaSiguiente}'");

            // Guarda el nombre del bloque
            PlayerPrefs.SetString("BloqueFungusAEjecutar", nombreBloqueFungus);
            PlayerPrefs.Save();
            Debug.Log(">>> PlayerPrefs guardado correctamente");

            // Verifica que se guardó
            string verificacion = PlayerPrefs.GetString("BloqueFungusAEjecutar", "NO_ENCONTRADO");
            Debug.Log($">>> Verificación PlayerPrefs: '{verificacion}'");

            // Carga la escena por ÍNDICE
            Debug.Log(">>> Intentando cargar escena índice 0...");
            SceneManager.LoadScene(0);
            Debug.Log(">>> Comando LoadScene ejecutado");
        }
        else // TipoTransicion.CambiarEscena
        {
            Debug.Log(">>> ENTRANDO EN MODO CAMBIAR ESCENA NORMAL");

            if (string.IsNullOrEmpty(nombreEscenaSiguiente))
            {
                Debug.LogError("ERROR: 'nombreEscenaSiguiente' está vacío!");
                return;
            }

            Debug.Log($">>> Intentando cargar escena: '{nombreEscenaSiguiente}'");

            if (Application.CanStreamedLevelBeLoaded(nombreEscenaSiguiente))
            {
                Debug.Log($">>> Cargando escena: {nombreEscenaSiguiente}");
                SceneManager.LoadScene(nombreEscenaSiguiente);
            }
            else
            {
                Debug.LogError($"ERROR: La escena '{nombreEscenaSiguiente}' NO existe en Build Settings.");
            }
        }
    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetRemainingDestinations()
    {
        return estadoDestinos.Count(pair => pair.Value == false);
    }
}