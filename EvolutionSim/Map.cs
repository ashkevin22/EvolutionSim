using System;
namespace EvolutionSim
{
	public class Circle
	{
		public (double x, double y) Center;
		public double Radius;

		public Circle((double x, double y) Center, double Radius)
		{
			this.Center = Center;
			this.Radius = Radius;
		}
	}

	public class Map
	{
		public readonly double xBound;
		public readonly double yBound;
		public readonly List<Circle> Lakes = new();

		/// <summary>
		/// Constructor for the Map object 
		/// </summary>
		/// <param name="xBound"></param>
		/// <param name="yBound"></param>
		/// <param name="numberOfLakes"></param>
		public Map(double xBound, double yBound, int numberOfLakes)
		{
			Random rand = new();
			this.xBound = xBound;
			this.yBound = yBound;
			(double x, double y) newLake;
			double radius;
			for(int i = 0; i < numberOfLakes; i++)
			{
				newLake.x = rand.NextInt64(2, (int)Math.Round(xBound) - 2);
				newLake.y = rand.NextInt64(2, (int)Math.Round(xBound) - 2);
				radius = rand.NextInt64(1, (int)Math.Round(xBound / 10));
				Lakes.Add(new Circle(newLake, radius));
            }
		}

		/// <summary>
		/// Function to check if a given position is a valid position or not
		/// </summary>
		/// <param name="x">x position to check if valid</param>
		/// <param name="y">y position to check if valid</param>
		/// <returns></returns>
		public bool CheckValidPos(double x, double y)
		{
			if(x > xBound || x < 0 || y > yBound || y < 0) { return false; }
			for (int i = 0; i < Lakes.Count; i++)
			{
				Circle Lake = Lakes[i];
				if (Math.Pow((x - Lake.Center.x), 2) + Math.Pow((y - Lake.Center.y), 2) < Math.Pow(Lake.Radius, 2)) { return false; }
			}
			return true;
		}
	}
}

