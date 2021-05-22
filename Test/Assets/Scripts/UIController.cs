using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _interface;
    [SerializeField] private GameObject _showButton;

    [SerializeField] private SimulationManager _simulationManager;
    [SerializeField] private Text _departurePosText;
    [SerializeField] private Text _arrivalPosText;

    [SerializeField] private Text _hMaxText;
    [SerializeField] private Text _speed;

    public int maxHeight = 10;
    public int speed = 1;

    private void Awake()
    {
        SetMaximumHeight();
        SetSpeed();
    }

    private void Start()
    {
        ShowUI();
    }

    private void Update()
    {
        SetCoordinates();
        SetHeightSpeedTexts();
        SetMaximumHeight();
        SetSpeed();
        Debug.Log("degisken : " + maxHeight + " - textbox : " + _hMaxText.text);
        Debug.Log("degisken : " + speed + " - textbox : " + _speed.text);
    }

    public void ShowUI()
    {
        _interface.SetActive(true);
        _showButton.SetActive(false);
    }

    public void HideUI()
    {
        _interface.SetActive(false);
        _showButton.SetActive(true);
    }

    private void SetHeightSpeedTexts()
    {
        _hMaxText.text = maxHeight.ToString();
        _speed.text = speed.ToString();
    }

    private void SetCoordinates()
    {
        _departurePosText.text = _simulationManager.GetDeparturePosition().ToString();
        _arrivalPosText.text = _simulationManager.GetArrivalPosition().ToString();
    }

    private int SetMaximumHeight()
    {
        int.TryParse(_hMaxText.text, out maxHeight);
        if (maxHeight < 0)
        {
            maxHeight = 10;
        }
        return maxHeight;
    }

    private int SetSpeed()
    {
        int.TryParse(_speed.text, out speed);
        if (speed < 0)
        {
            speed = 1;
        }
        return speed;
    }
}