using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInstance : MonoBehaviour
{
    public static GlobalInstance Instance;

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
}
