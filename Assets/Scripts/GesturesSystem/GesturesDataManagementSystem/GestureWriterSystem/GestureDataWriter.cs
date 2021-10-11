using System.Globalization;
using System.IO;
using System.Xml;
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
			using FileStream newGesturesFileStream = File.Create(expectedNewGestureDataFilePath);
			using XmlWriter newFileXMLWriter = XmlWriter.Create(newGesturesFileStream, new XmlWriterSettings { Indent = true });

			newFileXMLWriter.WriteStartDocument();
			newFileXMLWriter.WriteStartElement(GestureDataSystemConstants.GESTURE_TAG);
			newFileXMLWriter.WriteAttributeString(GestureDataSystemConstants.GESTURE_NAME_PARAMETER, gestureName);

			for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
			{
				Point cachedPoint = points[pointIndex];
				WritePointData(cachedPoint, newFileXMLWriter);
			}

			newFileXMLWriter.WriteEndElement();
		}

		private void WritePointData (Point cachedPoint, XmlWriter newFileXMLWriter)
		{
			float2 cachedPointPosition = cachedPoint.Position;
			newFileXMLWriter.WriteStartElement(GestureDataSystemConstants.POINT_TAG);
			newFileXMLWriter.WriteAttributeString(GestureDataSystemConstants.POINT_ID_PARAMETER, cachedPoint.ID.ToString());
			WritePointPositionCoordinate(GestureDataSystemConstants.POINT_X_COORDINATE_PARAMETER, cachedPointPosition.x, newFileXMLWriter);
			WritePointPositionCoordinate(GestureDataSystemConstants.POINT_Y_COORDINATE_PARAMETER, cachedPointPosition.y, newFileXMLWriter);
			newFileXMLWriter.WriteEndElement();
		}

		private void WritePointPositionCoordinate (string attributeName, float coordinateValue, XmlWriter newFileXMLWriter)
		{
			newFileXMLWriter.WriteAttributeString(attributeName, coordinateValue.ToString(CultureInfo.CurrentCulture));
		}
	}
}