using System.IO;
using GesturesSystem;
using GesturesSystem.GestureTypes;
using NUnit.Framework;

public class GesturesDataManagerTests
{
    private GesturesDataManager DataManager { get; set; }
    
    [OneTimeSetUp]
    public void Initialize ()
    {
        DataManager = new GesturesDataManager();
    }
    
    [Test]
    public void WriteTestToFileAndCheckIfThisFileContainsRightData ()
    {
        Gesture testGesture = DataManager.ReadGestureOldFormat("Assets/GesturesData/10-stylus-MEDIUM/10-stylus-medium-arrowhead-01.xml");
        DataManager.WriteGesture(testGesture.RawPointsCollection, testGesture.Name);
        string rewrittenGestureFilePath = string.Format(GestureDataWriterConstants.GESTURE_DATA_FILE_NAME_TEMPLATE, GestureDataSystemConstants.GesturesFolderPath, testGesture.Name);
        Gesture rewrittenNewGesture = DataManager.ReadGesture(rewrittenGestureFilePath);
        File.Delete(rewrittenGestureFilePath);
        CollectionAssert.AreEqual(testGesture.RawPointsCollection, rewrittenNewGesture.RawPointsCollection);
    }

    [Test]
    public void Create3GestureFilesWithSameNameAndCheckFilesCountAndNames ()
    {
        Gesture testGesture = DataManager.ReadGestureOldFormat("Assets/GesturesData/10-stylus-MEDIUM/10-stylus-medium-arrowhead-01.xml");

        for (int fileRepeatIndex = 0; fileRepeatIndex < 3; fileRepeatIndex++)
        {
            DataManager.WriteGesture(testGesture.RawPointsCollection, testGesture.Name);
        }

        for (int fileRepeatIndex = 0; fileRepeatIndex < 3; fileRepeatIndex++)
        {
            string currentFilePath = string.Format(GestureDataWriterConstants.GESTURE_DATA_FILE_NAME_TEMPLATE, GestureDataSystemConstants.GesturesFolderPath, $"{testGesture.Name}{(fileRepeatIndex == 0 ? "" : fileRepeatIndex.ToString())}");
            
            if (File.Exists(currentFilePath) == false)
            {
                Assert.Fail($"File {currentFilePath} expected but not found");
                return;
            }

            File.Delete(currentFilePath);
        }
    }
}
