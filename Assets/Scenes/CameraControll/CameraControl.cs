using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera cam;
    public Transform player;
    public Transform target;

    public List<Transform> enemys = new List<Transform>();


    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        controller = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlPlayer();

        if (Input.GetKey(KeyCode.Tab))
        {
            if (target)
            {
                target = null;
            }
            else
                target = GetTarget();
        }
        if (Input.GetMouseButton(1))
        {
            float m_rotationX = Input.GetAxis("Mouse X");
            float m_rotationY = Input.GetAxis("Mouse Y");
            RotateCameraAround(m_rotationX);
            RotateCameraY(m_rotationY);
        }
        if (target == null)
            FollowPlayer();
        else if (target.tag == "Boss")
        {

        }
        else
        {
            FollowCenterPoint();
        }

    }

    void FollowCenterPoint()
    {

    }

    void BossTarget()
    {

    }

    public float CamHeight = 1.5f;
    public float CamHeightMin = 1f;
    [SerializeField]
    public Vector2 CamRotateXRange = new Vector2(10, 35);

    public float CamFar = 1;
    [SerializeField]
    public Vector3 LookAtOffset;

    public float LookPlayerOffsetY;

    void FollowPlayer()
    {
        //cam.transform.position = player.transform.position - player.transform.forward * CamFar + Vector3.up * CamHeight;
        //cam.transform.LookAt(player.transform.position + player.transform.up * LookPlayerOffsetY);
        cam.transform.position = player.transform.position + player.transform.up * LookPlayerOffsetY - cam.transform.forward * CamFar;
    }

    public float rotateSpeed = 10;
    void RotateCameraY(float yValue)
    {
        //cam.transform.rotation = 
        Vector3 preAngle = cam.transform.rotation.eulerAngles;
        cam.transform.Rotate(Vector3.right * yValue * rotateSpeed * Time.deltaTime, Space.World);
        if (cam.transform.rotation.eulerAngles.x < CamRotateXRange.x)
        {
            cam.transform.rotation = Quaternion.Euler(CamRotateXRange.x, preAngle.y, preAngle.z);
        }
        if (cam.transform.rotation.eulerAngles.x > CamRotateXRange.y)
        {
            cam.transform.rotation = Quaternion.Euler(CamRotateXRange.y, preAngle.y, preAngle.z);
        }
    }


    void RotateCameraAround(float xValue)
    {
        cam.transform.Rotate(Vector3.up* xValue * rotateSpeed * Time.deltaTime,Space.World);
        FollowPlayer();
    }


    Vector3 moveDirection = Vector3.zero;
    public float MoveSpeed;
    public float JumpSpeed;
    public float gravity;
    void ControlPlayer()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= MoveSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = JumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    Transform GetTarget()
    {
        float dis = float.MaxValue;
        Transform tempTarget = null;
        foreach (var e in enemys)
        {
            float tempDis = Vector3.SqrMagnitude(player.transform.position - e.transform.position);
            if (tempDis < dis)
            {
                tempTarget = e;
                dis = tempDis;
            }
        }
        return tempTarget;
    }

}
