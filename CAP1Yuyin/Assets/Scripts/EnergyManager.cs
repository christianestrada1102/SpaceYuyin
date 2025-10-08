using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;

    [Header("Configuración de Energía")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;

    [Header("UI")]
    public Slider energySlider;
    public Text energyText;

    [Header("Victoria")]
    public GameObject panelVictoria;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentEnergy = 0f;
        UpdateUI();

        // Asegurar que el panel esté oculto al inicio
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        UpdateUI();

        Debug.Log("Energía: " + currentEnergy + "/" + maxEnergy);

        // Verificar si completó la misión
        if (currentEnergy >= maxEnergy)
        {
            MisionCompletada();
        }
    }

    void MisionCompletada()
    {
        Debug.Log("¡MISIÓN CUMPLIDA!");

        // Mostrar panel de victoria
        if (panelVictoria != null)
            panelVictoria.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;
    }
    void UpdateUI()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy;
        }

        if (energyText != null)
        {
            energyText.text = Mathf.Round(currentEnergy) + "/" + maxEnergy;
        }
    }
}