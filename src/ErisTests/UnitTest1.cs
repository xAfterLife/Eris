using ErisLib.Enums;

namespace ErisTests;

public class DownloadSpeedTests
{
    [TestMethod]
    public void GetDownloadSpeed_ValidInput_ReturnsCorrectValue()
    {
        // Arrange
        var downloadQueue = new List<(DateTime, long)>
            {
                (DateTime.Now, 0),
                (DateTime.Now.AddSeconds(1), 1024),
            };
        const DownloadSpeedType speedType = DownloadSpeedType.KBs;
        const int expected = 1024 / 1000;

        // Act
        var result = GetDownloadSpeed(downloadQueue, speedType);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetDownloadSpeed_InsufficientData_ReturnsZero()
    {
        // Arrange
        var downloadQueue = new List<(DateTime, long)>();
        const DownloadSpeedType speedType = DownloadSpeedType.KBs;
        const int expected = 0;

        // Act
        var result = GetDownloadSpeed(downloadQueue, speedType);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
