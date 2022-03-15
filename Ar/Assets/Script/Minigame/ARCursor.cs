using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class ARCursor : MonoBehaviour
{
    public PlayerData PD;
    public WaMManager WC;
    public ThrowCanManager TCM;
    public TowerStackManager TSM;

    public GameObject CursorChildObject;
    public GameObject ObjectToPlace;    
    public GameObject ReadyBtn;

    public List<GameObject> LevelCursorTC = new List<GameObject>();
    public List<GameObject> LevelCursorWaM = new List<GameObject>();
    public GameObject LevelCursorTSM;

    [SerializeField] bool UseCursor = true;
    [SerializeField] bool Placed = false;

    private Vector2 screenPosition;


    void Start()
    {
        PD = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (PD.GameMode == PlayerData.GameModeArray.ThrowCan)
        {
            CursorChildObject = Instantiate(LevelCursorTC[PD.level], gameObject.transform);
            CursorChildObject.SetActive(UseCursor);
            TCM = GameObject.FindGameObjectWithTag("GameController").GetComponent<ThrowCanManager>();
        }
        if (PD.GameMode == PlayerData.GameModeArray.WhackaMole)
        {
            CursorChildObject = Instantiate(LevelCursorWaM[0], gameObject.transform);
            CursorChildObject.SetActive(UseCursor);
            WC = GameObject.FindGameObjectWithTag("GameController").GetComponent<WaMManager>();
            WC.moleContainer = CursorChildObject.transform.GetChild(0).gameObject;
        }
        if (PD.GameMode == PlayerData.GameModeArray.TowerStack)
        {
            CursorChildObject = Instantiate(LevelCursorTSM, gameObject.transform);
            CursorChildObject.SetActive(UseCursor);
            TSM = GameObject.FindGameObjectWithTag("GameController").GetComponent<TowerStackManager>();
        }
    }

    void Update()
    {
        if (PD.GameMode == PlayerData.GameModeArray.ThrowCan)
        {
            if (TCM.State == ThrowCanManager.GameState.Ready)
            {
                if (UseCursor)
                {
                    UpdateCursor();
                }
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.WhackaMole)
        {
            if (WC.State == WaMManager.GameState.Ready)
            {
                if (UseCursor)
                {
                    UpdateCursor();
                }
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.TowerStack)
        {
            if (TSM.State == TowerStackManager.GameState.Ready)
            {
                if (UseCursor)
                {
                    UpdateCursor();
                }
            }
        }
    }

    void UpdateCursor()
    {
        screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (PD.GameMode == PlayerData.GameModeArray.ThrowCan)
        {
            TCM.RayCastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
            if (TCM.Distance > 3.5)
            {
                ReadyBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                ReadyBtn.GetComponent<Button>().interactable = false;
            }
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.WhackaMole)
        {
            WC.RayCastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
            if (CursorChildObject.transform.position != Vector3.zero)
            {
                ReadyBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                ReadyBtn.GetComponent<Button>().interactable = false;
            }
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.TowerStack)
        {
            TSM.RayCastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
            if (CursorChildObject.transform.position != Vector3.zero)
            {
                ReadyBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                ReadyBtn.GetComponent<Button>().interactable = false;
            }
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }
    }
    public void Ready()
    {
        ReadyBtn.SetActive(false);
        if (PD.GameMode == PlayerData.GameModeArray.ThrowCan)
        {
            TCM.State = ThrowCanManager.GameState.Countdown;
            CursorChildObject.SetActive(false);
            Destroy(gameObject, 0f);
            TCM.planeManager.enabled = false;
            if (UseCursor && !Placed)
            {
                Placed = true;
                TCM.currentCanSet = GameObject.Instantiate(LevelCursorTC[PD.level], transform.position, transform.rotation);
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.WhackaMole)
        {
            WC.State = WaMManager.GameState.Countdown;
            CursorChildObject.SetActive(false);
            Destroy(gameObject, 0f);
            WC.planeManager.enabled = false;
            if (UseCursor && !Placed)
            {
                Placed = true;
                WC.moleBoard = GameObject.Instantiate(LevelCursorWaM[0], transform.position, transform.rotation);
                WC.moleContainer = WC.moleBoard.transform.GetChild(0).gameObject;
            }
        }
        if (PD.GameMode == PlayerData.GameModeArray.TowerStack)
        {
            TSM.State = TowerStackManager.GameState.Countdown;
            CursorChildObject.SetActive(false);
            Destroy(gameObject, 0f);
            TSM.planeManager.enabled = false;
            if (UseCursor && !Placed)
            {
                Placed = true;
                TSM.CurrentCube = GameObject.Instantiate(LevelCursorTSM, transform.position, transform.rotation);
            }
        }
    }
}