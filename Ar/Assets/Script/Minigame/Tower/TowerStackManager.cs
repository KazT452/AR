using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class TowerStackManager : MonoBehaviour
{
    public ARRaycastManager RayCastManager;
    public ARPlaneManager planeManager;
    public ARCursor ArCursor;

    public TextMeshProUGUI CdTimer;
    public TextMeshProUGUI ScoreText;
    public GameObject TallyPanel;
    public GameObject PausePanel;
    public GameObject CurrentCube;
    public GameObject LastCube;

    public int Level;
    public int Score;
    public bool Done;
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] float speed = 0f;
    bool endPoint;
    [SerializeField] int streak;
    [SerializeField] List<Transform> towerTransform = new List<Transform>();

    //Timer
    private float currentCountdown = 0f;
    public float CountDownTimer;

    public enum GameState { Ready, Countdown, Start, Pause, End };
    public GameState State;
    // Start is called before the first frame update
    void Start()
    {
        State = GameState.Ready;
        currentCountdown = CountDownTimer;
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
            ScoreText.text = Score.ToString();
            if (Done)
            {
                GameOver();
                return;
            }

            pos1 = (LastCube.transform.position + Vector3.up * 10) + (((Level % 2 == 0) ? Vector3.left : Vector3.back) * -120);
            pos2 = pos1 + ((Level % 2 == 0) ? Vector3.left : Vector3.back) * 240;
            CurrentCube.transform.localPosition = Vector3.Lerp(pos2, pos1, speed);
            if (speed <= 1f && !endPoint)
            {
                speed += Time.deltaTime * (Mathf.Pow(Level, 0.4f) / 10);

                if (speed >= 1f)
                {
                    endPoint = true;
                }
            }
            else if (speed >= 0f && endPoint)
            {
                speed -= Time.deltaTime * (Mathf.Pow(Level, 0.4f) / 10);
                if (speed <= 0f)
                {
                    endPoint = false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                NewBlock();
            }
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
            NewBlock();
        }
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    private void NewBlock()
    {
        Debug.Log("newBlock");

        if (LastCube != null)
        {
            CurrentCube.transform.position = new Vector3(Mathf.Round(CurrentCube.transform.position.x),
                CurrentCube.transform.position.y,
                Mathf.Round(CurrentCube.transform.position.z));
            CurrentCube.transform.localScale = new Vector3(LastCube.transform.localScale.x - Mathf.Abs(CurrentCube.transform.position.x - LastCube.transform.position.x),
                LastCube.transform.localScale.y,
                LastCube.transform.localScale.z - Mathf.Abs(CurrentCube.transform.position.z - LastCube.transform.position.z));
            CurrentCube.transform.position = Vector3.Lerp(CurrentCube.transform.position, LastCube.transform.position, 0.5f) + Vector3.up * 5f;
            if (CurrentCube.transform.localScale.x <= 0f || CurrentCube.transform.localScale.z <= 0f)
            {
                Done = true;
                ScoreText.gameObject.SetActive(true);
                ScoreText.text = "Your Score " + Level;
                return;
            }
            //Check if new block is symmetrical to old block
            if (Mathf.RoundToInt(-CurrentCube.transform.localScale.x + LastCube.transform.localScale.x) <= 1f
                && Mathf.RoundToInt(-CurrentCube.transform.localScale.z + LastCube.transform.localScale.z) <= 1f)
            {
                streak++;
            }
            else
            {
                streak = 0;
            }
            if (streak < 5)
            {
                Score++;
            }
            else
            {
                Score += 2;
            }
        }
        towerTransform.Add(CurrentCube.transform);
        LastCube = CurrentCube;
        CurrentCube = Instantiate(LastCube);
        CurrentCube.name = Level + "";
        CurrentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((Level / 100f) % 1f, 1f, 1f));
        Level++;
        speed = 0f;
        if (Level > 20)
        {
            for(int i = 0; i < towerTransform.Count; i++)
            {
                towerTransform[i].localPosition = new Vector3(towerTransform[i].localPosition.x, towerTransform[i].localPosition.y - 10f, towerTransform[i].localPosition.z);
            }
        }
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

    private void GameOver()
    {
        State = GameState.End;
        TallyPanel.SetActive(true);
    }
}
