using System.Globalization;
using System.IO;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class GestureDataWriter
	{
		public void WriteGestureXML (Point[] points, string gestureName)
		{
			GesturesDataHelper.MakeSureGesturesDataFolderIsExists();
			WriteGestureDataToFile(points, gestureName, GetNewGestureDataFilePath(gestureName));
		}

		private string GetNewGestureDataFilePath (string gestureName)
		{
			string expectedNewGestureDataFilePath = CreateGestureDataFilePath(gestureName);

			if (File.Exists(expectedNewGestureDataFilePath) == true)
			{
				int fileCopyIndex = GetBiggestFileCopyIndex(gestureName) + 1;
				expectedNewGestureDataFilePath = CreateGestureDataFilePath($"{gestureName}{fileCopyIndex.ToString()}");
			}

			return expectedNewGestureDataFilePath;
		}

		private int GetBiggestFileCopyIndex (string gestureName)
		{
			string[] allFilePathsInDataDirectory = Directory.GetFiles(GestureDataSystemConstants.GesturesFolderPath);
			int biggestFileCopyIndex = 0;

			for (int filePathIndex = 0; filePathIndex < allFilePathsInDataDirectory.Length; filePathIndex++)
			{
				string cachedFilePath = allFilePathsInDataDirectory[filePathIndex];

				if (cachedFilePath.Contains(gestureName) == true)
				{
					if (int.TryParse(Path.GetFileNameWithoutExtension(cachedFilePath)[^1].ToString(), out int fileIndex) == true)
					{
						if (fileIndex > biggestFileCopyIndex)
						{
							biggestFileCopyIndex = fileIndex;
						}
					}
				}
			}

			return biggestFileCopyIndex;
		}

		private string CreateGestureDataFilePath (string gestureName)
		{
			return string.Format(GestureDataWriterConstants.GESTURE_DATA_FILE_NAME_TEMPLATE, GestureDataSystemConstants.GesturesFolderPath, gestureName);
		}

		private void WriteGestureDataToFile (Point[] points, string gestureName, string expectedNewGestureDataFilePath)
		{
			using StreamWriter newGestureWriter = new StreamWriter(expectedNewGestureDataFilePath);
			newGestureWriter.WriteLine(GestureDataWriterConstants.GESTURE_DATA_FILE_SERVICE_HEADER);
			newGestureWriter.WriteLine(GestureDataWriterConstants.GESTURE_OPEN_TAG_TEMPLATE, gestureName);

			for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
			{
				Point cachedPoint = points[pointIndex];
				float2 cachedPointPosition = cachedPoint.Position;
				newGestureWriter.WriteLine(GestureDataWriterConstants.POINT_TAG_TEMPLATE, cachedPoint.ID.ToString(), cachedPointPosition.x.ToString(CultureInfo.CurrentCulture), cachedPointPosition.y.ToString(CultureInfo.CurrentCulture));
			}

			newGestureWriter.WriteLine(GestureDataWriterConstants.GESTURE_CLOSE_TAG);
		}
	}
}