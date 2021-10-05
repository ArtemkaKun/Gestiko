using Unity.Mathematics;

namespace GesturesSystem.GestureTypes
{
	public static class GestureDatabase
	{
		public const int DEFAULT_NUMBER_OF_POINTS_IN_GESTURE = 64;
		public const int MAX_LOOK_UP_TABLE_COORDINATE = 1024;
		public const int DEFAULT_LOOK_UP_TABLE_SIZE = 64;
		public const int LOOK_UP_TABLE_SCALE_FACTOR = MAX_LOOK_UP_TABLE_COORDINATE / DEFAULT_LOOK_UP_TABLE_SIZE;
	}
}