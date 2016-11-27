using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour 
{
    private float Y_ANGLE_MIN = -20.0f;
    public float Y_ANGLE_MAX = 50.0f;
    private float Y_ANGLE_DIF = 70.0f;
    public float Y_ANGLE_DIFC = 70.0f;
    public float Y_ANGLE_MID = 0.0f;

    public Transform lookAt;
    public Transform camTransform;
    public Transform targetingSystemTarget;

    private Camera cam;

    private float distance = 1.0f;
    public float distMin = 0.5f;
    public float distMax = 2.5f;
    private float distChangeFromCollision = 0.0f;
    public float distanceMovement;
    public float height = 2.0f;
    public float currHeight = 0.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private void Start()
    {
        distChangeFromCollision = 0.0f;
        Y_ANGLE_MIN = Y_ANGLE_MAX - Y_ANGLE_DIFC;
        Y_ANGLE_DIF = Y_ANGLE_DIFC;
        Y_ANGLE_MID = (Y_ANGLE_DIFC) / 2 + Y_ANGLE_MIN;
        currHeight = height;
        camTransform = transform;
        cam = Camera.main;

    }
    private void Update()
    {
        float distChange = Input.GetAxis("Mouse ScrollWheel");
        if(distChange != 0)
        {
            distance += distChange;
            distance = Mathf.Clamp(distance, distMin, distMax);
            Y_ANGLE_DIF = 30 + (Y_ANGLE_DIFC * 1.5f * (distance - distMin) / (distMax - distMin));
            currHeight = 0.5f + (height * 1.5f * (distance - distMin) / (distMax - distMin));
            Y_ANGLE_MIN = Y_ANGLE_MID - (Y_ANGLE_DIF/2);
            Y_ANGLE_MAX = Y_ANGLE_MID + (Y_ANGLE_DIF / 2);
        }
        Vector3 relativePos = transform.position - (lookAt.position);
        RaycastHit hit;

        if (Physics.Raycast(lookAt.position, relativePos, out hit, distance))
        {
            if (hit.collider.tag == "Terrain" && distChangeFromCollision == 0.0f)
            {
                Vector3 dir = hit.point - lookAt.position;
                float dist = dir.magnitude;
                distChangeFromCollision = (dist-0.1f) - distance;
                //distance = distance - hit.distance;
            }
        }
        else
        {
            distChangeFromCollision = 0.0f;
        }
        currentX += Input.GetAxis("Mouse X");
        currentY += -Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }
    private void LateUpdate()
    {
        if(distChangeFromCollision != 0)
        {
            distChangeFromCollision += distanceMovement * Time.deltaTime;
            if (distChangeFromCollision > 0)
            {
                distChangeFromCollision = 0;
                distance += distChangeFromCollision - distanceMovement * Time.deltaTime;
            }
            else
            {
                distance -= distanceMovement * Time.deltaTime;
                currentY -= distanceMovement*5 * Time.deltaTime;
            }
        }
        Vector3 dir = new Vector3(0, currHeight, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        if(lookAt)
        {
            camTransform.position = (lookAt.position + rotation * dir);
            if (targetingSystemTarget != null)
            {
                Vector3 targetDir = targetingSystemTarget.position - lookAt.position;
                cam.transform.LookAt(lookAt.position + targetDir * 0.5f);
            }
            else
            {
                camTransform.LookAt(lookAt.position);
            }
        }

    }
    public void SetTarget(GameObject target)
    {
        if(target)
        {
            targetingSystemTarget = target.transform;
        }
        else
        {
            targetingSystemTarget = null;
        }
    }
}
