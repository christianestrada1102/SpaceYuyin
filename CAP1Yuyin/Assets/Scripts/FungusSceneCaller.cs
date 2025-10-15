using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FungusBlockExecutodsr : MonoBehaviour
{
    void Awake()
    {
        // CRITICAL: Awake se ejecuta ANTES que Start de Fungus
        // Verifica si hay un bloque pendiente de ejecutar
        if (PlayerPrefs.HasKey("BloqueFungusAEjecutar"))
        {
            string nombreBloque = PlayerPrefs.GetString("BloqueFungusAEjecutar");

            // Limpia el PlayerPrefs
            PlayerPrefs.DeleteKey("BloqueFungusAEjecutar");
            PlayerPrefs.Save();

            Debug.Log($">>> Detectado bloque pendiente: {nombreBloque}");

            // Busca el Flowchart
            Flowchart flowchart = FindObjectOfType<Flowchart>();

            if (flowchart != null)
            {
                // Desactivar temporalmente para evitar Game Started
                flowchart.enabled = false;

                // Ejecutar el bloque correcto después
                StartCoroutine(EjecutarBloquePersonalizado(flowchart, nombreBloque));
            }
            else
            {
                Debug.LogError("ERROR: No se encontró Flowchart en la escena");
            }
        }
        else
        {
            Debug.Log(">>> No hay bloque pendiente, Fungus iniciará normalmente con Game Started");
        }
    }

    private IEnumerator EjecutarBloquePersonalizado(Flowchart flowchart, string nombreBloque)
    {
        // Esperar a que la escena termine de cargar
        yield return new WaitForEndOfFrame();

        // Reactivar el Flowchart
        flowchart.enabled = true;

        // Esperar un frame más para que Fungus se inicialice
        yield return null;

        // DETENER cualquier bloque que se haya iniciado automáticamente
        flowchart.StopAllBlocks();

        // Ejecutar el bloque correcto
        Debug.Log($">>> Ejecutando bloque guardado: {nombreBloque}");
        flowchart.ExecuteBlock(nombreBloque);
    }
}