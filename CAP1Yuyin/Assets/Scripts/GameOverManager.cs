using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [Header("UI")]
    public GameObject panelGameOver;

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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}