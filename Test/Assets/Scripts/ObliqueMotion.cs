using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    [SerializeField] private Transform departurePosition;
    [SerializeField] private Transform arrivalPosition;
    
    public int speed = 0;
    public int hMax = 0;
    
    private const float Gravity = 9.8f;
    private float _distance;
    private float _projectileVelocity;
    private float _vX;
    private float _vY;
    public bool isMoving = false;
    public float _angle = 53.0f;
    
    private UIController uiController;

    private void Awake()
    {
        uiController = GameObject.FindObjectOfType<UIController>();
        FindArrivalTransform();
        FindDepartureTransform();
    }

    private void Start()
    {
        GetDatasFromUI();
        StartCoroutine(SimulateProjectile());
    }

    private void Update()
    {
        //MouseInput();
        GetDatasFromUI();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !isMoving)
        {
            StartCoroutine(SimulateProjectile());
        }
    }

    IEnumerator SimulateProjectile()
    {
        transform.position = departurePosition.position + new Vector3(0, 0, 0); //Xo, Yo, Zo
        _distance = Vector3.Distance(transform.position, arrivalPosition.position); //X1, Y1, Z1
        _projectileVelocity = _distance / (Mathf.Sin(2 * _angle * Mathf.Deg2Rad) / (Gravity * speed)); //Vo
        _vX = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(_angle * Mathf.Deg2Rad); //Vox
        _vY = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(_angle * Mathf.Deg2Rad); //Voy
        float flightDuration = _distance / _vX; //Tflight
        transform.rotation = Quaternion.LookRotation(arrivalPosition.position - transform.position);
        float deltaTime = 0;
        while (deltaTime < flightDuration)
        {
            isMoving = true;
            transform.Translate(0, (_vY - (Gravity * speed) * deltaTime) * Time.deltaTime, _vX * Time.deltaTime);
            deltaTime += Time.deltaTime;
            yield return null;
            isMoving = false;
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
        if (!isMoving)
        {
            hMax = uiController.uiHmax;
            speed = uiController.uiSpeed;
        }
    }
}