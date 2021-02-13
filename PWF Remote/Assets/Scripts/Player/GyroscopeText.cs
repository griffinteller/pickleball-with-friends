using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GyroscopeText : MonoBehaviour
{
    private TMP_Text _text;
    private Quaternion _rawAttitude = Quaternion.identity;
    private Quaternion _zeroAttitude = Quaternion.identity;
    
    public Quaternion Attitude => Quaternion.Inverse(_zeroAttitude) * _rawAttitude;

    public void Start()
    {
        _text = GetComponent<TMP_Text>();
        Input.gyro.enabled = true;
        GetRawAttitude();
        Calibrate();
    }
    
    public void Update()
    {
        GetRawAttitude();
        SetText();
    }

    public void Calibrate()
    {
        _zeroAttitude = _rawAttitude;
    }

    private void GetRawAttitude()
    {
        _rawAttitude = GyroToUnity(Input.gyro.attitude);
    }

    private void SetText()
    {
        _text.text = Attitude.eulerAngles.ToString();
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
