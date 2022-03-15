using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    public ThrowCanManager TCM;

    private bool hit = false;
    private bool addtoList = false;

    // Start is called before the first frame update
    void Start()
    {
        TCM = GameObject.FindGameObjectWithTag("GameController").GetComponent<ThrowCanManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TCM.CdTimer.text == "2")
        {
            if (!addtoList)
            {
                addtoList = true;
                TCM.Cans.Add(gameObject);
            }
        }
        if (TCM.State == ThrowCanManager.GameState.Start)
        {            
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }
        if (!hit)
        {
            if (gameObject.transform.rotation.x >= 0.4 || gameObject.transform.rotation.x <= -0.4 || gameObject.transform.rotation.z >= 0.4 || gameObject.transform.rotation.z <= -0.4)
            {
                //TCM.Score += 1;
                hit = true;
                for (int i = 0; i<TCM.Cans.Count; i++)
                {
                    if (TCM.Cans[i].name == gameObject.name)
                    {
                        TCM.Cans.Remove(TCM.Cans[i]);
                        Destroy(gameObject, 0f);
                    }
                }

            }
        }        
    }
}
