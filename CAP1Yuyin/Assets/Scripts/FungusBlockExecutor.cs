using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusBlockExecutor : MonoBehaviour
{
    void Start()
    {
        // Verifica si hay un bloque pendiente de ejecutar
        if (PlayerPrefs.HasKey("BloqueFungusAEjecutar"))
        {
            string nombreBloque = PlayerPrefs.GetString("BloqueFungusAEjecutar");

            // Limpia el PlayerPrefs
            PlayerPrefs.DeleteKey("BloqueFungusAEjecutar");
            PlayerPrefs.Save();

            // Busca el Flowchart y ejecuta el bloque
            Flowchart flowchart = FindObjectOfType<Flowchart>();

            if (flowchart != null)
            {
                Debug.Log($">>> Ejecutando bloque guardado: {nombreBloque}");
                flowchart.ExecuteBlock(nombreBloque);
            }
            else
            {
                Debug.LogError("ERROR: No se encontró Flowchart en la escena");
            }
        }
    }
}