namespace GesturesSystem
{
	public static class GestureDataWriterConstants
	{
		public const string GESTURE_DATA_FILE_NAME_TEMPLATE = "{0}{1}.xml";
		public const string GESTURE_DATA_FILE_SERVICE_HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
		public const string GESTURE_OPEN_TAG_TEMPLATE = "<" + GestureDataSystemConstants.GESTURE_TAG + " " + GestureDataSystemConstants.GESTURE_NAME_PARAMETER + " = \"{0}\">";
		public const string POINT_TAG_TEMPLATE = "\t<" + GestureDataSystemConstants.POINT_TAG + " " + GestureDataSystemConstants.POINT_ID_PARAMETER + " = \"{0}\" " + GestureDataSystemConstants.POINT_X_COORDINATE_PARAMETER + " = \"{1}\" " + GestureDataSystemConstants.POINT_Y_COORDINATE_PARAMETER + " = \"{2}\" />";
		public const string GESTURE_CLOSE_TAG = "</" + GestureDataSystemConstants.GESTURE_TAG + ">";
	}
}