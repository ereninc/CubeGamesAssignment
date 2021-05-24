using System;
using UnityEngine;
using System.Collections;

//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    [SerializeField] private Transform departurePosition;
    [SerializeField] private Transform arrivalPosition;
    private const float Gravity = 9.8f;
    private float _distance;
    private float _projectileVelocity;
    private float _vX;
    private float _vY;
    private float _angle = 53.0f;
    private bool _isFinished = false;
    
    private UIController _uiController;
    public int speed = 0;
    public int hMax = 0;
    public bool isMoving = false;

    private void Awake()
    {
        _uiController = GameObject.FindObjectOfType<UIController>();
        FindArrivalTransform();
        FindDepartureTransform();
    }

    private void Start()
    {
        Init();
    }
    
    private void Update()
    {
        GetDatasFromUI();
        ResetPosition();
    }
    
    private void Init()
    {
        GetDatasFromUI();
        //StartCoroutine(SimulateProjectile());
        _isFinished = false;
        isMoving = false;
    }

    IEnumerator SimulateProjectile()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.25f);
            var projectilePos = transform.position;
            projectilePos = departurePosition.position + new Vector3(0, 0, 0); //Xo, Yo, Zo
            transform.position = projectilePos;
            var position = arrivalPosition.position;
            _distance = Vector3.Distance(projectilePos, position); //X1, Y1, Z1
            
            _projectileVelocity = _distance / (Mathf.Sin(2 * _angle * Mathf.Deg2Rad) / (Gravity * speed)); //Vo
            _vX = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(_angle * Mathf.Deg2Rad); //Vox
            _vY = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(_angle * Mathf.Deg2Rad); //Voy

            /*float hm = (_projectileVelocity * _projectileVelocity) * Mathf.Sin(2 * _angle * Mathf.Deg2Rad) / (2 * Gravity);
            Debug.Log(hm);
            /*_projectileVelocity = 20;
            _angle = 40;
            _vX = 20 * Mathf.Cos(40 * Mathf.Deg2Rad);
            Debug.Log(_vX);

            _vY = 20 * Mathf.Sin(40 * Mathf.Deg2Rad);
            Debug.Log(_vY);
            float hm = ((_vY) * (_vY)) / (2 * (Gravity));
            Debug.Log(hm);*/

            float flightDuration = _distance / _vX; //Tflight
            transform.rotation = Quaternion.LookRotation(position - projectilePos);
            float deltaTime = 0;
            while (deltaTime < flightDuration)
            {
                isMoving = true;
                _isFinished = false;
                transform.Translate(0, (_vY - (Gravity * speed) * deltaTime) * Time.deltaTime, _vX * Time.deltaTime);
                deltaTime += Time.deltaTime;
                yield return null;
                isMoving = false;
                _isFinished = true;
            } 
        }
    }

    private void ResetPosition()
    {
        if (_isFinished)
        {
            transform.position = departurePosition.position;
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
            hMax = _uiController.uiHmax;
            _angle = hMax * 4.5f;
            if (_angle >= 70) _angle = 45;
            speed = _uiController.uiSpeed;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SimulateProjectile());
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}