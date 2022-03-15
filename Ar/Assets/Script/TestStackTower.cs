using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestStackTower : MonoBehaviour
{
    public GameObject CurrentCube;
    public GameObject LastCube;
    public Text Text;
    public int Level;
    public int Score;
    public bool Done;
    [SerializeField] private Slider streakBar;
    [SerializeField] float count;
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] float speed = 0f;
    bool endPoint;
    [SerializeField] int streak;

    private void Start()
    {
        NewBlock();
        streakBar.maxValue = 100;
        streakBar.value = 0;
        count = 5f;
        streakBar.interactable = false;
    }

    private void NewBlock()
    {


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
                Text.gameObject.SetActive(true);
                Text.text = "Your Score " + Level;
                return;
            }
            if (Mathf.RoundToInt(-CurrentCube.transform.localScale.x+ LastCube.transform.localScale.x) <=1f 
                && Mathf.RoundToInt(-CurrentCube.transform.localScale.z+ LastCube.transform.localScale.z) <= 1f)
            {
                streak++;
                Debug.Log("??");
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
        LastCube = CurrentCube;
        CurrentCube = Instantiate(LastCube);
        CurrentCube.name = Level + "";
        CurrentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((Level / 100f) % 1f, 1f, 1f));
        Level++;        
        Camera.main.transform.position = CurrentCube.transform.position + new Vector3(100, 100, 100);
        Camera.main.transform.LookAt(CurrentCube.transform.position);
        speed = 0f;
    }

    private void Update()
    {
        Debug.Log(Mathf.RoundToInt(-1.4954f));
        Debug.Log(Mathf.Abs(0.8354f));
        if (streakBar.value > 0)
        {
            streakBar.value -= Time.deltaTime;
        }
        else
        {
            streakBar.value = 0;
        }

        if(streakBar.value <= streakBar.minValue)
        {
        }

        if (Done)
        {
            return;
        }

        pos1 = (LastCube.transform.position + Vector3.up * 10) + (((Level % 2 == 0) ? Vector3.left : Vector3.back) * -120);
        pos2 = pos1 + ((Level % 2 == 0) ? Vector3.left : Vector3.back) * 240;
        CurrentCube.transform.localPosition = Vector3.Lerp(pos2, pos1, speed);
        if (speed <= 1f&&!endPoint)
        {
            speed += Time.deltaTime * (Mathf.Pow(Level, 0.4f) / 10);

            if (speed >= 1f)
            {
                endPoint = true;
            }
        }
        else if(speed>=0f&&endPoint)
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
