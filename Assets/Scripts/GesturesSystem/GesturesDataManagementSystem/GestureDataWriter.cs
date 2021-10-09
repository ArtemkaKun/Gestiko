using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
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
			int currentStrokeID = -1;
			
			for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
			{
				Point cachedPoint = points[pointIndex];
				int cachedPointID = cachedPoint.ID;
				
				if (cachedPointID != currentStrokeID)
				{
					if (pointIndex > 0)
					{
						newGestureWriter.WriteLine("\t</Stroke>");
					}
			
					newGestureWriter.WriteLine("\t<Stroke>");
					currentStrokeID = cachedPointID;
				}

				float2 cachedPointPosition = points[pointIndex].Position;
				newGestureWriter.WriteLine($"\t\t<Point X = \"{cachedPointPosition.x.ToString(CultureInfo.CurrentCulture)}\" Y = \"{cachedPointPosition.y.ToString(CultureInfo.CurrentCulture)}\" />");
			}

			newGestureWriter.WriteLine("\t</Stroke>");
			newGestureWriter.WriteLine("</Gesture>");
		}
	}
}