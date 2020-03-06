using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private Transform height; // how much force is added to currentEngineForce per second
    [SerializeField] private float engineRevUp; // how much force is added to currentEngineForce per second
    [SerializeField] private float maxEngineForce;
    private float currentEngineForce;
    private Rigidbody rigidBody;
    private Vector3 up;
    private float currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        currentEngineForce = 0f;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        up = transform.up;
    }
    void FixedUpdate()
    {
        currentVelocity = rigidBody.velocity.magnitude;
    }
    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (currentEngineForce < maxEngineForce)
        {
            currentEngineForce += deltaTime * engineRevUp;
        }
        currentEngineForce = Mathf.Min(currentEngineForce, maxEngineForce);
        rigidBody.AddForce(up * currentEngineForce * deltaTime);
    }
    void OnCollisionEnter(Collision x)
    {
        Debug.Log(currentVelocity);
        maxEngineForce = 0f;
    }
}
