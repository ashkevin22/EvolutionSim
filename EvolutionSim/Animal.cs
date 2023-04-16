using System;
namespace EvolutionSim
{
	public abstract class Animal
	{
		public double Energy;
		public NeuralNet NeuralNetwork;
		public double xLoc;
		public double yLoc;

		/// <summary>
		/// Function to move the Animal
		/// </summary>
		/// <returns>returns new position of the Animal</returns>
		public abstract (double, double) Move();

		/// <summary>
		/// Function to reproduce and create a new Animal based on the parent(s)
		/// </summary>
		/// <returns>new offspring Animal</returns>
		public abstract Animal Reproduce();
	}
}

