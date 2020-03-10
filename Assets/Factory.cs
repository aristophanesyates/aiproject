using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public int shipNumber;
    public List<GameObject> ships;
    [SerializeField] private GameObject shipPrefab;
    public float startX;
    public float startY;
    public float startZ;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < shipNumber; i++)
        {
            ships.Add(GameObject.Instantiate(shipPrefab, new Vector3(startX + 3.3f * i, startY, startZ), new Quaternion()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
