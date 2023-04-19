using System;
namespace EvolutionSim
{
	public class Predator : Animal
	{
		public Predator(double x, double y)
		{
            //TODO: Neural network
            this.xLoc = x;
            this.yLoc = y;
		}

        public override void Move(Map map)
        {
            // TODO: neural network stuff 
            Random rand = new();
            this.Energy -= _energyLossPerMove;
            double newX;
            double newY;
            do
            {
                newX = xLoc + (rand.NextDouble() * 6) - 3;
                newY = yLoc + (rand.NextDouble() * 6) - 3;
            } while (!map.CheckValidPos(newX, newY));
            xLoc = newX;
            yLoc = newY;
        }

        public override Animal Reproduce()
        {
            //TODO: create a new neural network based on the topography of the current one
            //      but with a chance of mutation
            throw new NotImplementedException();
        }

        public void Eat(Prey prey)
        {
            this.Energy = 100;
            Reproduce();
            //TODO: figure out eat behavior
        }
    }
}

