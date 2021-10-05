using Unity.Mathematics;

namespace GesturesSystem
{
	public class Point
	{
		public ulong ID { get; private set; }
		public float2 Position { get; private set; }
		public int2 LookUpTablePosition { get; private set; }

		public Point (ulong id, float2 position)
		{
			ID = id;
			Position = position;
			LookUpTablePosition = int2.zero;
		}

		public void SetLookUpTablePosition (int2 newLookUpTablePosition)
		{
			LookUpTablePosition = newLookUpTablePosition;
		}
	}
}
