using UnityEngine;

namespace GesturesSystem
{
	public static class GestureDataSystemConstants
	{
		public static string GesturesFolderPath { get; } = @$"{Application.persistentDataPath}\{GESTURES_CONTENT_FOLDER_NAME}\";

		public const string GESTURE_TAG = "Gesture";
		public const string GESTURE_NAME_PARAMETER = "Name";
		public const string POINT_TAG = "Point";
		public const string POINT_ID_PARAMETER = "ID";
		public const string POINT_X_COORDINATE_PARAMETER = "X";
		public const string POINT_Y_COORDINATE_PARAMETER = "Y";
		
		private const string GESTURES_CONTENT_FOLDER_NAME = "GesturesData";
	}
}