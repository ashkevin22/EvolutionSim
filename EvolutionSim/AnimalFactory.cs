using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    public enum AnimalType
    {
        Predator,
        Prey,
    }
    public static class AnimalFactory
    {
        public static Animal? CreateAnimal(AnimalType animalType, double newX, double newY, NeuralNet? net = null)
        {
            if(animalType == AnimalType.Predator)
            {
                return new Predator(newX, newY, net);
            }
            else if(animalType == AnimalType.Prey)
            {
                return new Prey(newX, newY, net);
            }
            else
            {
                throw new Exception("Code should never reach this point");
            }
        }
    }
}
