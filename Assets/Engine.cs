using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    float shipWeight;
    // genes
    [SerializeField] private float peakThrust;              // force in mega-newtons the engine can apply
    [SerializeField] private float burnStartHeight;         // the height the burn should start
    [SerializeField] private float startingFuel;            // the starting fuel in kilonewtons

    // derrived variables
    [SerializeField] private Rigidbody rigidBody;           // rigid body of the ship
    [SerializeField] private Transform heightTransform;     // bottom of ship, used to get distance from ground
    [SerializeField] private float engineRevUpTime;         // seconds to reach peakThrust

    // 
    private float currentThrust;

    // logic variables
    private float currentVelocity;
    private bool stopBurn;
    private float lastHeight;
    private float fuelBurned;
    private Vector3 up;
    private float GetFuelWeight(float newtons)
    {
        return newtons / 9.806f;
    }
    // Start is called before the first frame update
    void Start()
    {
        shipWeight = rigidBody.mass;
        fuelBurned = 0f;
        lastHeight = heightTransform.position.y + 1f;
        currentThrust = 0f;
        stopBurn = false;
        up = transform.up;
    }
    void FixedUpdate()
    {
        currentVelocity = rigidBody.velocity.magnitude;
        rigidBody.mass = Mathf.Min(GetFuelWeight(shipWeight + startingFuel - fuelBurned), shipWeight);
    }
    // Update is called once per frame
    void Update()
    {
        if (!stopBurn && (lastHeight < heightTransform.position.y || fuelBurned > startingFuel))
        {
            stopBurn = true;
            Debug.Log("Ship stopped at height: " + heightTransform.position.y.ToString());
        }
        float deltaTime = Time.deltaTime;
        if (heightTransform.position.y <= burnStartHeight && !stopBurn)
        {
            if (currentThrust < peakThrust)
            {
                currentThrust += deltaTime * (peakThrust / engineRevUpTime);
            }
            currentThrust = Mathf.Min(currentThrust, peakThrust);
            rigidBody.AddForce(up * currentThrust);
            fuelBurned += currentThrust * deltaTime;
        }
        lastHeight = heightTransform.position.y;
    }
    void OnCollisionEnter(Collision x)
    {
        if (currentVelocity > 0f)
        {
            Debug.Log("Hit ground at: " + currentVelocity.ToString() + " m/s");
            Debug.Log("Total expended fuel was: " + fuelBurned.ToString());
        }
        stopBurn = true;
    }
}
