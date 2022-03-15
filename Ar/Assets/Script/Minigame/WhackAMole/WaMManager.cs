using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class WaMManager : MonoBehaviour
{
    public Mole Mole;
    public ARRaycastManager RayCastManager;
    public ARPlaneManager planeManager;
    public WaMControl waWControl;


    public GameObject moleContainer;
    public List<Transform> molePos = new List<Transform>();
    public GameObject moleBoard;
    [SerializeField] public static int Score = 0;
    //Timer
    private float currentTimer = 0f;
    public float GameTimer;
    private float currentCountdown = 0f;
    public float CountDownTimer;

    public TextMeshProUGUI CdTimer;
    public TextMeshProUGUI GameTimeText;
    public TextMeshProUGUI ScoreText;
    public GameObject TallyPanel;
    public GameObject PausePanel;

    public float ShowMoleTimer; // Editor Timer setting for mole showing
    private float moleshowTimer; // Timer to count mole showing
    public enum GameState { Ready, Countdown, Start, Pause, End };
    public GameState State;

    // Start is called before the first frame update
    void Start()
    {
        State = GameState.Ready;
        moleshowTimer = ShowMoleTimer;
        currentCountdown = CountDownTimer;
        currentTimer = GameTimer;
        TallyPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Countdown)
        {
            Countdown();
        }
        if (State == GameState.Start)
        {
            ScoreText.text = "Score: " + Score.ToString();
            GameTimeText.text = currentTimer.ToString();
            GameTimer -= Time.deltaTime;
            
            if (GameTimer > 0f||Player.Health>0)
            {
                moleshowTimer -= Time.deltaTime;
                if (moleshowTimer < 0f)
                {
                    molePos[UnityEngine.Random.Range(0, molePos.Count)].GetComponent<Mole>().ShowMole();
                    moleshowTimer = ShowMoleTimer;
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    private void Countdown()
    {
        CdTimer.gameObject.SetActive(true);
        CdTimer.text = currentCountdown.ToString("0");
        if (molePos.Count < moleContainer.transform.childCount)
        {
            for (int i = 0; i < moleContainer.transform.childCount; i++)
            {
                molePos.Add(moleContainer.transform.GetChild(i).gameObject.GetComponent<Transform>());
            }
        }
        if (currentCountdown > 0f)
        {
            currentCountdown -= Time.deltaTime;
        }
        else
        {
            State = GameState.Start;
            currentCountdown = CountDownTimer;
            CdTimer.gameObject.SetActive(false);
            molePos[UnityEngine.Random.Range(0, molePos.Count)].GetComponent<Mole>().ShowMole();
            moleshowTimer = ShowMoleTimer;
        }
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    private void GameOver()
    {
        State = GameState.End;
        TallyPanel.SetActive(true);
    }

    private void RemoveElement<T>(ref T[] arr, int index)
    {
        for(int i = index;i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        Array.Resize(ref arr, arr.Length - 1);
    }

    public void ReturnBtn()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
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
