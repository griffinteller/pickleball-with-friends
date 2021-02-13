using TMPro;
using UnityEngine;
using Gyroscope = Player.Gyroscope;

public class GyroscopeText : MonoBehaviour
{
    public Gyroscope gyroscope;
    
    private TMP_Text _text;

    public void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    public void Update()
    {
        SetText();
    }

    private void SetText()
    {
        _text.text = gyroscope.Attitude.eulerAngles.ToString();
    }
}
