using EvolutionSim;

namespace EvolutionSim
{
    public class Sandbox
    {
        static void Main(string[] args)
        {
            List<int> topology = new List<int> { 4, 2 };

            List<double> forward = new();
            forward.Add(0.989);
            forward.Add(0.232);
            forward.Add(0.989);
            forward.Add(0.232);

            List<double> backward = new();
            backward.Add(1);
            backward.Add(0.5);

            NeuralNet myNet = new(topology);
            myNet.FeedForward(forward);
            List<double> results = myNet.GetResults();
            for(int i = 0; i < results.Count; i++)
            {
                Console.WriteLine(results[i]);
            }
        }
    }
}