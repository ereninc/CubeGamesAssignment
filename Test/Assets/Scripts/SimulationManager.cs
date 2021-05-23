using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private GameObject departurePosition;
    [SerializeField] private GameObject arrivalPosition;
    [SerializeField] private GameObject projectile;
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
                    departurePosition = Instantiate(departurePosition, new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z), Quaternion.identity) as GameObject;
                    projectile = Instantiate(projectile, departurePosition.transform.position, Quaternion.identity);
                    projectile.SetActive(false);
                    _instantiateCount += 1;
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
