using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    public interface Observer
    {
        /// <summary>
        /// Function to update the observers after an iteration with the amount of prey and predators
        /// </summary>
        /// <param name="numPreds">number of predators</param>
        /// <param name="numPrey">number of prey</param>
        public abstract void Update(int numPreds, int numPrey);
    }
}
