using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    [SerializeField] private Transform departurePosition;
    [SerializeField] private Transform arrivalPosition;
    
    private const float Gravity = 9.8f;
    private float _targetDistance;
    private float _projectileVelocity;
    private float _vX;
    private float _vY;
    private bool _isMoving = false;
    private float _angle = 45.0f;
    
    public int speed = 0;
    public int hMax = 0;
    
    private UIController uiController;

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
        yield return new WaitForSeconds(1.25f);
        transform.position = departurePosition.position + new Vector3(0, 0, 0);
        _targetDistance = Vector3.Distance(transform.position, arrivalPosition.position); //x
        _projectileVelocity = _targetDistance / (Mathf.Sin(2 * _angle * Mathf.Deg2Rad) / (Gravity * speed)); //Vo
        _vX = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(_angle * Mathf.Deg2Rad); //Vox
        _vY = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(_angle * Mathf.Deg2Rad); //Voy
        float flightDuration = _targetDistance / _vX; //tflight
        transform.rotation = Quaternion.LookRotation(arrivalPosition.position - transform.position);
        float deltaTime = 0;
        while (deltaTime < flightDuration)
        {
            _isMoving = true;
            transform.Translate(0, (_vY - (Gravity * speed) * deltaTime) * Time.deltaTime, _vX * Time.deltaTime);
            deltaTime += Time.deltaTime;
            yield return null;
            _isMoving = false;
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
        if (!_isMoving)
        {
            hMax = uiController.uiHmax;
            speed = uiController.uiSpeed;
        }
    }
}