namespace GesturesSystem
{
	public static class GestureDataWriterConstants
	{
		public const string GESTURE_DATA_FILE_NAME_TEMPLATE = "{0}{1}.xml";
		public const string GESTURE_DATA_FILE_SERVICE_HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
		public const string GESTURE_OPEN_TAG_TEMPLATE = "<Gesture Name = \"{0}\">";
		public const string POINT_TAG_TEMPLATE = "\t<Point ID = \"{0}\" X = \"{1}\" Y = \"{2}\" />";
		public const string GESTURE_CLOSE_TAG = "</Gesture>";
	}
}