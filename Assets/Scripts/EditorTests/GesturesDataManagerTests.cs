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
        Gesture rewrittenNewGesture = DataManager.ReadGesture(string.Format(GestureDataWriterConstants.GESTURE_DATA_FILE_NAME_TEMPLATE, GestureDataSystemConstants.GesturesFolderPath, testGesture.Name));
        CollectionAssert.AreEqual(testGesture.RawPointsCollection, rewrittenNewGesture.RawPointsCollection);
    }
}
