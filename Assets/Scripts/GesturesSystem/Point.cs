using System;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class Point : IEquatable<Point>
	{
		public int ID { get; private set; }
		public float2 Position { get; private set; }
		public int2 LookUpTablePosition { get; private set; }

		public Point (int id, float2 position)
		{
			ID = id;
			Position = position;
			LookUpTablePosition = int2.zero;
		}

		public void SetLookUpTablePosition (int2 newLookUpTablePosition)
		{
			LookUpTablePosition = newLookUpTablePosition;
		}
		
		public bool Equals (Point other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return ID == other.ID && Position.Equals(other.Position) && LookUpTablePosition.Equals(other.LookUpTablePosition);
		}
	}
}
