using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shakespeare : MonoBehaviour {

	[Header("Genetic Algorithm")]
	[SerializeField] string targetString = "To be, or not to be, that is the question.";
	[SerializeField] string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$£%^&*()=+?@ 1234567890";
	[SerializeField] int populationSize = 200;
	[SerializeField] float mutationRate = 0.01f;


	[Header("Text Components")]
	[SerializeField] Text targetText;
	[SerializeField] Text bestText;
	[SerializeField] Text bestFitnessText;
	[SerializeField] Text generationText;
	[SerializeField] Text scrollDNA;

	[Header("Button Text")]
	[SerializeField] Text buttonText;

	private GeneticAglorithm<char> ga;		// Create the Genetic Algorithm class, making each DNA sequence a character
	private System.Random random;			// Create a random generator
	private bool running = false;

	void Start () {
		// Intialise the random generator and genetic algorithm class
		random = new System.Random();
		ga = new GeneticAglorithm<char>(populationSize, targetString.Length, random, GetRandomGene, FitnessFunction, mutationRate:mutationRate);
	}

	void Update () {

		if(running)
		{
			// Each frame create a new generation
			ga.NewGeneration();

			updateText();

			// When we find the perfect solution (e.g. match the string with the best member of the population)
			if(ga.BestFitness == 1)
			{
				// Remove button through nasty evil code here
				GameObject button = GameObject.Find("StartButton");
				if(button)
				{
					button.SetActive(false);
				}

				this.enabled = false;
			}
		}
	}

	/*
	 * This function is created here and passed into the Genetic Algorithm
	 * We use this to create random genes for eah member of the popluation. Used when initially creating the population and for mutating a gene
	 */
	private char GetRandomGene()
	{
		int i = random.Next(validCharacters.Length);
		return validCharacters[i];
	}

	/*
	 * This function is created here and passed into the Genetic Algorithm
	 * Used to determine how fit a particular member of the population is. (In this case, compare the population member against the target string)
	 */
	private float FitnessFunction(int index)
	{
		float score = 0;

		// Get the specific member of the population (determined by the index)
		DNA<char> dna = ga.Population[index];

		// Go through each Gene in the population
		for(int i = 0; i < dna.Genes.Length; i++)
		{
			// For each gene that matches the corresponding charatcer in the target string, add 1 to the score
			if(dna.Genes[i] == targetString[i])
			{
				score += 1;
			}
		}

		// Normalise the score so it's between 0 and 1 (0 being 0% correct and 1 being 100% correct)
		score /= targetString.Length;

		return score;
	}

	private void updateText()
	{
		// Update the text boxes as the GA runs 
		if(targetText)
		{
			targetText.text = targetString;
		}

		if(bestFitnessText)
		{
			bestFitnessText.text = ga.BestFitness.ToString();
		}

		if(bestText)
		{
			string input = "";
			for(int i = 0; i < ga.BestGenes.Length; i++)
			{
				input += ga.BestGenes[i];
			}
			bestText.text = input;

		}

		if(generationText)
		{
			generationText.text = ga.Generation.ToString();
		}

		if(scrollDNA)
		{
			string input = "";
			int columns = 0;
			
			foreach(DNA<char> dna in ga.Population)
			{
				for(int i = 0; i < dna.Genes.Length; i++)
				{
					input += dna.Genes[i];
				}

				if(columns < 5)
				{
					input += "\t";
					columns++;
				}
				else
				{
					input += "\n";
					columns = 0;
				}
			}

			scrollDNA.text = input;
		}
	}

	public void onButtonClick()
	{
		// This allows us to start and pause the GA
		if(running)
		{
			running = false;
			if(buttonText)
			{
				buttonText.text = "Start";
			}
		}
		else
		{
			running = true;
			if(buttonText)
			{
				buttonText.text = "Pause";
			}
		}
	}
}
