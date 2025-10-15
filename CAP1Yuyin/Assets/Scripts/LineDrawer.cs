using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [Header("Referencias")]
    public Estrellas controladorEstrellas;
    public LineRenderer lineRenderer;

    [Header("Orden de Conexión Manual")]
    [Tooltip("Arrastra aquí los destinos en el orden que quieres conectarlos")]
    public List<Transform> ordenConexion = new List<Transform>();

    [Header("Configuración de Líneas")]
    public float velocidadDibujo = 2f;
    public float anchoLinea = 0.1f;
    public Color colorLinea = Color.white;
    public Material materialLinea;

    private bool lineasDibujadas = false;

    private void Start()
    {
        if (controladorEstrellas == null)
        {
            controladorEstrellas = FindObjectOfType<Estrellas>();
        }

        // Configurar LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        ConfigurarLineRenderer();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Verificar si todas las estrellas están colocadas
        if (!lineasDibujadas &&
            controladorEstrellas != null &&
            controladorEstrellas.estrellasColocadas >= controladorEstrellas.totalEstrellas)
        {
            StartCoroutine(DibujarLineasConstelacion());
            lineasDibujadas = true;
        }
    }

    private void ConfigurarLineRenderer()
    {
        lineRenderer.startWidth = anchoLinea;
        lineRenderer.endWidth = anchoLinea;
        lineRenderer.startColor = colorLinea;
        lineRenderer.endColor = colorLinea;
        lineRenderer.positionCount = 0;

        if (materialLinea != null)
        {
            lineRenderer.material = materialLinea;
        }
        else
        {
            // Material por defecto
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.material.color = colorLinea;
        }

        // Configuración adicional
        lineRenderer.sortingOrder = 5;
        lineRenderer.useWorldSpace = true;
    }

    private IEnumerator DibujarLineasConstelacion()
    {
        yield return new WaitForSeconds(0.5f); // Pequeña pausa antes de empezar

        lineRenderer.enabled = true;

        // Obtener todos los destinos ocupados en orden
        var destinosOcupados = controladorEstrellas.destinos;
        lineRenderer.positionCount = destinosOcupados.Count;

        // Dibujar líneas progresivamente
        for (int i = 0; i < destinosOcupados.Count; i++)
        {
            Vector3 posicion = destinosOcupados[i].position;
            posicion.z = 0; // Asegurar que está en el plano correcto

            lineRenderer.SetPosition(i, posicion);

            // Si no es la última línea, animar hasta el siguiente punto
            if (i < destinosOcupados.Count - 1)
            {
                yield return StartCoroutine(AnimarLineaHasta(i, i + 1, destinosOcupados));
            }
        }

        // Notificar que las líneas terminaron
        OnLineasCompletadas();
    }

    private IEnumerator AnimarLineaHasta(int indiceActual, int indiceSiguiente, System.Collections.Generic.List<Transform> destinos)
    {
        float tiempoTranscurrido = 0f;
        Vector3 inicio = destinos[indiceActual].position;
        Vector3 fin = destinos[indiceSiguiente].position;
        inicio.z = 0;
        fin.z = 0;

        while (tiempoTranscurrido < velocidadDibujo)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / velocidadDibujo;

            Vector3 posicionIntermedia = Vector3.Lerp(inicio, fin, progreso);
            lineRenderer.SetPosition(indiceSiguiente, posicionIntermedia);

            yield return null;
        }

        lineRenderer.SetPosition(indiceSiguiente, fin);
    }

    private void OnLineasCompletadas()
    {
        Debug.Log("Líneas de constelación completadas");

        // Notificar al ConstelacionManager
        if (ConstelacionManager.Instance != null)
        {
            ConstelacionManager.Instance.OnAnimacionLineasCompletada();
        }
        else
        {
            Debug.LogError("ERROR: No se encontró ConstelacionManager en la escena.");
        }
    }

    // Método opcional para resetear las líneas
    public void ResetearLineas()
    {
        lineasDibujadas = false;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
    }
}