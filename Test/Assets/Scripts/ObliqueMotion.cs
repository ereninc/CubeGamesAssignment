using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    [SerializeField] private Transform departurePosition;
    [SerializeField] private Transform arrivalPosition;
    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;
    
    private float _targetDistance;
    private float _projectileVelocity;
    private float _vX;
    private float _vY;
    
    public int speed = 0;
    public int hMax = 0;
    [SerializeField] private UIController uiController;

    private void Awake()
    {
        uiController = GameObject.FindObjectOfType<UIController>();
        FindArrivalTransform();
        FindDepartureTransform();
    }

    private void Update()
    {
        MouseInput();
        GetDatasFromUI();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(SimulateProjectile());
        }
    }

    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(1.25f);

        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = departurePosition.position + new Vector3(0, 0, 0);

        // Calculate distance to target
        _targetDistance = Vector3.Distance(transform.position, arrivalPosition.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        _projectileVelocity = _targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / (gravity * speed));

        // Extract the X  Y component of the velocity
        _vX = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad); 
        _vY = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = _targetDistance / _vX;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(new Vector3(arrivalPosition.position.x, arrivalPosition.position.y, arrivalPosition.position.z) - transform.position);

        float elapseTime = 0;

        while (elapseTime < flightDuration)
        {
            transform.Translate(0, (_vY - (gravity * speed) * elapseTime) * Time.deltaTime, _vX * Time.deltaTime);

            elapseTime += Time.deltaTime;

            yield return null;
        }
    }

    private Transform FindArrivalTransform()
    {
        arrivalPosition = GameObject.FindWithTag("Arrival").transform;
        return arrivalPosition;
    }

    private Transform FindDepartureTransform()
    {
        departurePosition = GameObject.FindWithTag("Departure").transform;
        return departurePosition;
    }

    private void GetDatasFromUI()
    {
        hMax = uiController.uiHmax;
        speed = uiController.uiSpeed;
    }
}