using UnityEngine;

public class ConstellationLines : MonoBehaviour
{
    // Este método se llama cuando la animación de líneas termina
    public void AnimacionTerminada()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAnimacionLineasCompletada();
        }
        else
        {
            Debug.LogError("ERROR: No se encontró GameManager en la escena.");
        }
    }
}
