using System;
namespace EvolutionSim
{
	public class Predator : Animal
	{
		public Predator()
		{
            //TODO: what the fuck is a neural network
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

        public void Eat(Prey prey)
        {
            this.Energy = 100;
            Reproduce();
            //TODO: figure out eat behavior
        }
    }
}

