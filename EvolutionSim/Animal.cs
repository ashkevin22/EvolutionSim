using System;
namespace EvolutionSim
{
	public abstract class Animal
	{
		public double EnergyLevel = 100;
		public double ReproduceLevel = 0;
        public NeuralNet NeuralNetwork;
		public double xLoc;
		public double yLoc;


		/// <summary>
		/// Function to move the Animal
		/// </summary>
		/// <param name="map">Map object to check for valid moves</param>
		/// <returns>returns new offspring if reproduced, null otherwise</returns>
		public abstract (Animal? offspring, bool isDead) Move(Map map, Animal? closestAnimal);

		/// <summary>
		/// Function to reproduce and create a new Animal based on the parent(s)
		/// </summary>
		/// <returns>new offspring Animal</returns>
		public abstract Animal Reproduce(Map map, NeuralNet net);
	}
}

