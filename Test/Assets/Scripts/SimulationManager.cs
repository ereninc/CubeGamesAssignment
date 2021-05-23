using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private ObliqueMotion obliqueMotion;
    [SerializeField] private GameObject departurePosition;
    [SerializeField] private GameObject arrivalPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject pool;
    private int _instantiateCount = 0;
    private List<GameObject> _projectilePool = new List<GameObject>();
    private int _projectileCount = 16;
    private int _counter = 0;

    private void Awake()
    {
        CreateProjectilePool();
    }

    private void Update()
    {
        MouseInput();
        GetActiveProjectiles();
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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_counter < 16)
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (_instantiateCount == 0)
                {
                    if (hit)
                    {
                        departurePosition = Instantiate(departurePosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z), Quaternion.identity) as GameObject;
                        pool.transform.position = departurePosition.transform.position;
                        _instantiateCount += 1;
                    }
                }
                else if(_instantiateCount == 1)
                {
                    if (hit)
                    {
                        arrivalPosition = Instantiate(arrivalPosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z), Quaternion.identity) as GameObject;
                        projectile.SetActive(true);
                        _instantiateCount += 1;
                    }
                }
                else
                {
                    GameObject projectile = _projectilePool[_counter].gameObject;
                    projectile.SetActive(true);
                    _counter++;
                    if (_counter == 16)
                    {
                        ExtendPool();
                    }
                }
            }
            else
            {
                GameObject projectile = _projectilePool[_counter].gameObject;
                projectile.SetActive(true);
                _counter++;
                if (_counter==24)
                {
                    _counter = 0;
                }
            }
        }
    }

    private void ExtendPool()
    {
        for (int i = 0; i < 8; i++)
        {
            projectile = Instantiate(projectile, pool.transform.position, Quaternion.identity);
            projectile.transform.SetParent(pool.transform);
            projectile.transform.position = pool.transform.position;
            projectile.SetActive(false);
            _projectilePool.Add(projectile);
        }
    }

    private void GetActiveProjectiles()
    {
        foreach (var item in _projectilePool)
        {
            if (item.activeSelf)
            {
                Debug.Log(item.name);
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
