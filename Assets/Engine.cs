using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public bool landed = false;
    public float fuelBurned;
    public float finalVelocity;
    public float stopHeight;
    private float shipWeight;
    Renderer boy;
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
    private Vector3 up;
    private float GetFuelWeight(float newtons)
    {
        return newtons / 9.806f;
    }
    public void Reset()
    {
        fuelBurned = 0f;
        lastHeight = heightTransform.position.y + 1f;
        currentThrust = 0f;
        stopBurn = false;
        landed = false;
        rigidBody.velocity = new Vector3(0f, 0f, 0f);
        stopHeight = 0.0f;
    }
    public void SetGene(float geneValue, int geneIndex)
    {
        if (geneIndex == 0) // peak thrust gene
        {
            peakThrust = geneValue;
        }
        else if (geneIndex == 1) // judgement gene
        {
            burnStartHeight = geneValue;
        }
        else                // starting fuel gene
        {
            startingFuel = geneValue;
        }
    }
    private void Fail()
    {
        fuelBurned = 1000f;
        finalVelocity = 1000f;
        landed = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        shipWeight = rigidBody.mass;
        fuelBurned = 0f;
        lastHeight = heightTransform.position.y + 1f;
        currentThrust = 0f;
        stopBurn = false;
        stopHeight = 0.0f;
        up = transform.up;
        boy = GetComponent<MeshRenderer>();
    }
    void FixedUpdate()
    {
        currentVelocity = rigidBody.velocity.magnitude;
        rigidBody.mass = Mathf.Min(GetFuelWeight(shipWeight + startingFuel - fuelBurned), shipWeight);

        boy.material.color = Color.Lerp(Color.white, Color.blue, currentThrust / Mathf.Max(peakThrust, 0.00001f));
        if (stopBurn)
        {
            boy.material.color = Color.red;
        }
        if (!stopBurn && (lastHeight < heightTransform.position.y || fuelBurned > startingFuel))
        {
            stopBurn = true;
            stopHeight = heightTransform.position.y;
            //Debug.Log("Ship stopped at height: " + heightTransform.position.y.ToString());
        }
        float deltaTime = Time.deltaTime;
        if (heightTransform.position.y <= burnStartHeight && !stopBurn)
        {
            currentThrust = peakThrust;
            rigidBody.AddForce(up * currentThrust);
            fuelBurned += currentThrust * deltaTime;
            fuelBurned = Mathf.Min(fuelBurned, startingFuel);
        }
        lastHeight = heightTransform.position.y;
        if (lastHeight > 1000f)
        {
            Fail();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnCollisionEnter(Collision x)
    {
        if (currentVelocity > 0f)
        {
            finalVelocity = currentVelocity;
        }
        stopBurn = true;
        if (stopHeight != 0.0f)
        {
            stopHeight = 10.0f;
        }
        landed = true;
    }
}
