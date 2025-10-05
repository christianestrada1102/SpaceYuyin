using UnityEngine;
using UnityEngine.EventSystems;

public class BotonManualClick : MonoBehaviour, IPointerClickHandler
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("ERROR: No se encontró GameManager!");
        }
        else
        {
            Debug.Log(">>> BotonManualClick inicializado correctamente");
        }
    }

    // Este método se llama cuando se hace clic en el objeto
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(">>> ========================================");
        Debug.Log(">>> ¡¡¡CLIC DETECTADO EN EL BOTÓN!!!");
        Debug.Log(">>> ========================================");

        if (gameManager != null)
        {
            gameManager.CambiarEscena();
        }
        else
        {
            Debug.LogError(">>> GameManager es NULL!");
        }
    }

    // Método alternativo que puede ser llamado desde el Inspector
    public void OnClick()
    {
        Debug.Log(">>> OnClick() llamado desde Inspector");

        if (gameManager != null)
        {
            gameManager.CambiarEscena();
        }
    }
}
