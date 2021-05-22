﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private GameObject _departurePosition;
    [SerializeField] private GameObject _arrivalPosition;
    [SerializeField] private GameObject _projectile;
    private int _instantiateCount = 0;
    private bool _instantiatable = true;

    private void Update()
    {
        MouseInput();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) &&_instantiatable)
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (_instantiateCount == 0)
            {
                if (hit)
                {
                    _departurePosition = Instantiate(_departurePosition, hitInfo.point, Quaternion.identity) as GameObject;
                    _projectile = Instantiate(_projectile, new Vector3(_departurePosition.transform.position.x, _departurePosition.transform.position.y + 0.25f, _departurePosition.transform.position.z), Quaternion.identity);
                    _projectile.SetActive(false);
                    _instantiateCount += 1;
                }
            }
            else
            {
                if (hit)
                {
                    _arrivalPosition = Instantiate(_arrivalPosition, hitInfo.point, Quaternion.identity) as GameObject;
                    _projectile.SetActive(true);
                    _instantiateCount = 0;
                    _instantiatable = false;
                }
            }
        }
    }

    public Vector3 GetDeparturePosition()
    {
        return _departurePosition.transform.position;
    }
    
    public Vector3 GetArrivalPosition()
    {
        return _arrivalPosition.transform.position;
    }
}
