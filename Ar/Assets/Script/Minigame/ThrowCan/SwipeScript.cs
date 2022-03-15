using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour
{
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    public float throwForceInXandY = 1f;
    public float throwForceInZ = 10f;

    Rigidbody rb;
    ThrowCanManager TCM;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        TCM = GameObject.FindGameObjectWithTag("GameController").GetComponent<ThrowCanManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;

            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;


            if (timeInterval < 0.1f && (direction.x < 100f || direction.x > -100f) && direction.y > -100f)
            {
                return;
            }
            else
            {
                rb.isKinematic = false;
                rb.AddRelativeForce((-direction.x * throwForceInXandY) / 5, (-direction.y * throwForceInXandY) / 5, (throwForceInZ / timeInterval) * TCM.DistanceForce);
                StartCoroutine(SpawnBall());
            }
        }
    }

    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject, 3f);
        TCM.BallSwipe = null;
    }
}
