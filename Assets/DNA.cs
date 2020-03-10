using System;

public class DNA<T> {
	public float[] Genes { get; private set; }
	public float Fitness { get; private set; }

	private Func<int, float> getRandomGene;
	private Func<int, float> fitnessFunction;

	public DNA(int size, Func<int, float> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
	{
		Genes = new float[size];
		this.getRandomGene = getRandomGene;
		this.fitnessFunction = fitnessFunction;

		if(shouldInitGenes)
		{
			for(int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = getRandomGene(i);
            }
        }
	}

	public float CalculateFitness(int index)
	{
		Fitness = fitnessFunction(index);

		return Fitness;
	}

	public DNA<T> Crossover(DNA<T> otherParent)
	{
		DNA<T> child = new DNA<T>(Genes.Length, getRandomGene, fitnessFunction, shouldInitGenes:false);

		for(int i = 0; i < Genes.Length; i++)
		{
			child.Genes[i] = UnityEngine.Random.Range(0f, 1f) < 0.5f ? Genes[i] : otherParent.Genes[i];
		}

		return child;
	}

	public void Mutate(float mutationRate, float mutationVariance)
	{
		for(int i = 0; i < Genes.Length; i++)
		{
			if(UnityEngine.Random.Range(0f, 1f) < mutationRate)
			{
				Genes[i] = UnityEngine.Mathf.Lerp(Genes[i], getRandomGene(i), mutationVariance);
			}
		}
	}
}