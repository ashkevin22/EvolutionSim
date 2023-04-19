using System;
namespace EvolutionSim
{
	public abstract class Animal
	{
		public double Energy = 100;
		public NeuralNet NeuralNetwork;
		public double xLoc;
		public double yLoc;

		protected const double _energyLossPerMove = 1;

		/// <summary>
		/// Function to move the Animal
		/// </summary>
		/// <param name="map">Map object to check for valid moves</param>
		/// <returns>returns new position of the Animal</returns>
		public abstract void Move(Map map);

		/// <summary>
		/// Function to reproduce and create a new Animal based on the parent(s)
		/// </summary>
		/// <returns>new offspring Animal</returns>
		public abstract Animal Reproduce();
	}
}

