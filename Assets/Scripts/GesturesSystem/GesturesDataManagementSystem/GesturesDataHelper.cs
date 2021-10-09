using System.IO;

namespace GesturesSystem
{
	public static class GesturesDataHelper
	{
		public static bool CheckIfGesturesDataFolderIsExists ()
		{
			return Directory.Exists(GestureDataSystemConstants.GesturesFolderPath);
		}

		public static void MakeSureGesturesDataFolderIsExists ()
		{
			if (CheckIfGesturesDataFolderIsExists() == false)
			{
				Directory.CreateDirectory(GestureDataSystemConstants.GesturesFolderPath);
			}
		}
	}
}