using System.Collections;
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
                    _departurePosition = Instantiate(_departurePosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.125f, hitInfo.point.z), Quaternion.identity) as GameObject;
                    _projectile = Instantiate(_projectile, _departurePosition.transform.position, Quaternion.identity);
                    _projectile.SetActive(false);
                    _instantiateCount += 1;
                }
            }
            else
            {
                if (hit)
                {
                    _arrivalPosition = Instantiate(_arrivalPosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.125f, hitInfo.point.z), Quaternion.identity) as GameObject;
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

    public Vector3 GetProjectilePosition()
    {
        return _projectile.transform.position;
    }
}
