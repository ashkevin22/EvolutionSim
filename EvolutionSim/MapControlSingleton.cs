using System;
namespace EvolutionSim
{
	public sealed class MapControlSingleton
	{
		public static MapControlSingleton? _instance = null;
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
		public static MapControlSingleton GetInstance()
		{

			if(_instance == null)
			{
				_instance = new MapControlSingleton();
			}
			return _instance;
		}

		/// <summary>
		/// Function to create the simulation
		/// </summary>
		/// <param name="map">map object cretaed by MainWindow</param>
		/// <param name="numPreds">initial number of predators</param>
		/// <param name="numPrey">initial number of prey</param>
		public void CreateSimulation(Map map, int numPreds, int numPrey)
		{
			Random rand = new();
			double xPos = -1;
			double yPos = -1;
			this.Map = map;
			// TODO: generate predators and prey in packs maybe?
			// Generate all of the predators
			for(int i = 0; i < numPreds; i++)
			{
				do
				{
					xPos = rand.NextDouble() * Map.xBound;
					yPos = rand.NextDouble() * Map.yBound;
				} while (!Map.CheckValidPos(xPos, yPos));
				Predators.Add(new Predator(xPos, yPos));
			}
			// Generate all of the prey
			for(int i = 0; i < numPrey; i++)
			{
				do
				{
					xPos = rand.NextDouble() * Map.xBound;
					yPos = rand.NextDouble() * Map.yBound;
				} while (!Map.CheckValidPos(xPos, yPos));
				Prey.Add(new Prey(xPos, yPos));
			}
		}

		/// <summary>
		/// Function to run a single iteration of the simulation
		/// </summary>
		public void RunIteration()
		{
			Random rand = new();
			// Move all the predators
			for(int i = 0; i < Predators.Count; i++)
			{
				Predators[i].Move(Map);				
            }
            // Move all the prey
            for (int i = 0; i < Prey.Count; i++)
            {
                Prey[i].Move(Map);
            }
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

