using System;
namespace EvolutionSim
{
	public class Prey : Animal
	{
		public Prey(double x, double y)
		{
            //TODO: neural net again
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

        public void Drink()
        {
            this.Energy = 100;
            //TODO: drink mechanics
        }
    }
}

