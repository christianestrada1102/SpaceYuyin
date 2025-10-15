using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Estrellas : MonoBehaviour
{
    [Header("Destinos (Slots)")]
    public List<Transform> destinos;
    public Dictionary<Transform, bool> estadoDestinos = new Dictionary<Transform, bool>();

    [Header("Control de progreso")]
    public int estrellasColocadas = 0;
    public int totalEstrellas = 3;

    [Header("Fungus")]
    public Fungus.Flowchart fungus;

    [Header("Escenas")]
    public string siguienteEscena = "Minijuego3.4";
    public string escenaActual = "Minijuego3.2";

    private void Start()
    {
        foreach (var destino in destinos)
        {
            estadoDestinos[destino] = false;
        }
    }

    public void MarcarDestino(Transform destino, bool ocupado)
    {
        if (estadoDestinos.ContainsKey(destino))
        {
            estadoDestinos[destino] = ocupado;

            if (ocupado)
            {
                estrellasColocadas++;
                ComprobarVictoria();
            }
        }
    }

    private void ComprobarVictoria()
    {
        if (estrellasColocadas >= totalEstrellas)
        {
            Debug.Log("¡Minijuego completado!");

            if (SceneManager.GetActiveScene().name == escenaActual)
            {
                GameManager.Instance.CambiarEscena(siguienteEscena);
            }
            else
            {
                if (fungus != null)
                {
                    fungus.ExecuteBlock("AfterConste");
                    Debug.Log("Ejecutando bloque Fungus: AfterConste");
                }
                else
                {
                    Debug.LogWarning("No se asignó el Flowchart de Fungus en Estrellas.");
                }
            }
        }
    }

    public Transform GetClosestAvailableDestination(Vector3 posicionActual)
    {
        Transform destinoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (var par in estadoDestinos)
        {
            if (!par.Value)
            {
                float distancia = Vector3.Distance(posicionActual, par.Key.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    destinoMasCercano = par.Key;
                }
            }
        }

        return destinoMasCercano;
    }
}
