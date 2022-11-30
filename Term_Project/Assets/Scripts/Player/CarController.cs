using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float m_horizontalInput;                        // horizontal
    private float m_verticalInput;                          // vertical
    private float m_steeringAngle;                          // 조향 각도(자동차가 방향을 바꿀 때 조향 바퀴의 스핀들이 선회 이동하는 각도

    public WheelCollider frontDriverW, frontPassengerW;     // 앞 바퀴 콜라이더
    public WheelCollider rearDriverW, rearPassengerW;       // 뒷 바퀴 콜라이더
    public Transform frontDriverT, frontPassengerT;         // 앞 바퀴
    public Transform rearDriverT, rearPassengerT;           // 뒷 바퀴
    public float maxSteerAngle = 30;                        // 최대 조향각
    public float motorForce = 100;                          // 바퀴를 회전시키는 힘

    float time = 0.0f, cooltime = 0.0f;                     // 거리 계산 시간, 자동차를 내렸다가 다시 타기위한 쿨타임
    Vector3 pastPos, currentPos;                            // 거리 계산을 위한 변수
    public static float absXDir, absZDir;                   // 축 거리
    public static bool coolTimeStart = false;               // 쿨 타임

    Rigidbody rigidbody;
    GameObject player;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void GetInput()
    {
        if (Player.isRiding)
        {
            m_horizontalInput = Input.GetAxis("Horizontal");
            m_verticalInput = Input.GetAxis("Vertical");
        }
    }

    /* 조향 각 계산 */
    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    /* 가속 */
    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    /* 휠 콜라이더, 오브젝트 포지션 업데이트 */
    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    /* 휠 콜라이더의 위치와 바퀴 오브젝트의 위치를 맞춰줌 */
    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        CalDistance();
        GetOutOfTheCar();
    }

    /* 차가 이동한 거리를 계산 */
    void CalDistance()
    {
        if (Player.isRiding)
        {
            time += Time.deltaTime;
            if (pastPos == null) pastPos = transform.position;
            if (((int)time) % 1 == 0)
            {
                if (currentPos == null) currentPos = transform.position;
                else
                {
                    pastPos = currentPos;
                    currentPos = transform.position;
                }
            }
        }

        if (pastPos != null && currentPos != null)
        {
            absXDir = Mathf.Abs(currentPos.x - pastPos.x);
            absZDir = Mathf.Abs(currentPos.z - pastPos.z);
        }
    }

    /* 차에서 하차 + 쿨타임 3초 */
    void GetOutOfTheCar()
    {
        if (player.transform.parent != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.Instance.GetComponent<AudioSource>().volume = 0.8f;
                player.gameObject.transform.parent = null;
                Player.isRiding = false;

                player.SetActive(true);
                coolTimeStart = true;
            }
        }

        if (coolTimeStart)
        {
            cooltime += Time.deltaTime;

            if (cooltime > 3f)
            {
                coolTimeStart = false;
                cooltime = 0.0f;
            }
        }
    }
}

