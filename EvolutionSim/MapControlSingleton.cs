using System;
using System.Diagnostics;
using System.Xml.XPath;

namespace EvolutionSim
{
	public sealed class MapControlSingleton
	{
		public static MapControlSingleton? _instance = null;
		public Map? Map = null;
		public List<Predator> Predators;
		public List<Prey> Prey;

		// distance in which predators can eat the prey
		private const double _eatThresholdDist = 1;

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
				Predator pred = Predators[i];
				Prey closestPrey = GetClosestPrey(pred.xLoc, pred.yLoc);
				CheckCanEat(pred, closestPrey);
				(Animal? offspring, bool isDead) = Predators[i].Move(Map, closestPrey);
				if(offspring != null)
				{
					Predators.Add((Predator)offspring);
				}
                if (isDead)
                {
                    Predators.Remove(pred);
                }
            }
            // Move all the prey
            for (int i = 0; i < Prey.Count; i++)
            {
				Prey prey = Prey[i];
				Predator? closestPred = GetClosestPredator(prey.xLoc, prey.yLoc);
                (Animal? offspring, bool isDead) = Prey[i].Move(Map, closestPred);
                if (offspring != null)
                {
                    Prey.Add((Prey)offspring);
                }
				if (isDead)
				{
					Prey.Remove(prey);
				}
            }
        }

		/// <summary>
		/// Get the closest prey to the given position
		/// </summary>
		/// <param name="xPos">x position</param>
		/// <param name="yPos">y position</param>
		/// <returns>prey closest to the given position</returns>
		public Prey? GetClosestPrey(double xPos, double yPos)
		{
			Prey? closestPrey = null;
			Prey tempPrey;
			double closestDist = int.MaxValue;
			for(int i = 0; i < Prey.Count; i++)
			{
				tempPrey = Prey[i];
				// using manhattan distance in the hopes that it'll make this run faster
				double tempDist = Math.Abs(tempPrey.xLoc - xPos) + Math.Abs(tempPrey.yLoc - yPos);
                if (tempDist < closestDist)
				{
					closestDist = tempDist;
					closestPrey = tempPrey;
				}
			}
			return closestPrey;
		}

        /// <summary>
        /// Get the closest predator to the given position
        /// </summary>
        /// <param name="xPos">x position</param>
        /// <param name="yPos">y position</param>
        /// <returns>predator closest to the given position</returns>
        public Predator? GetClosestPredator(double xPos, double yPos)
        {
            Predator? closestPredator = null;
            Predator tempPredator;
            double closestDist = int.MaxValue;
            for (int i = 0; i < Predators.Count; i++)
            {
                tempPredator = Predators[i];
                // using manhattan distance in the hopes that it'll make this run faster
                double tempDist = Math.Abs(tempPredator.xLoc - xPos) + Math.Abs(tempPredator.yLoc - yPos);
                if (tempDist < closestDist)
                {
                    closestDist = tempDist;
                    closestPredator = tempPredator;
                }
            }
            return closestPredator;
        }

        /// <summary>
        /// Function to check if there are any prey that can be eaten by a given predator
        /// </summary>
        /// <param name="predator">Predator to check for prey to eat</param>
        /// <returns>Prey that were eaten, null if there wasn't one</returns>
        public Prey? CheckCanEat(Predator predator, Prey prey)
		{
			// using manhattan distance in the hopes that it'll make this run faster
			if(predator == null || prey == null) return null;
			if(Math.Abs(prey.xLoc - predator.xLoc) + Math.Abs(prey.yLoc - predator.yLoc) < _eatThresholdDist)
			{
				predator.Eat();
				Prey.Remove(prey);
				return prey;
			}
			return null;
		}
	}
}

