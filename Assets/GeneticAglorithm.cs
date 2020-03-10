using System;
using System.Collections.Generic;

public class GeneticAglorithm<T> {

	public List<DNA<T>> Population { get; private set; }
	public int Generation { get; private set; }
	public float BestFitness { get; private set; }
	public T[] BestGenes { get; private set; }


    public float MutationRate;
    public float MutationVariance;

    private Random random;
	private float fitnessSum;

	public GeneticAglorithm(int populationSize, int dnaSize, Func<int, float> getRandomGene, Func<int, float> fitnessFunction, float mutationRate, float mutationVariance = 0.5f, bool shouldInitGenes = true)
	{
		Generation = 1;
        MutationRate = mutationRate;
        MutationRate = mutationVariance;
        Population = new List<DNA<T>>();

		BestGenes = new T[dnaSize];

		for(int i = 0; i < populationSize; i++)
		{
			Population.Add(new DNA<T>(dnaSize, getRandomGene, fitnessFunction, shouldInitGenes: true));
		}
	}

	public void NewGeneration()
    {
        if (Population.Count <= 0) {
			return;
		}

		CalculateFitness();

		List<DNA<T>> newPopulation = new List<DNA<T>>();
        Population.Sort
        (
            (DNA<T> a, DNA<T> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            }
        );
        if (Generation != 1)
        {
            string printString;
            printString = ("Worst:  Fitness=" + ((float)((int)(Population[0].Fitness * 100)) / 100).ToString());
            printString += ("    Peak Thrust=" + ((float)((int)(Population[0].Genes[0] * 100)) / 100).ToString());
            printString += ("   Judgement Height=" + ((float)((int)(Population[0].Genes[1] * 100)) / 100));
            printString += ("  Starting Fuel=" + ((float)((int)(Population[0].Genes[2] * 100)) / 100));
            UnityEngine.Debug.Log(printString);

            float medianFitness = (Population[(Population.Count / 2) - 1].Fitness + Population[(Population.Count / 2) - 0].Fitness) / 2;
            float medianGene0 = (Population[(Population.Count / 2) - 1].Genes[0] + Population[(Population.Count / 2) - 0].Genes[0]) / 2;
            float medianGene1 = (Population[(Population.Count / 2) - 1].Genes[1] + Population[(Population.Count / 2) - 0].Genes[1]) / 2;
            float medianGene2 = (Population[(Population.Count / 2) - 1].Genes[2] + Population[(Population.Count / 2) - 0].Genes[2]) / 2;
            printString = ("Median: Fitness=" + ((float)((int)(medianFitness * 100)) / 100).ToString());
            printString += ("    Peak Thrust=" + ((float)((int)(medianGene0 * 100)) / 100).ToString());
            printString += ("   Judgement Height=" + ((float)((int)(medianGene1 * 100)) / 100));
            printString += ("  Starting Fuel=" + ((float)((int)(medianGene2 * 100)) / 100));
            UnityEngine.Debug.Log(printString);

            printString = ("Second: Fitness=" + ((float)((int)(Population[Population.Count - 2].Fitness * 100)) / 100).ToString());
            printString += ("    Peak Thrust=" + ((float)((int)(Population[Population.Count - 2].Genes[0] * 100)) / 100).ToString());
            printString += ("   Judgement Height=" + ((float)((int)(Population[Population.Count - 2].Genes[1] * 100)) / 100));
            printString += ("  Starting Fuel=" + ((float)((int)(Population[Population.Count - 2].Genes[2] * 100)) / 100));
            UnityEngine.Debug.Log(printString);

            printString = ("First:  Fitness=" + ((float)((int)(Population[Population.Count - 1].Fitness * 100)) / 100).ToString());
            printString += ("    Peak Thrust=" + ((float)((int)(Population[Population.Count - 1].Genes[0] * 100)) / 100).ToString());
            printString += ("   Judgement Height=" + ((float)((int)(Population[Population.Count - 1].Genes[1] * 100)) / 100));
            printString += ("  Starting Fuel=" + ((float)((int)(Population[Population.Count - 1].Genes[2] * 100)) / 100));
            UnityEngine.Debug.Log(printString);
        }
        for (int i = 0; i < Population.Count; i++)
		{
            //string printString;
            //printString = ("Fitness=" + ((float)((int)(Population[i].Fitness * 100)) / 100).ToString());
            //printString += ("    Peak Thrust=" + ((float)((int)(Population[i].Genes[0] * 100)) / 100).ToString());
            //printString += ("   Judgement Height=" + ((float)((int)(Population[i].Genes[1] * 100)) / 100));
            //printString += ("  Starting Fuel=" + ((float)((int)(Population[i].Genes[2] * 100)) / 100));
            //UnityEngine.Debug.Log(printString);
            //DNA<T> parent1 = ChooseParent();
            //DNA<T> parent2 = ChooseParent();
            DNA<T> parent1 = Population[Population.Count-1];
            DNA<T> parent2 = Population[Population.Count-2];


            DNA<T> child = parent1.Crossover(parent2);

            child.Mutate(MutationRate, MutationVariance);

			newPopulation.Add(child);
		}

		Population = newPopulation;

		Generation++;
	}

	public void CalculateFitness()
	{
		fitnessSum = 0;

		DNA<T> best = Population[0];

		for(int i = 0; i < Population.Count; i++)
		{
			fitnessSum += Population[i].CalculateFitness(i);

			if(Population[i].Fitness < best.Fitness)
			{
				best = Population[i];
			}
		}

		BestFitness = best.Fitness;
		best.Genes.CopyTo(BestGenes, 0);
	}

	private DNA<T> ChooseParent()
	{
        return Population[1];
        //double randomNumber = random.NextDouble() * fitnessSum;

        //for(int i = 0; i < Population.Count; i++)
        //{
        //	if( randomNumber < Population[i].Fitness)
        //	{
        //		return Population[i];
        //	}

        //	randomNumber -= Population[i].Fitness;
        //}

        //return Population[random.Next(0, Population.Count)];
    }
}
