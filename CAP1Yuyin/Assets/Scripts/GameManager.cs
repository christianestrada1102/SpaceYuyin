using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Escenas")]
    public string escenaActual = "Minijuego3.2";
    public string siguienteEscena = "Minijuego3.4";

    [Header("Fungus")]
    public Fungus.Flowchart fungus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔹 Método para cambiar de escena SIN parámetros (usa la variable interna)
    public void CambiarEscena()
    {
        Debug.Log("Cambiando a escena: " + siguienteEscena);
        SceneManager.LoadScene(siguienteEscena);
    }

    // 🔹 Método para cambiar de escena CON nombre específico
    public void CambiarEscena(string nombreEscena)
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            Debug.Log("Cambiando a escena: " + nombreEscena);
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogError("Intentaste cambiar a una escena vacía o no asignada.");
        }
    }

    // 🔹 Llamado cuando termina la animación de líneas
    public void OnAnimacionLineasCompletada()
    {
        Debug.Log(">> Animación de líneas completada.");

        // Si hay un Flowchart asignado, ejecuta el bloque correspondiente
        if (fungus != null)
        {
            fungus.ExecuteBlock("AfterConste");
            Debug.Log("Ejecutando bloque Fungus: AfterConste");
        }
        else
        {
            Debug.LogWarning("No se asignó un Flowchart de Fungus en GameManager.");
        }
    }
}
