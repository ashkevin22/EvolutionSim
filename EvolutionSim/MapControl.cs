using System;
namespace EvolutionSim
{
	public sealed class MapControlSingleton
	{
		public MapControlSingleton? _instance = null;
		public Map? Map = null;
		public List<Predator> Predators;
		public List<Prey> Prey;

		// distance in which predators can eat the prey
		private const double _eatThresholdDist = 2;

		/// <summary>
		/// Private constructor for the singleton that sets the Predators and Prey Lists to empty lists
		/// </summary>
		private MapControlSingleton()
		{
			Predators = new List<Predator>();
			Prey = new List<Prey>();
        }

		/// <summary>
		/// Function to get the instance of MapControlSingleton
		/// Generates a new instance if there is not already one
		/// </summary>
		/// <returns>Instance of the MapControlSingleton class</returns>
		public MapControlSingleton GetInstance()
		{
			if(_instance == null)
			{
				_instance = new MapControlSingleton();
			}
			return _instance;
		}

		/// <summary>
		/// Function to create a new map from the given parameters
		/// </summary>
		/// <param name="xBound">x limit for the map</param>
		/// <param name="yBound">y limit for the map</param>
		/// <param name="numLakes">number of lakes in the map</param>
		public void CreateMap(double xBound, double yBound, int numLakes)
		{
			Map = new Map(xBound, yBound, numLakes);
		}

		/// <summary>
		/// Function to run a single iteration of the simulation
		/// </summary>
		public void RunIteration()
		{
			//TODO: IDFK this shit is hard
		}

		/// <summary>
		/// Function to check if there are any prey that can be eaten by a given predator
		/// </summary>
		/// <param name="predator">Predator to check for prey to eat</param>
		/// <returns>Prey that were eaten, null if there wasn't one</returns>
		public Prey? CheckCanEat(Predator predator)
		{
			Prey prey;
			for(int i = 0; i < Prey.Count; i++)
			{
				prey = Prey[i];
				// using manhattan distance in the hopes that it'll make this run faster
				if(Math.Abs(prey.xLoc - predator.xLoc) + Math.Abs(prey.yLoc - predator.yLoc) < _eatThresholdDist)
				{
					predator.Eat(prey);
					return prey;
				}
			}
			return null;
		}
	}
}

