using System.Collections.Generic;
using System.IO;
using System.Xml;
using GesturesSystem.GestureTypes;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class GestureDataReader
	{
		public Gesture ReadGesture (string fileName)
		{
			List<Point> points = new List<Point>();
			string gestureName = "";

			if (File.Exists(fileName) == true)
			{
				using XmlTextReader xmlReader = new XmlTextReader(File.OpenText(fileName));
				
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType != XmlNodeType.Element)
					{
						continue;
					}

					switch (xmlReader.Name)
					{
						case GestureDataSystemConstants.GESTURE_TAG:
							gestureName = xmlReader[GestureDataSystemConstants.GESTURE_NAME_PARAMETER];
							break;
						case GestureDataSystemConstants.POINT_TAG:
							points.Add(new Point(int.Parse(xmlReader[GestureDataSystemConstants.POINT_ID_PARAMETER] ?? string.Empty), new float2(float.Parse(xmlReader[GestureDataSystemConstants.POINT_X_COORDINATE_PARAMETER] ?? string.Empty), float.Parse(xmlReader[GestureDataSystemConstants.POINT_Y_COORDINATE_PARAMETER] ?? string.Empty))));
							break;
					}
				}
			}

			return new Gesture(points.ToArray(), gestureName);
		}
	}
}