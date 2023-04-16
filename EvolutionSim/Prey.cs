using System;
namespace EvolutionSim
{
	public class Prey : Animal
	{
		public Prey()
		{
            //TODO: neural net again
		}

        public override (double, double) Move()
        {
            this.Energy -= _energyLossPerMove;
            throw new NotImplementedException();
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

