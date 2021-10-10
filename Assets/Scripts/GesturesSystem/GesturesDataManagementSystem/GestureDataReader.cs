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

		public Gesture ReadOldGesture (string fileName)
		{
			List<Point> points = new List<Point>();
			XmlTextReader xmlReader = null;
			int currentStrokeIndex = -1;
			string gestureName = "";

			try
			{
				xmlReader = new XmlTextReader(File.OpenText(fileName));

				while (xmlReader.Read())
				{
					if (xmlReader.NodeType != XmlNodeType.Element)
					{
						continue;
					}

					switch (xmlReader.Name)
					{
						case "Gesture":
							gestureName = xmlReader["Name"];

							if (gestureName != null && gestureName.Contains("~"))
							{
								gestureName = gestureName[..gestureName.LastIndexOf('~')];
							}

							if (gestureName != null && gestureName.Contains("_"))
							{
								gestureName = gestureName.Replace('_', ' ');
							}

							break;
						case "Stroke":
							currentStrokeIndex++;
							break;
						case "Point":
							points.Add(new Point(currentStrokeIndex, new float2(float.Parse(xmlReader["X"] ?? string.Empty), float.Parse(xmlReader["Y"] ?? string.Empty))));
							break;
					}
				}
			}
			finally
			{
				xmlReader?.Close();
			}

			return new Gesture(points.ToArray(), gestureName);
		}
	}
}