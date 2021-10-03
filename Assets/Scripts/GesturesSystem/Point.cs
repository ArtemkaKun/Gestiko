using Unity.Mathematics;

namespace GesturesSystem
{
	public class Point
	{
		public ulong ID { get; private set; }
		public float2 Position { get; private set; }
		private int2 LookUpTablePosition { get; set; }

		public Point (ulong id, float2 position)
		{
			ID = id;
			Position = position;
			LookUpTablePosition = int2.zero;
		}
	}
}
