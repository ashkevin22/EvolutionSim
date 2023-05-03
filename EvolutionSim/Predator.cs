using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace EvolutionSim
{
	public class Predator : Animal
	{
        private const double _maxMoveDist = 0.25;
        private const double _energyLossPerMove = 1.7;

        public Predator(double x, double y, NeuralNet? net = null)
		{
            if(net == null)
            {
                this.NeuralNetwork = new(new List<int> { 4, 4, 2 });
            }
            else
            {
                this.NeuralNetwork = net;
            }
            this.xLoc = x;
            this.yLoc = y;
		}

        public override (Animal? offspring, bool isDead) Move(Map map, Animal? closestAnimal)
        {
            Random rand = new();

            this.EnergyLevel -= (rand.NextDouble() * _energyLossPerMove);
            // once the animal reaches -50 energy, it dies
            if (EnergyLevel <= -50) return (null, true);
            // if the animal is out of energy, it must stand still
            if (EnergyLevel <= 0) return (null, false);
            
            double closestAnimalXloc;
            double closestAnimalYloc;
            if (closestAnimal == null)
            {
                closestAnimalXloc = xLoc;
                closestAnimalYloc = yLoc;
            }
            else
            {
                closestAnimalXloc = closestAnimal.xLoc;
                closestAnimalYloc = closestAnimal.yLoc;
            }

            List<double> inputs = new List<double> { xLoc, yLoc, closestAnimalXloc, closestAnimalYloc };

            Animal? offspring = null;
            // move
            this.NeuralNetwork.FeedForward(inputs);
            List<double> results = this.NeuralNetwork.GetResults();

            // backprop for training the neural net
            double newX;
            double newY;
            double xMove;
            double yMove;
            double bestDist = int.MaxValue;
            double newDist;
            double bestXmove = 0;
            double bestYmove = 0;
            int numIters = 20;
            for (int i = 0; i < numIters; i++)
            {
                do
                {
                    xMove = rand.NextDouble();
                    yMove = rand.NextDouble();
                    newX = xLoc + ((xMove - 0.5) * _maxMoveDist);
                    newY = yLoc + ((yMove - 0.5) * _maxMoveDist);
                } while (!map.CheckValidPos(newX, newY));

                newDist = Math.Sqrt(Math.Pow(newX - closestAnimalXloc, 2) + Math.Pow(newY - closestAnimalYloc, 2));
                if (newDist < bestDist)
                {
                    bestDist = newDist;
                    bestXmove = xMove;
                    bestYmove = yMove;
                }
            }

            List<double> backProp = new List<double> { bestXmove, bestYmove };
            this.NeuralNetwork.BackProp(backProp);

            double tempX = xLoc + ((results[0] - 0.5) * _maxMoveDist);
            double tempY = yLoc + ((results[1] - 0.5) * _maxMoveDist);
            if (map.CheckValidPos(tempX, tempY))
            {
                xLoc = tempX;
                yLoc = tempY;
            }

            // reproduction check
            if (ReproduceLevel >= 100)
            {
                ReproduceLevel = 0;
                offspring = Reproduce(map, this.NeuralNetwork);
            }

            return (offspring, false);
        }

        public override Animal Reproduce(Map map, NeuralNet net)
        {
            //TODO: create a new neural network based on the topography of the current one
            //      but with a chance of mutation
            Random rand = new();
            double childX;
            double childY;
            double xMove;
            double yMove;
            do
            {
                xMove = rand.NextDouble();
                yMove = rand.NextDouble();
                childX = xLoc + ((xMove - 0.5) * _maxMoveDist);
                childY = yLoc + ((yMove - 0.5) * _maxMoveDist);
            } while (!map.CheckValidPos(childX, childY));
            return new Predator(childX, childY, net);
        }

        /// <summary>
        /// Funtion for a predator to eat a prey
        /// Refills energy and increases ReproduceLevel
        /// </summary>
        public void Eat()
        {
            this.EnergyLevel = 100;
            this.ReproduceLevel += 100;
        }
    }
}

