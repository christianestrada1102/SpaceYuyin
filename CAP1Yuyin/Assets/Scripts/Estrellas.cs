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


    private void Start()
    {
        // Inicializar contador en 0
        estrellasColocadas = 0;

        foreach (var destino in destinos)
        {
            estadoDestinos[destino] = false;
        }

        Debug.Log($"Estrellas inicializadas. Total: {totalEstrellas}, Colocadas: {estrellasColocadas}");
    }

    public void MarcarDestino(Transform destino, bool ocupado)
    {
        if (estadoDestinos.ContainsKey(destino))
        {
            // Solo contar si el estado cambia
            bool estadoAnterior = estadoDestinos[destino];
            estadoDestinos[destino] = ocupado;

            if (ocupado && !estadoAnterior)
            {
                estrellasColocadas++;
                Debug.Log($"Estrella colocada. Progreso: {estrellasColocadas}/{totalEstrellas}");
            }
            else if (!ocupado && estadoAnterior)
            {
                estrellasColocadas--;
                Debug.Log($"Estrella removida. Progreso: {estrellasColocadas}/{totalEstrellas}");
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