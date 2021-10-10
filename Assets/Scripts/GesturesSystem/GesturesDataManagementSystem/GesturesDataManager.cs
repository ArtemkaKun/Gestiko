using GesturesSystem.GestureTypes;

namespace GesturesSystem
{
	public class GesturesDataManager
	{
		private GestureDataWriter DataWriter { get; } = new GestureDataWriter();
		private GestureDataReader DataReader { get; } = new GestureDataReader();
		
		public Gesture ReadGestureOldFormat (string fileName)
		{
			return DataReader.ReadOldGesture(fileName);
		}

		public Gesture ReadGesture (string fileName)
		{
			return DataReader.ReadGesture(fileName);
		}

		public void WriteGesture (Point[] gesturePointsCollection, string gestureName)
		{
			DataWriter.WriteGestureXML(gesturePointsCollection, gestureName);
		}
	}
}