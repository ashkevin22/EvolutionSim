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
            throw new NotImplementedException();
        }

        public override Animal Reproduce()
        {
            throw new NotImplementedException();
        }

        public void Eat(Prey prey)
        {
            //TODO: figure out eat behavior
        }
    }
}

