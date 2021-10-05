using System.Collections.Generic;
using System.IO;
using System.Xml;
using GesturesSystem.GestureTypes;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class GesturesDataManager
	{
		public Gesture ReadGesture (string fileName)
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

		public void WriteGesture (Point[] points, string gestureName, string fileName)
		{
			using StreamWriter sw = new StreamWriter(fileName);

			sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
			sw.WriteLine("<Gesture Name = \"{0}\">", gestureName);
			int currentStroke = -1;

			for (int i = 0; i < points.Length; i++)
			{
				if (points[i].ID != currentStroke)
				{
					if (i > 0)
					{
						sw.WriteLine("\t</Stroke>");
					}

					sw.WriteLine("\t<Stroke>");
					currentStroke = points[i].ID;
				}

				sw.WriteLine("\t\t<Point X = \"{0}\" Y = \"{1}\" T = \"0\" Pressure = \"0\" />", points[i].Position.x, points[i].Position.y);
			}

			sw.WriteLine("\t</Stroke>");
			sw.WriteLine("</Gesture>");
		}
	}
}