using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaMControl : MonoBehaviour
{
    public WaMManager WC;


    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{

        //}
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    var ray = Camera.main.ScreenPointToRay()
        //}
        if (WC.State == WaMManager.GameState.Start)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
                //Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
                //Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
                //Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);
                //Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green);

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Mole")
                    {
                        hit.transform.gameObject.GetComponent<Mole>().health -= 20;
                        if (hit.transform.gameObject.GetComponent<Mole>().health <= 0)
                        {
                            WaMManager.Score += 10;
                        }
                    }
                }           
            }
        }
        


    }
}
