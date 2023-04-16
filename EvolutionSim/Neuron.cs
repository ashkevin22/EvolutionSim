using System;

namespace EvolutionSim
{
	public class Connection
	{
		public double Weight;
		public double DeltaWeight = 0;

		public Connection(double weight)
		{
			this.Weight = weight;
		}
	}

	public class Neuron
	{
		public double m_outputVal;

		private static double Eta = 0.15;
		private static double Alpha = 0.5;
		private List<Connection> m_outputWeights;
		private int m_myIndex;
		private double m_gradient;

		/// <summary>
		/// Constructor that creates a neuron based on number of outputs
		/// </summary>
		/// <param name="numOutputs">number of outputs for the neuron</param>
		/// <param name="myIndex">index of neuron</param>
		public Neuron(int numOutputs, int myIndex)
		{
			for (int i = 0; i < numOutputs; i++)
			{
				m_outputWeights.Add(new Connection(RandomWeight()));
			}
		}

		/// <summary>
		/// Function to feed forward the inputs for the neural network
		/// </summary>
		/// <param name="prevLayer">previous layer in the neural net</param>
		public void FeedForward(List<Neuron> prevLayer)
		{
			double sum = 0;
			for(int i = 0; i < prevLayer.Count; i++)
			{
				sum += prevLayer[i].m_outputVal
					* prevLayer[i].m_outputWeights[m_myIndex].Weight;
			}
			m_outputVal = TransferFunction(sum);
		}

		/// <summary>
		/// Function to calculate the output gradients for a neuron
		/// </summary>
		/// <param name="targetVal">target value for neuron</param>
		public void CalcOutputGradients(double targetVal)
		{
            double delta = targetVal - m_outputVal;
            m_gradient = delta * TransferFunctionDerivative(m_outputVal);
		}

		/// <summary>
		/// Calculate the gradients for a hidden neuron
		/// </summary>
		/// <param name="nextLayer"></param>
		public void CalcHiddenGradients(List<Neuron> nextLayer)
		{
			double dow = SumDOW(nextLayer);
			m_gradient = dow * TransferFunctionDerivative(m_outputVal);
		}

		/// <summary>
		/// Function to update all of the input weights for the neurons
		/// </summary>
		/// <param name="prevLayer">previous layer to allow for updates</param>
		public void UpdateInputWeights(List<Neuron> prevLayer)
		{
			for(int i = 0; i < prevLayer.Count; i++)
			{
				Neuron neuron = prevLayer[i];
				double oldDeltaWeight = neuron.m_outputWeights[m_myIndex].DeltaWeight;
				double newDeltaWeight =
                    // Individual input, magnified by the gradient and train rate:
                    Eta
                    * neuron.m_outputVal
					* m_gradient
                    // Also add momentum = a fraction of the previous delta weight:
                    + Alpha
					* oldDeltaWeight;

				neuron.m_outputWeights[m_myIndex].DeltaWeight = newDeltaWeight;
				neuron.m_outputWeights[m_myIndex].Weight += newDeltaWeight;
			}
		}

		/// <summary>
		/// Function to sum the weights of the next layer
		/// </summary>
		/// <param name="nextLayer">next layer of neurons</param>
		/// <returns>sum of weights</returns>
		private double SumDOW(List<Neuron> nextLayer)
		{
			double sum = 0;
			for(int i = 0; i < nextLayer.Count - 1; i++)
			{
				sum += m_outputWeights[i].Weight * nextLayer[i].m_gradient;
			}
			return sum;
		}

		/// <summary>
		/// Transfer function for neurons
		/// </summary>
		/// <param name="x">sum of weights</param>
		/// <returns>double after transfer</returns>
		private static double TransferFunction(double x)
		{
			return Math.Tanh(x);
		}

		/// <summary>
		/// Derivative of the transfer function
		/// </summary>
		/// <param name="x">input value</param>
		/// <returns>double after transfer derivative</returns>
		private static double TransferFunctionDerivative(double x)
		{
			return 1.0 - x * x;
		}

		/// <summary>
		/// Function to generate a random weight between 0 and 1 for Connection init
		/// </summary>
		/// <returns>random double between 0 and 1</returns>
        private static double RandomWeight()
        {
			Random rand = new();
			return rand.NextDouble();
		}
	}
}

