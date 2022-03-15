using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mole : MonoBehaviour
{
    public WaMManager waM;

    public float visibleYHeight; // value of Y when Mole is shown
    public float hiddenYHeight; // value of Y when Mole is hidden
    [SerializeField] private Vector3 myNewXYZPosition; // position to move current Mole
    public float Speed; // Speed to show up
    public float hideMoleTimer; // Editor Timer setting for mole hiding
    private float molehideTimer; // Timer for counting mole hiding
    public float health;
    public GameObject healthBar; // HP bar canvas reference
    public GameObject bulletPrefab; // Bullet prefab reference
    public Transform firePoint; // firing position
    [SerializeField] private Slider hpBar; // HP bar slider
    private float shootCD; // Time to indicate to shoot
    [Tooltip("Time interval to shoot again")]
    public float shootTime; // Time interval to shoot again

    bool resetHP = false;
    bool showMole = false;



    void Start()
    {
        waM = GameObject.FindGameObjectWithTag("GameController").GetComponent<WaMManager>();
        hpBar.maxValue = 100f;
        ShowMole();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.LookAt(Camera.main.transform); 
        transform.localPosition = Vector3.Lerp(transform.localPosition, myNewXYZPosition, Time.deltaTime * Speed);
        hpBar.value = health;
        if(waM.State== WaMManager.GameState.Countdown)
        {
            HideMole();
        }
        if(waM.State== WaMManager.GameState.Start)
        {
            if (showMole)
            {
                shootCD -= Time.deltaTime;
                if (shootCD < 0)
                {
                    Shoot();
                    shootCD = shootTime;
                }
                float targetPlaneAngle = vector3AngleOnPlane(Camera.main.transform.position, transform.position, -transform.up, transform.forward);
                Vector3 newRotation = new Vector3(0, targetPlaneAngle, 0);
                transform.Rotate(newRotation, Space.Self);
            }

            if (molehideTimer < 0||health<=0)
            {
                HideMole();
            }
            else
            {
                molehideTimer -= Time.deltaTime;
            }
        }        

        
    }

    public void HideMole()
    {
        myNewXYZPosition = new Vector3(transform.localPosition.x, hiddenYHeight, transform.localPosition.z);
        resetHP = false;
        showMole = false;
        health = 0f;
    }
    public void ShowMole()
    {
        myNewXYZPosition = new Vector3(transform.localPosition.x, visibleYHeight, transform.localPosition.z);
        molehideTimer = hideMoleTimer;
        ResetHP();
        showMole = true;
        shootCD = shootTime;
    }

    private void ResetHP()
    {
        if (!resetHP)
        {
            health = 100f;
            resetHP = true;
            Debug.Log("HP Reset");
        }
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(Camera.main.transform.position);
        }
    }

    float vector3AngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toZeroAngle)
    {
        Vector3 projectedVector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float projectedVectorAngle = Vector3.SignedAngle(projectedVector, toZeroAngle, planeNormal);

        return projectedVectorAngle;
    }
}