using UnityEngine;
using Fungus;

public class FungusEntryPoint : MonoBehaviour
{
    [Tooltip("Arrastra aquí tu Flowchart de Fungus")]
    public Flowchart flowchart;

    [Tooltip("Bloque por defecto al iniciar la novela desde cero")]
    public string bloqueInicial = "Inicio";

    [Tooltip("Bloque al que se regresa después de un minijuego")]
    public string bloqueAfterMini = "AfterMini";

    private bool yaEjecutado = false;

    void Start()
    {
        // Evitar ejecuciones dobles si hay duplicados
        if (yaEjecutado)
        {
            Debug.Log(">>> FungusEntryPoint: Ya se ejecutó anteriormente, evitando reinicio duplicado.");
            return;
        }

        yaEjecutado = true;

        // Revisar si venimos de un minijuego
        bool fromMini = PlayerPrefs.GetInt("FromMiniGame", 0) == 1;
        string nextBlock = fromMini ? bloqueAfterMini : bloqueInicial;

        Debug.Log($">>> FungusEntryPoint: Iniciando Flowchart en bloque '{nextBlock}' (FromMiniGame={fromMini})");

        if (flowchart != null)
        {
            flowchart.ExecuteBlock(nextBlock);
        }
        else
        {
            Debug.LogError("[FungusEntryPoint] No se asignó el Flowchart en el Inspector.");
        }

        // Si veníamos del minijuego, limpiar el flag
        if (fromMini)
        {
            PlayerPrefs.DeleteKey("FromMiniGame");
            PlayerPrefs.Save();
        }
    }
}
