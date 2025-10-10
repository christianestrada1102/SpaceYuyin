using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [Header("UI")]
    public GameObject panelGameOver;
    public Button buttonRestart;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        if (buttonRestart != null)
        {
            buttonRestart.onClick.AddListener(Reiniciar);
        }
    }

    public void MostrarGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Reiniciar()
    {
        Debug.Log("¡Reiniciando juego!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}