using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 target;

    public float Speed;

    // Start is called before the first frame update
    public void Seek(Vector3 _target)
    {
        target = _target;
    }

    private void Start()
    {
        Seek(Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(target.x - transform.position.x,target.y-transform.position.y,target.z -transform.position.z);
        float distanceThisFrame = Speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            Destroy(gameObject, 0f);
            WaMManager.Score += 5;
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.Health -= 10f;
            Debug.Log("hit");
        }
    }
}
