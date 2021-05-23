using System;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private ObliqueMotion obliqueMotion;
    [SerializeField] private GameObject departurePosition;
    [SerializeField] private GameObject arrivalPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject pool;
    private int _instantiateCount = 0;
    private bool _instantiatable = true;
    private List<GameObject> _projectilePool = new List<GameObject>();
    private int _projectileCount = 16;
    private int _counter = 0;

    private void Awake()
    {
        obliqueMotion = FindObjectOfType<ObliqueMotion>();
        CreateProjectilePool();
    }

    private void Update()
    {
        MouseInput();
    }

    private void CreateProjectilePool()
    {
        pool = Instantiate(pool, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        for (int i = 0; i < _projectileCount; i++)
        {
            projectile = Instantiate(projectile, pool.transform.position, Quaternion.identity);
            projectile.transform.SetParent(pool.transform);
            projectile.transform.position = pool.transform.position;
            _projectilePool.Add(projectile);
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) &&_instantiatable && _counter < 16)
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (_instantiateCount == 0)
            {
                if (hit)
                {
                    departurePosition = Instantiate(departurePosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z), Quaternion.identity) as GameObject;
                    pool.transform.position = departurePosition.transform.position;
                    GameObject projectile = _projectilePool[_counter].gameObject;
                    projectile.SetActive(false);
                    _instantiateCount += 1;
                    _counter++;
                }
            }
            else
            {
                if (hit)
                {
                    arrivalPosition = Instantiate(arrivalPosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z), Quaternion.identity) as GameObject;
                    projectile.SetActive(true);
                    _instantiateCount = 0;
                    _instantiatable = false;
                }
            }
        }
    }

    public Vector3 GetDeparturePosition()
    {
        return departurePosition.transform.position;
    }
    
    public Vector3 GetArrivalPosition()
    {
        return arrivalPosition.transform.position;
    }

    public Vector3 GetProjectilePosition()
    {
        return projectile.transform.position;
    }
}
