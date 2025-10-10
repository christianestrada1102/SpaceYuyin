using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FungusSceneCaller : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Nombre de la escena de Fungus a cargar")]
    public string nombreEscenaFungus = "EscenaFungus";

    [Header("Configuración de Fungus")]
    [Tooltip("Nombre del bloque a ejecutar en Fungus")]
    public string nombreBloque = "NombreDelBloque";

    // Método público para llamar desde el botón
    public void AlPresionarBoton()
    {
        // Guardar el nombre del bloque en PlayerPrefs
        PlayerPrefs.SetString("BloqueFungusAEjecutar", nombreBloque);
        PlayerPrefs.Save();

        Debug.Log($">>> Guardando bloque para ejecutar: {nombreBloque}");
        Debug.Log($">>> Cambiando a escena: {nombreEscenaFungus}");

        // Cargar la escena de Fungus
        SceneManager.LoadScene(nombreEscenaFungus);
    }

    // Método alternativo si quieres ejecutar con parámetros desde código
    public void CambiarEscenaYEjecutarBloque(string escena, string bloque)
    {
        PlayerPrefs.SetString("BloqueFungusAEjecutar", bloque);
        PlayerPrefs.Save();

        Debug.Log($">>> Guardando bloque para ejecutar: {bloque}");
        Debug.Log($">>> Cambiando a escena: {escena}");

        SceneManager.LoadScene(escena);
    }
}