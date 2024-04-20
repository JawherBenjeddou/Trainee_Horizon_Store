using System;
using UnityEngine;
using UnityEngine.UI;

public class Trainee_Horizon_FeridSpaceShip_MouseLook : MonoBehaviour
{
    [Header("Look Parameters")]
    [SerializeField, Range(.1f, 10)] private float _lookSpeedX;
    [SerializeField, Range(.1f, 10)] private float _lookSpeedY;
    [SerializeField, Range(1, 180)] private float _upperLookLimit;
    [SerializeField, Range(0f, 1f)] private float _smoothingFactor;
    [SerializeField, Range(1, 180)] private float _lowerLookLimit;


    private Transform _player;
    private float _rotationX;
    
    [SerializeField] private Camera _cam;


    [Header("Gyro Parameters")]
    [SerializeField] private float _gyroWeight = 0.98f;
    [SerializeField] public float accelerometerWeight = 0.02f;
    [SerializeField] private GameObject gyroSlider;
    private Gyroscope gyro;
    private Quaternion rot;
    private bool gyroEnabled;


    private void Start()
    {
        Input.gyro.enabled = true;
        gyroEnabled = EnableGyro();
    }
    public void HandleMouseLook(Vector2 input)
    {
        _rotationX -= input.y * _lookSpeedY;
        _rotationX = Mathf.Clamp(_rotationX, -_upperLookLimit, _lowerLookLimit);
        _cam.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, input.x * _lookSpeedX, 0);
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            _player.transform.rotation = Quaternion.Euler(90, 90, 0);
            rot = new Quaternion(0, 0, 1, 0);
            return true;
        }
        gyroSlider.SetActive(false);
        return false;
    }


    public void HandleGyro()
    {

        //_player.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);

        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
        else
        {
            Vector3 previousEulerAngles = transform.eulerAngles;
            Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;
            Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
            //targetEulerAngles.x = 0.0f;
            //targetEulerAngles.y = 0.0f;
            targetEulerAngles.z = 0.0f;
            transform.eulerAngles = targetEulerAngles;
        }
    }
}

