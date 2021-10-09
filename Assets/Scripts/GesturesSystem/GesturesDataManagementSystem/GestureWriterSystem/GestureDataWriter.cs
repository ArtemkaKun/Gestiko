using System.Globalization;
using System.IO;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class GestureDataWriter
	{
		public void WriteGestureXML (Point[] points, string gestureName)
		{
			if (Directory.Exists(GestureDataSystemConstants.GesturesFolderPath) == false)
			{
				Directory.CreateDirectory(GestureDataSystemConstants.GesturesFolderPath);
			}

			using StreamWriter newGestureWriter = new StreamWriter(string.Format(GestureDataWriterConstants.GESTURE_DATA_FILE_NAME_TEMPLATE, GestureDataSystemConstants.GesturesFolderPath, gestureName));
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