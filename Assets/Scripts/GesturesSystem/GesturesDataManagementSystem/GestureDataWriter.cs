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

			using StreamWriter newGestureWriter = new StreamWriter($"{GestureDataSystemConstants.GesturesFolderPath}{gestureName}.xml");

			newGestureWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
			newGestureWriter.WriteLine($"<Gesture Name = \"{gestureName}\">");
			
			for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
			{
				Point cachedPoint = points[pointIndex];
				float2 cachedPointPosition = cachedPoint.Position;
				newGestureWriter.WriteLine($"\t<Point ID = \"{cachedPoint.ID.ToString()}\" X = \"{cachedPointPosition.x.ToString(CultureInfo.CurrentCulture)}\" Y = \"{cachedPointPosition.y.ToString(CultureInfo.CurrentCulture)}\" />");
			}
			
			newGestureWriter.WriteLine("</Gesture>");
		}
	}
}