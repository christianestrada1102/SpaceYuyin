using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Transición a Novela")]
    [Tooltip("Nombre exacto de la escena de Fungus/novela")]
    public string escenaNovelaSiguiente = "NovelaEp1";
    [Tooltip("Nombre del bloque Fungus que debe ejecutarse al volver")]
    public string bloqueAfterMini = "AfterMini";
    [Tooltip("Segundos antes de cargar la escena de la novela")]
    public float tiempoAntesDeCargar = 2f;

    [Header("Debug/Test")]
    public bool testWinWithKey = true; // presionar tecla W para ganar durante pruebas

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // no hagas DontDestroyOnLoad aquí: este objeto no necesita persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateUI();

        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    void Update()
    {
        // utilidad para probar en editor: presiona W para simular victoria
        if (testWinWithKey && Input.GetKeyDown(KeyCode.W))
        {
            AddEnergy(maxEnergy); // fuerza completar
        }
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        UpdateUI();
        Debug.Log($"[EnergyManager] Energía: {currentEnergy}/{maxEnergy}");

        if (currentEnergy >= maxEnergy)
        {
            MisionCompletada();
        }
    }

    void MisionCompletada()
    {
        Debug.Log("[EnergyManager] ¡MISIÓN CUMPLIDA!");

        // Mostrar panel de victoria si existe
        if (panelVictoria != null)
            panelVictoria.SetActive(true);

        // Guardar el bloque que debe ejecutarse después del minijuego
        PlayerPrefs.SetString("NextFungusBlock", bloqueAfterMini);
        PlayerPrefs.SetInt("FromMiniGame", 1); // 👈 Marca que vienes del minijuego
        PlayerPrefs.Save();
        Debug.Log($">>> Guardado en PlayerPrefs: NextFungusBlock = {bloqueAfterMini}");

        // Aseguramos tiempo normal (por si el minijuego pausó el timeScale)
        Time.timeScale = 1f;

        // Lanzar la coroutine que espera y carga la escena
        StartCoroutine(WaitAndLoadNovela(tiempoAntesDeCargar));
    }

    IEnumerator WaitAndLoadNovela(float seconds)
    {
        // opcional: si quieres bloquear input o reproducir anim, hazlo aquí
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime; // cuenta aun si el timeScale cambia
            yield return null;
        }

        // Check de seguridad: que el nombre de escena no esté vacío
        if (string.IsNullOrEmpty(escenaNovelaSiguiente))
        {
            Debug.LogError("[EnergyManager] Nombre de escena de novela vacío. No se puede cargar la escena.");
            yield break;
        }

        Debug.Log($"[EnergyManager] Cargando escena de novela: {escenaNovelaSiguiente}");
        SceneManager.LoadScene(escenaNovelaSiguiente);
    }

    void UpdateUI()
    {
        if (energySlider != null && maxEnergy > 0f)
        {
            energySlider.value = currentEnergy / maxEnergy; // slider espera 0..1
        }

        if (energyText != null)
        {
            energyText.text = $"{Mathf.RoundToInt(currentEnergy)}/{Mathf.RoundToInt(maxEnergy)}";
        }
    }
}
