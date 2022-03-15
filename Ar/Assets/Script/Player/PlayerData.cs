using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//public class Singleton<PlayerData> : MonoBehaviour where PlayerData : MonoBehaviour
public class PlayerData : MonoBehaviour
{




    private string userName;

    public int level;
    public List<Button> gmSelect = new List<Button>();
    public List<Button> levels = new List<Button>();
    public int[] clearStar = {0, 0, 0, 0, 0, 0, 0,0,0,0};
    public enum GameModeArray { ThrowCan =1,WhackaMole=2,TowerStack =3}
    public GameModeArray GameMode;
    bool getObject = false;
    bool getLevel = false;



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
    }

    //void Update()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex != 1)
    //    {
    //        getObject = false;
    //        getLevel = false;
    //        gmSelect.Clear();
    //    }
    //    if (SceneManager.GetActiveScene().buildIndex == 1 )
    //    {
    //        if (!getObject)
    //        {
    //            var canvas = GameObject.Find("Canvas");
    //            GameObject mgPanel;
    //            for(int k=0; k < canvas.transform.childCount; k++)
    //            {
    //                if (canvas.transform.GetChild(k).name == "MiniGamePanel")
    //                {
    //                    mgPanel = canvas.transform.GetChild(k).gameObject;
    //                    for (int i = 0; i < mgPanel.transform.childCount; i++)
    //                    {
    //                        gmSelect.Add(mgPanel.transform.GetChild(i).GetComponent<Button>());
    //                        getObject = true;
    //                    }
    //                    break;
    //                }
    //            }
    //            //WHACK A MOLE
    //            Button WAM = gmSelect[2].GetComponent<Button>();
    //            WAM.GetComponent<Button>().onClick.AddListener(delegate { TOWAM(); });
    //            //Tower Stack
    //            Button Tower = gmSelect[3].GetComponent<Button>();
    //            Tower.GetComponent<Button>().onClick.AddListener(delegate { ToTower(); });

    //        }                   
    //        //THROW CAN
    //        for(int i =0; i < gmSelect[1].transform.childCount; i++)
    //        {
    //            if (gmSelect[1].gameObject.activeInHierarchy)
    //            {                   
    //                for (int k = 0; k < gmSelect[1].transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).childCount-1; k++)
    //                {
    //                    if (levels.Count < gmSelect[1].transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).childCount-1)
    //                    {
    //                        levels.Add(gmSelect[1].transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(k).gameObject.GetComponent<Button>());
    //                    }
                        
    //                    //Stars = levels[k].transform.GetChild(0).GetComponentsInChildren<Transform>();
    //                    //RemoveElement(ref Stars, 0);
    //                    //if (clearStar[k] == 3)
    //                    //{
    //                    //    for (int j = 0; j < Stars.Length; j++)
    //                    //    {
    //                    //        Stars[j].GetComponent<Image>().sprite = lightStar;
    //                    //    }
    //                    //}
    //                    //else if (clearStar[k] == 2)
    //                    //{
    //                    //    for (int j = 0; j < Stars.Length - 1; j++)
    //                    //    {
    //                    //        Stars[j].GetComponent<Image>().sprite = lightStar;
    //                    //    }
    //                    //}
    //                    //else if (clearStar[k] == 1)
    //                    //{
    //                    //    for (int j = 0; j < Stars.Length - 2; j++)
    //                    //    {
    //                    //        Stars[j].GetComponent<Image>().sprite = lightStar;
    //                    //    }
    //                    //}
    //                    //Array.Clear(Stars, 0, Stars.Length);                        
    //                }
    //                //for (int starCheck = 0; starCheck < clearStar.Length; starCheck++)
    //                //{
    //                //    if (clearStar[starCheck] == 0)
    //                //    {
    //                //        for (int not1st = starCheck + 1; not1st < clearStar.Length; not1st++)
    //                //        {
    //                //            levels[not1st].interactable = false;
    //                //        }
    //                //        break;
    //                //    }
    //                //}
    //                //if (!getLevel)
    //                //{
    //                //    foreach (Button button in levels)
    //                //    {
    //                //        button.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(button.gameObject.name); });
    //                //        getLevel = true;
    //                //    }
    //                //}
    //            }
    //            else
    //            {
    //                levels.Clear();
    //            }
    //        }
    //    }    
    //}


    void ChooseLevel(string selectLevel)
    {
        Debug.Log(selectLevel);
        ChooseGame(1);
        for(int i = 0; i < levels.Count; i++)
        {
            if(selectLevel == levels[i].name)
            {
                level = i;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex != 3)
        {
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        }
    }

    void TOWAM()
    {
        ChooseGame(2);
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);
    }
    void ToTower()
    {
        ChooseGame(3);
        SceneManager.LoadSceneAsync(5, LoadSceneMode.Single);
    }

    public void ChooseGame(int gamemode)
    {
        switch ((GameModeArray)gamemode)
        {
            case (GameModeArray.ThrowCan):
                GameMode = (GameModeArray)gamemode;
                Debug.Log(GameModeArray.ThrowCan);
                break;
            case (GameModeArray.WhackaMole):
                GameMode = (GameModeArray)gamemode;
                Debug.Log(GameModeArray.WhackaMole);
                break;
            case (GameModeArray.TowerStack):
                GameMode = (GameModeArray)gamemode;
                Debug.Log(GameModeArray.TowerStack);
                break;
        }
    }

    private void RemoveElement<T>(ref T[] arr, int index)
    {
        for (int i = index; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        Array.Resize(ref arr, arr.Length - 1);
    }
}
