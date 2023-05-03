using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    public class Logger : Observer
    {
        StreamWriter _writer = new(Environment.CurrentDirectory + "../../../../../EvolutionSim/SimulationResults.txt");

        public void Update(int numPreds, int numPrey)
        {
            Debug.WriteLine("here");
            _writer.WriteLine($"{numPreds},{numPrey}");
            _writer.Flush();
        }
    }
}
