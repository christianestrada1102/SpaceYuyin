using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEstrellas : MonoBehaviour
{
    [Header("Referencias")]
    public Estrellas controladorEstrellas;

    [Header("Configuración")]
    public float distanciaSnap = 1.5f;
    public float velocidadRetorno = 10f;

    private Camera cam;
    private Vector3 posicionInicial;
    private bool arrastrando = false;
    private bool colocada = false;
    private Transform destinoAsignado = null;
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;
    private float zOriginal;

    private void Start()
    {
        cam = Camera.main;
        posicionInicial = transform.position;
        zOriginal = transform.position.z;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (controladorEstrellas == null)
        {
            controladorEstrellas = FindObjectOfType<Estrellas>();
        }
    }

    private void OnMouseDown()
    {
        if (!colocada)
        {
            arrastrando = true;

            // Calcular offset entre mouse y posición del objeto
            Vector3 posMouse = GetMouseWorldPosition();
            offset = transform.position - posMouse;

            // Traer al frente
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 10;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (arrastrando && !colocada)
        {
            Vector3 posMouse = GetMouseWorldPosition();
            transform.position = new Vector3(
                posMouse.x + offset.x,
                posMouse.y + offset.y,
                zOriginal
            );
        }
    }

    private void OnMouseUp()
    {
        if (arrastrando && !colocada)
        {
            arrastrando = false;

            // Buscar el destino más cercano disponible
            Transform destinoCercano = controladorEstrellas.GetClosestAvailableDestination(transform.position);

            if (destinoCercano != null)
            {
                float distancia = Vector3.Distance(transform.position, destinoCercano.position);

                if (distancia <= distanciaSnap)
                {
                    // Colocar en el destino
                    ColocarEnDestino(destinoCercano);
                }
                else
                {
                    // Regresar a posición inicial
                    RegresarAPosicionInicial();
                }
            }
            else
            {
                // No hay destinos disponibles, regresar
                RegresarAPosicionInicial();
            }

            // Restaurar orden de renderizado
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 0;
            }
        }
    }

    private void ColocarEnDestino(Transform destino)
    {
        colocada = true;
        destinoAsignado = destino;
        transform.position = new Vector3(destino.position.x, destino.position.y, zOriginal);

        // Marcar destino como ocupado
        controladorEstrellas.MarcarDestino(destino, true);

        Debug.Log($"Estrella {gameObject.name} colocada en {destino.name}");
    }

    private void RegresarAPosicionInicial()
    {
        StartCoroutine(AnimarRetorno());
    }

    private System.Collections.IEnumerator AnimarRetorno()
    {
        while (Vector3.Distance(transform.position, posicionInicial) > 0.01f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                posicionInicial,
                Time.deltaTime * velocidadRetorno
            );
            yield return null;
        }

        transform.position = posicionInicial;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 posMouse = Input.mousePosition;
        posMouse.z = Mathf.Abs(cam.transform.position.z - zOriginal);
        return cam.ScreenToWorldPoint(posMouse);
    }

    // Método opcional para resetear la estrella
    public void ResetearEstrella()
    {
        if (colocada && destinoAsignado != null)
        {
            controladorEstrellas.MarcarDestino(destinoAsignado, false);
            controladorEstrellas.estrellasColocadas--;
        }

        colocada = false;
        destinoAsignado = null;
        transform.position = posicionInicial;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 0;
        }
    }
}