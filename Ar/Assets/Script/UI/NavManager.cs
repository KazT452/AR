using UnityEngine;
using UnityEngine.SceneManagement;

public class NavManager : MonoBehaviour
{
    public PlayerData PD;
    public enum GameScene { MainMenu, ThrowCan, WhackaMole, Stack };
    public GameScene GS;
    // Start is called before the first frame update
    public static NavManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PD = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void ChangeScene(string gameScene)
    {
        SceneManager.LoadScene(gameScene,LoadSceneMode.Single);
        if (gameScene == "ThrowCan")
        {
            PD.GameMode = PlayerData.GameModeArray.ThrowCan;
        }
        else if (gameScene == "WhackAMole")
        {
            PD.GameMode = PlayerData.GameModeArray.WhackaMole;
        }
        else if (gameScene == "TowerStack")
        {
            PD.GameMode = PlayerData.GameModeArray.TowerStack;
        }
    }
}
