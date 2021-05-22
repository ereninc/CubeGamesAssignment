using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _interface;
    [SerializeField] private GameObject _showButton;

    private void Start()
    {
        ShowUI();
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
}