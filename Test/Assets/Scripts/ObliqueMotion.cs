using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    [SerializeField] private Transform departurePosition;
    [SerializeField] private Transform arrivalPosition;
    [SerializeField] private float hMax = 10;
    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;
    
    private float _targetDistance;
    private float _projectileVelocity;
    private float _vX;
    private float _vY;

    [SerializeField] private UIController _uiController;
    private int speed;
    private int maxH;

    private void Awake()
    {
        FindArrivalTransform();
        FindDepartureTransform();
    }

    private void Start()
    {
    }

    private void Update()
    {
        MouseInput();
        speed = _uiController.speed;
        maxH = _uiController.maxHeight;
        Debug.Log(speed);
        Debug.Log(maxH);
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
        yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = departurePosition.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        _targetDistance = Vector3.Distance(transform.position, arrivalPosition.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        _projectileVelocity = _targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y component of the velocity
        _vX = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad); 
        _vY = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = _targetDistance / _vX;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(new Vector3(arrivalPosition.position.x, arrivalPosition.position.y + 0.25f,arrivalPosition.position.z) - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (_vY - gravity * elapse_time) * Time.deltaTime, _vX * Time.deltaTime);
   
            elapse_time += Time.deltaTime;

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
}