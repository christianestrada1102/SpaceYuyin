using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationLines : MonoBehaviour
{
    [Header("Configuración de Líneas")]
    public Material lineaMaterial;
    public float anchoLinea = 0.1f;
    public Color colorLinea = Color.white;
    public float duracionAnimacion = 0.15f;

    [Header("Orden de Conexión - Osa Mayor")]
    [Tooltip("Arrastra aquí los 7 destinos en el ORDEN que quieres conectarlos")]
    public List<Transform> ordenDeConexion;

    private List<LineRenderer> lineas = new List<LineRenderer>();
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void DibujarConstelacion()
    {
        StartCoroutine(AnimarLineasConstelacion());
    }

    private IEnumerator AnimarLineasConstelacion()
    {
        // Validación
        if (ordenDeConexion == null || ordenDeConexion.Count < 2)
        {
            Debug.LogError("ERROR: Necesitas al menos 2 destinos en 'ordenDeConexion'");
            yield break;
        }

        Debug.Log(">>> INICIO de animación de líneas");

        // Dibuja cada línea conectando los puntos en orden
        for (int i = 0; i < ordenDeConexion.Count - 1; i++)
        {
            Transform inicio = ordenDeConexion[i];
            Transform fin = ordenDeConexion[i + 1];

            if (inicio == null || fin == null)
            {
                Debug.LogWarning($"Advertencia: Destino {i} o {i + 1} es nulo, saltando línea");
                continue;
            }

            // ESPERA a que esta línea termine de dibujarse
            yield return StartCoroutine(DibujarLineaAnimada(inicio.position, fin.position));

            // Pequeña pausa entre cada línea
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log(">>> FIN de animación de líneas - Todas las líneas dibujadas");

        // AHORA SÍ notifica al GameManager
        if (gameManager != null)
        {
            Debug.Log(">>> Llamando a OnAnimacionLineasCompletada...");
            gameManager.OnAnimacionLineasCompletada();
        }
        else
        {
            Debug.LogError("ERROR: GameManager es NULL en ConstellationLines!");
        }
    }

    private IEnumerator DibujarLineaAnimada(Vector3 inicio, Vector3 fin)
    {
        // Crea un nuevo GameObject para esta línea
        GameObject lineaObj = new GameObject("LineaConstelacion");
        lineaObj.transform.parent = this.transform;

        LineRenderer lr = lineaObj.AddComponent<LineRenderer>();
        lineas.Add(lr);

        // Configuración del LineRenderer
        lr.startWidth = anchoLinea;
        lr.endWidth = anchoLinea;
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        // Asigna el material
        if (lineaMaterial != null)
        {
            lr.material = lineaMaterial;
        }
        else
        {
            lr.material = new Material(Shader.Find("Sprites/Default"));
        }

        lr.startColor = colorLinea;
        lr.endColor = colorLinea;

        // Ajusta el sorting layer
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 10;

        // Animación: la línea "crece" desde el inicio hasta el fin
        float tiempo = 0f;

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracionAnimacion;

            Vector3 puntoActual = Vector3.Lerp(inicio, fin, progreso);

            lr.SetPosition(0, inicio);
            lr.SetPosition(1, puntoActual);

            yield return null;
        }

        // Asegura que la línea llegue exactamente al final
        lr.SetPosition(0, inicio);
        lr.SetPosition(1, fin);

        // Efecto de brillo en la línea
        yield return StartCoroutine(BrilloLinea(lr));
    }

    private IEnumerator BrilloLinea(LineRenderer lr)
    {
        float duracion = 0.5f;
        float tiempo = 0f;
        Color colorOriginal = colorLinea;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float brillo = 1f + Mathf.Sin((tiempo / duracion) * Mathf.PI) * 0.5f;

            lr.startColor = colorOriginal * brillo;
            lr.endColor = colorOriginal * brillo;

            yield return null;
        }

        lr.startColor = colorOriginal;
        lr.endColor = colorOriginal;
    }

    public void BorrarLineas()
    {
        foreach (LineRenderer lr in lineas)
        {
            if (lr != null)
            {
                Destroy(lr.gameObject);
            }
        }
        lineas.Clear();
    }
}