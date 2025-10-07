using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Si usas TextMeshPro (recomendado)


public class VictoryPanel : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoVictoria;
    public Button botonContinuar;

    [Header("Configuración")]
    [TextArea(3, 6)]
    public string mensajeVictoria = "¡Felicidades!\n¡Has completado la Osa Mayor!";

    [Space(10)]
    public string nombreEscenaSiguiente = "Miniuego3.1";

    [Space(10)]
    public float tiempoAntesDeAparecer = 0.5f;

    void Start()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }

        if (botonContinuar != null)
        {
            botonContinuar.onClick.AddListener(CambiarEscena);
        }
    }

    public void MostrarPanelVictoria()
    {
        StartCoroutine(MostrarPanelConDelay());
    }

    private IEnumerator MostrarPanelConDelay()
    {
        yield return new WaitForSeconds(tiempoAntesDeAparecer);

        if (textoVictoria != null)
        {
            textoVictoria.text = mensajeVictoria;
        }

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            panelVictoria.transform.localScale = Vector3.zero;
            StartCoroutine(AnimarAparicion());
        }
    }

    private IEnumerator AnimarAparicion()
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

    private void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaSiguiente))
        {
            SceneManager.LoadScene(nombreEscenaSiguiente);
        }
        else
        {
            Debug.LogError("ERROR: No se especificó el nombre de la escena siguiente.");
        }
    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}