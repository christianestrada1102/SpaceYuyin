using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;

    [Header("Configuraci�n de Energ�a")]
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

        // Asegurar que el panel est� oculto al inicio
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        UpdateUI();

        Debug.Log("Energ�a: " + currentEnergy + "/" + maxEnergy);

        // Verificar si complet� la misi�n
        if (currentEnergy >= maxEnergy)
        {
            MisionCompletada();
        }
    }

    void MisionCompletada()
    {
        Debug.Log("�MISI�N CUMPLIDA!");

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