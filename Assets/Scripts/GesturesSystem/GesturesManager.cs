using System.Collections.Generic;
using System.IO;
using GesturesSystem.GestureTypes;
using UnityEngine;

namespace GesturesSystem
{
	public class GesturesManager : MonoBehaviour
	{
		private Gesture[] TrainingSet { get; set; }
		private GesturesDataManager DataManager { get; set; } = new GesturesDataManager();

		private void Awake ()
		{
			TrainingSet = LoadTrainingSet();
		}

		private Gesture[] LoadTrainingSet ()
		{
			List<Gesture> gestures = new List<Gesture>();
			string[] gestureFolders = Directory.GetDirectories("Assets/GesturesData");

			foreach (string folder in gestureFolders)
			{
				string[] gestureFiles = Directory.GetFiles(folder, "*.xml");

				foreach (string file in gestureFiles)
				{
					gestures.Add(DataManager.ReadGesture(file));
				}
			}

			return gestures.ToArray();
		}
	}
}