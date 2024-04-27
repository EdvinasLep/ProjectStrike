using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraHolder;

    public List<Transform> players = new List<Transform>();

    Transform p1;
    Transform p2;

    public Vector3 middlePoint;

    public float orthoMin = 2;
    public float orthoMax = 2;

    float targetZ;

    public float zMin = -1.4f;
    public float zMax = 5;

    public float yOffset = 0.3f;
    public float xOffset = 0.3f;
    public float zOffset;

    public float ambientMoveAmount = 0.5f;  
    public float ambientMoveSpeed = 0.5f;   

    private float ambientPhase = 0.0f;

    Camera cam;



    private void Start()
    {
        cam = Camera.main;
        cameraHolder = cam.transform.parent;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(players[0].position, players[1].position);
        float half = (distance / 2);

        middlePoint = (players[1].position - players[0].position).normalized * half;
        middlePoint += players[0].position;
        middlePoint.y += yOffset;
        middlePoint.x += xOffset;
        middlePoint.z += zOffset;

        targetZ = -(2 * (half / 2));

        if (Mathf.Abs(targetZ) < Mathf.Abs(zMin))
        {
            targetZ = zMin;
        }

        if (Mathf.Abs(targetZ) > Mathf.Abs(zMax))
        {
            targetZ = zMax;
        }

        cam.transform.localPosition = new Vector3(0, 0.5f, targetZ);

        float ambientX = Mathf.Sin(ambientPhase) * ambientMoveAmount;
        float ambientY = Mathf.Cos(ambientPhase) * ambientMoveAmount;
        ambientPhase += ambientMoveSpeed * Time.deltaTime;

        Vector3 ambientPosition = new Vector3(middlePoint.x + ambientX, middlePoint.y + ambientY, middlePoint.z);
        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, ambientPosition, Time.deltaTime * 5);
    }

    public static CameraManager instance;

    public static CameraManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}