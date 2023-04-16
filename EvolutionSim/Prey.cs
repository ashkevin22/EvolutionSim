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
            throw new NotImplementedException();
        }

        public override Animal Reproduce()
        {
            throw new NotImplementedException();
        }

        public void Drink()
        {
            //TODO: drink mechanics
        }
    }
}

