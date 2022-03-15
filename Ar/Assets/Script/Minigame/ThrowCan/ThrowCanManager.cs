using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ThrowCanManager : MonoBehaviour
{
    public SwipeScript BallSwipe;
    public PlayerData PD;
    public ARCursor ARCursor;

    public int SpawnedBall =0; //Show how many balls was spawned
    [Tooltip("Reducing Game Time")]
    [SerializeField] private float currentTimer = 0f; // Modifying game timer
    [Header("Clear Timer for Stars")]
    [Tooltip("Enter only 3* & 1* clear time, 2* is between both")]
    [SerializeField] private float clear3Timer = 0f;
    [Tooltip("Enter only 3* & 1* clear time, 2* is between both")]
    [SerializeField] private float clear1Timer = 0f;

    [Tooltip("Total Game time")]
    public float GameTimer; // Total Game time set to
    private float currentCountdown = 0f; // Modifying countdown timer
    [Tooltip("Total Countdown time")]
    public float CountDownTimer; // Total Countdown timer set to
    public float DistanceForce; // For calculate force for Distance throwing
    public float Distance; // For calculate Distance between camera and GameObject


    [SerializeField] private List<GameObject> Ball = new List<GameObject>(); // List of types of ball
    [SerializeField] private List<GameObject> Stars = new List<GameObject>(); // List for star collection
    public List<GameObject> Cans = new List<GameObject>(); // List of the cans in the game spawned
    public GameObject currentCanSet; // The prefab to be spawned for ARCursor.cs
    public TextMeshProUGUI CdTimer; // Countdown Timer Text
    public TextMeshProUGUI GameTimeText; // Game Timer Text
    public TextMeshProUGUI spawnedBallText; // Total spawned ball text
    public GameObject warningPanel; // Panel of warning
    public GameObject PausePanel; // Panel of pausing
    public GameObject TallyPanel; // Panel of tally
    public TextMeshProUGUI tallyTimeTxt; // Text of total time used on tally panel
    public ARRaycastManager RayCastManager;
    public ARPlaneManager planeManager;
    public enum GameState { Ready,Countdown,Start,Pause,End};
    public GameState State;
    private bool firstClear = true;

    void Start()
    {
        currentTimer = GameTimer;
        currentCountdown = CountDownTimer;
        State = GameState.Ready;
        PausePanel.SetActive(false);
        warningPanel.SetActive(false);
        TallyPanel.SetActive(false);
        CdTimer.gameObject.SetActive(false);
        PD = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (PD.clearStar[PD.level] != 0)
        {
            firstClear = false;
        }
    }

    void Update()
    {
        if (State == GameState.Ready)
        {
            Vector3 v3 = ARCursor.CursorChildObject.transform.position - Camera.main.transform.position;
            Distance = Vector3.Dot(v3, Camera.main.transform.forward);
            DistanceForce = Distance;
        }
        if (State == GameState.Countdown)
        {
            Countdown();
        }
        if (State == GameState.Start)
        {
            Vector3 v3 = currentCanSet.transform.position - Camera.main.transform.position;
            Distance = Vector3.Dot(v3, Camera.main.transform.forward);
            DistanceForce = Distance;
            //if cans are visable in the camera or Distance too close
            if (!IsVisible(Camera.main)|| Distance < 3.5f)
            {
                //pause
                State = GameState.Pause;
                warningPanel.SetActive(true);
            }
            spawnedBallText.gameObject.SetActive(true);
            spawnedBallText.text = "Ball: " + SpawnedBall.ToString();
            //Always spawn ball when there is no ball
            if (BallSwipe == null)
            {
                SpawnedBall += 1;
                GameObject ball = Instantiate(Ball[0], Camera.main.transform);
                BallSwipe = ball.GetComponent<SwipeScript>();
                BallSwipe.enabled = true;
            }
            //Check if all can is down
            if (Cans.Count == 0)
            {
                State = GameState.End;
                Clear();
            }
            //Remove all additional can
            for(int i =0; i < Cans.Count; i++)
            {
                if (Cans[i].name == "")
                {
                    Cans.Remove(Cans[i]);
                }
            }
            GameTimeText.gameObject.SetActive(true);
            currentTimer -= Time.deltaTime;
            GameTimeText.text = currentTimer.ToString("0");
            if (currentTimer < 0f)
            {
                State = GameState.End;
                GameOver();
            }
        }
        if (State == GameState.Pause)
        {
            if (BallSwipe != null)
            {
                BallSwipe.enabled = false;
            }
            if (currentCanSet != null)
            {
                if (!PausePanel.activeInHierarchy)
                {
                    Vector3 v3 = currentCanSet.transform.position - Camera.main.transform.position;
                    Distance = Vector3.Dot(v3, Camera.main.transform.forward);
                    if (IsVisible(Camera.main) && Distance > 3.5f)
                    {
                        //unpause
                        State = GameState.Countdown;
                        warningPanel.SetActive(false);
                    }
                }
               
            }            
        }
        if(State== GameState.End)
        {
            warningPanel.SetActive(false);
            PausePanel.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseBtn();
        }
    }    

    private void Countdown()
    {
        CdTimer.gameObject.SetActive(true);
        CdTimer.text = currentCountdown.ToString("0");
        if (currentCountdown > 0f)
        {
            currentCountdown -= Time.deltaTime;
        }
        else
        {
            State = GameState.Start;
            currentCountdown = CountDownTimer;
            CdTimer.gameObject.SetActive(false);
        }
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        if (BallSwipe != null)
        {
            BallSwipe.enabled = true;
        }
    }
    private void Clear()
    {
        TallyPanel.SetActive(true);
        tallyTimeTxt.text = ((currentTimer - GameTimer)*-1).ToString("0");
        int clearedState=0; //To determine the current supposed clearing star == clear time
        if (currentTimer > clear3Timer)
        {
            //3 Star Clear
            Debug.Log("3*");
            clearedState = 3;
            Stars[2].SetActive(true);
            if(clearedState> PD.clearStar[PD.level])
            {
                PD.clearStar[PD.level] = clearedState;
            }            
            if (firstClear)
            {
            }
            else
            {
            }
        }
        else if (currentTimer < clear3Timer && GameTimer > clear1Timer)
        {
            //2 Star Clear
            Debug.Log("2*");
            clearedState = 2;
            Stars[1].SetActive(true);
            if (clearedState > PD.clearStar[PD.level])
            {
                PD.clearStar[PD.level] = clearedState;
            }
            if (firstClear)
            {
            }
            else
            {
            }
        }
        else if (currentTimer < clear1Timer)
        {
            //1 Star Clear
            Debug.Log("1*");
            clearedState = 1;
            Stars[0].SetActive(true);
            if (clearedState > PD.clearStar[PD.level])
            {
                PD.clearStar[PD.level] = clearedState;
            }
            if (firstClear)
            {
            }
            else
            {
            }
        }
    }

    // Check if cans are visible in cam
    private bool IsVisible(Camera c)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        foreach (var plane in planes)
        {
            for (int i = 0; i < Cans.Count; i++)
            {
                if (plane.GetDistanceToPoint(Cans[i].transform.position) > 0)
                {
                    Debug.Log("cAn: "+Cans[i].name);
                    break;
                }
                else if(i==Cans.Count-1)
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
        }
        return true;
    }

    private void GameOver()
    {
        TallyPanel.SetActive(true);
        tallyTimeTxt.text = (currentTimer - GameTimer).ToString("0");
    }

    public void ReturnBtn()
    {
        SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
    }

    public void AgainBtn()
    {
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void PauseBtn()
    {
        PausePanel.SetActive(true);
        State = GameState.Pause;
    }
}
