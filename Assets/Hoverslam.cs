using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hoverslam : MonoBehaviour
{
    public bool started = false;
    private int dnaSize = 3;
    [SerializeField] private Factory factory;
    [Header("Genetic Algorithm")]
    [SerializeField] public float mutationRate = 0.05f;
    [SerializeField] public float mutationVariance = 0.1f;
    [SerializeField] public float timeScale = 1;
    [SerializeField] private float highestPossibleThrust;
    [SerializeField] private float fuelTankCapacity;


    private GeneticAglorithm<float> geneticAglorithm;       // Genetic Aglorithm with each Gene being a float
    private float lowestFuelSpent = 0f;
    private float lowestFinalVelocity = 0f;
    private float bestFitness = 0f;

    // Use this for initialization
    void Start()
    {
        // Create the Random class

        // Create genetic algorithm class
        geneticAglorithm = new GeneticAglorithm<float>(factory.ships.Count, dnaSize, GetRandomGene, FitnessFunction, mutationRate, mutationVariance);
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            return;
        }
        geneticAglorithm.MutationRate = mutationRate;
        geneticAglorithm.MutationVariance = mutationVariance;
        // Update time scale based on Editor value - do this every frame so we capture changes instantly
        Time.timeScale = timeScale;
        bool allLanded = false;
        for (int shipIndex = 0; shipIndex < factory.ships.Count; shipIndex++)
        {
            allLanded = factory.ships[shipIndex].GetComponent<Engine>().landed;
            if (allLanded == false)
            {
                break;
            }
        }
        if (allLanded)
        {
            for (int shipIndex = 0; shipIndex < factory.ships.Count; shipIndex++)
            {
                Engine engine = factory.ships[shipIndex].GetComponent<Engine>();
                for (int geneIndex = 0; geneIndex < 3; geneIndex++)
                {
                    engine.SetGene(geneticAglorithm.Population[shipIndex].Genes[geneIndex], geneIndex);
                }
                Transform shipTransform = factory.ships[shipIndex].transform;
                shipTransform.position = new Vector3 (shipTransform.position.x, factory.startY, shipTransform.position.z);
                engine.Reset();
            }
            geneticAglorithm.NewGeneration();
        }
        //geneticAglorithm.NewGeneration();
        //bestJump = geneticAglorithm.BestFitness;
    }

    private float GetRandomGene(int geneIndex)
    {
        if (geneIndex == 0) // peak thrust gene
        {
            return Random.Range(0f, highestPossibleThrust);
        }
        else if (geneIndex == 1) // judgement gene
        {
            return Random.Range(0f, factory.startY);
        }
        else                // starting fuel gene
        {
            return Random.Range(0f, fuelTankCapacity);
        }
    }

    private float FitnessFunction(int index)
    {
        // Go through each gene in a member of the population and make their fitness equal to their jump strength minus the game ticks
        // they spent in the DeadZone
        float score = 0;
        DNA<float> dna = geneticAglorithm.Population[index];
        float finalVelocity = factory.ships[index].GetComponent<Engine>().finalVelocity;
        score = finalVelocity;
        return score;
    }
}
