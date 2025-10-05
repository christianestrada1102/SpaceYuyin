using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StarDragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private GameManager solver; // Referencia al GameManager
    private bool estaBloqueada = false; // ¡NUEVA VARIABLE! Controla si la estrella está bloqueada

    void Start()
    {
        // Se busca el GameManager en la escena.
        solver = FindObjectOfType<GameManager>();
        if (solver == null)
        {
            Debug.LogError("ERROR: No se encontró el GameManager. ¡El arrastre no funcionará!");
            this.enabled = false; // Se desactiva para evitar errores si no hay GameManager
        }
    }

    // Función auxiliar para convertir la posición del mouse (en píxeles de pantalla) a coordenadas del mundo (escena)
    private Vector3 GetMouseWorldPosition()
    {
        // Coordenadas del mouse en pantalla (x, y) + la coordenada z que guardamos
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        // Convertir las coordenadas de pantalla a coordenadas del mundo
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Se llama cuando el usuario presiona el botón del mouse sobre el Collider de este objeto
    void OnMouseDown()
    {
        // ¡VERIFICACIÓN CRÍTICA! Si está bloqueada, no permitir arrastrar
        if (estaBloqueada) return;

        // Almacena la coordenada Z para mantener la estrella en el plano 2D correcto.
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Calcula el 'offset' (desplazamiento) para arrastrar desde el punto donde se hizo clic.
        offset = gameObject.transform.position - GetMouseWorldPosition();
    }

    // Se llama mientras el usuario arrastra el mouse con el botón presionado
    void OnMouseDrag()
    {


        // ¡VERIFICACIÓN CRÍTICA! Si está bloqueada, no permitir movimiento
        if (estaBloqueada) return;

        // Mueve el objeto a la nueva posición del mouse con el offset
        transform.position = GetMouseWorldPosition() + offset;
    }

    // Se llama cuando el usuario suelta el botón del mouse
    void OnMouseUp()
    {
        // ¡VERIFICACIÓN CRÍTICA! Si está bloqueada, no hacer nada
        if (estaBloqueada) return;

        // Si el script ya está deshabilitado (bloqueado), salimos inmediatamente.
        if (!this.enabled) return;

        // 1. Pídele al GameManager que encuentre el destino LIBRE más cercano.
        Transform destinoLibreCercano = solver.GetClosestAvailableDestination(transform.position);

        if (destinoLibreCercano != null)
        {
            // 2. Comprueba si la estrella está lo suficientemente cerca para "encajar".
            float distancia = Vector3.Distance(transform.position, destinoLibreCercano.position);

            // Umbral de cercanía: (¡Asegúrate de que este valor sea lo suficientemente grande!)
            float umbralDeEncaje = .5f;

            if (distancia < umbralDeEncaje)
            {
                // ¡ACCIÓN CLAVE 1: ENCAJE FORZADO (Snapping)!
                transform.position = destinoLibreCercano.position;

                // ¡ACCIÓN CLAVE 2: BLOQUEO! Notifica al GameManager y marca como bloqueada
                solver.MarkDestinationAsOccupied(destinoLibreCercano);
                estaBloqueada = true; // ¡AHORA SÍ SE BLOQUEA CORRECTAMENTE!

                // ¡EFECTO DE BRILLO!
               

                Debug.Log(gameObject.name + " bloqueada en su destino. ¡Completado!");
            }
        }
    }
}