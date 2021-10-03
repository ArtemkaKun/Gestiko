using Unity.Mathematics;

namespace GesturesSystem
{
	public class Point
	{
		private ulong ID { get; set; }
		private float2 Position { get; set; }
		private int2 LookUpTablePosition { get; set; }

		public Point (ulong id, float2 position)
		{
			ID = id;
			Position = position;
			LookUpTablePosition = int2.zero;
		}
	}
}
