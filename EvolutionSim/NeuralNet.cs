using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace EvolutionSim
{
	public class NeuralNet
	{
		private List<List<Neuron>> m_layers = new();
		private double m_error;
		private double m_recentAverageError;
		private double m_recentAverageSmoothingFactor;

		/// <summary>
		/// Constructor for NeuralNet, creates a neural network based on the topology List
		/// </summary>
		/// <param name="topology">List describing the topology of the neural network</param>
		public NeuralNet(List<int> topology)
		{
			int numLayers = topology.Count;
			for(int layerNum = 0; layerNum < numLayers; layerNum++)
			{
				m_layers.Add(new List<Neuron>());
				int numOutputs = layerNum == topology.Count - 1 ? 0 : topology[layerNum + 1];

				for(int neuronNum = 0; neuronNum <= topology[layerNum]; neuronNum++)
				{
					m_layers.Last().Add(new Neuron(numOutputs, neuronNum));
				}
				m_layers.Last().Last().m_outputVal = 1.0;
			}
		}

		/// <summary>
		/// Funciton to get the results from the feed forward function of the neural network
		/// </summary>
		/// <returns>outputs of the nodes in the final layer of the network</returns>
		public List<double> GetResults()
		{
			List<double> results = new();

			for(int n = 0; n < m_layers.Last().Count - 1; n++)
			{
				results.Add(m_layers.Last()[n].m_outputVal);
			}
			return results;
		}

		/// <summary>
		/// Function to feed forward the input values through the entire neural network
		/// </summary>
		/// <param name="inputVals">Initial input values for the neural network</param>
		public void FeedForward(List<double> inputVals)
		{
			//Debug.Assert(inputVals.Count == m_layers[0].Count - 1);

			for(int i = 0; i < inputVals.Count; i++)
			{
				m_layers[0][i].m_outputVal = inputVals[i];
			}

			for(int layerNum = 1; layerNum < m_layers.Count; layerNum++)
			{
				List<Neuron> prevLayer = m_layers[layerNum - 1];
				for(int i = 0; i < m_layers[layerNum].Count - 1; i++)
				{
					m_layers[layerNum][i].FeedForward(prevLayer);
				}
			}
		}

		/// <summary>
		/// Function to propogate backwards through the neural network and adjust all of the weights
		/// </summary>
		/// <param name="targetVals">expected outputs of the neural network</param>
		public void BackProp(List<double> targetVals)
		{
			List<Neuron> outputLayer = m_layers.Last();
			m_error = 0.0;

			for (int n = 0; n < outputLayer.Count - 1; ++n)
			{
				double delta = targetVals[n] - outputLayer[n].m_outputVal;
				m_error += delta * delta;
			}
			m_error /= outputLayer.Count - 1; // get average error squared
			m_error = Math.Sqrt(m_error); // RMS

			m_recentAverageError =
					(m_recentAverageError * m_recentAverageSmoothingFactor + m_error)
					/ (m_recentAverageSmoothingFactor + 1.0);

			for (int n = 0; n < outputLayer.Count - 1; ++n)
			{
				outputLayer[n].CalcOutputGradients(targetVals[n]);
			}

			for (int layerNum = m_layers.Count - 2; layerNum > 0; --layerNum)
			{
				List<Neuron> hiddenLayer = m_layers[layerNum];
				List<Neuron> nextLayer = m_layers[layerNum + 1];

				for (int n = 0; n < hiddenLayer.Count; ++n)
				{
					hiddenLayer[n].CalcHiddenGradients(nextLayer);
				}
			}

			for (int layerNum = m_layers.Count - 1; layerNum > 0; --layerNum)
			{
				List<Neuron> layer = m_layers[layerNum];
				List<Neuron> prevLayer = m_layers[layerNum - 1];

				for (int n = 0; n < layer.Count - 1; ++n)
				{
					layer[n].UpdateInputWeights(prevLayer);
				}
			}
		}
	}
}