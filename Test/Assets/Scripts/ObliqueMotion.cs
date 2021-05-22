using System;
using UnityEngine;
using System.Collections;
 
//h = ((Vi)^2* sin^2 * Qi) / 2g

public class ObliqueMotion : MonoBehaviour
{
    public Transform Target;
    public float hMax = 0;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
 
    public Transform _projectile;      
    private Transform _myTransform;

    private float _targetDistance;
    private float _projectileVelocity;
   
    void Awake()
    {
        _myTransform = transform;      
    }
 
    void Start()
    {          
        StartCoroutine(SimulateProjectile());
    }

    private void Update()
    {
        Debug.Log(_projectile.transform.position);
    }

    void CalculateAngle()
    {
        
    }
 
    IEnumerator SimulateProjectile()
    {
        for (;;)
        {
            // Short delay added before Projectile is thrown
            yield return new WaitForSeconds(1.5f);
       
            // Move projectile to the position of throwing object + add some offset if needed.
            _projectile.position = _myTransform.position + new Vector3(0, 0.0f, 0);
       
            // Calculate distance to target
            _targetDistance = Vector3.Distance(_projectile.position, Target.position);
 
            // Calculate the velocity needed to throw the object to the target at specified angle.
            _projectileVelocity = _targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(_projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(_projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
            // Calculate flight time.
            float flightDuration = _targetDistance / Vx;
   
            // Rotate projectile to face the target.
            _projectile.rotation = Quaternion.LookRotation(Target.position - _projectile.position);
       
            float elapse_time = 0;
 
            while (elapse_time < flightDuration)
            {
                _projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
                elapse_time += Time.deltaTime;
 
                yield return null;
            }
        }
    }
}