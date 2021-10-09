using System.IO;
using UnityEngine;

namespace GesturesSystem
{
	public static class GestureDataSystemConstants
	{
		public static string GesturesFolderPath { get; } = @$"{Application.persistentDataPath}\{GESTURES_CONTENT_FOLDER_NAME}\";

		private const string GESTURES_CONTENT_FOLDER_NAME = "GesturesData";
	}
}