using Microsoft.AspNetCore.Mvc;
using Rezervacije.Controllers;

namespace Rezervacije.Test;

[TestFixture]
public class RootControllerTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Is_Root_Online()
    {
        // Arrange
        var controller = new RootController();

        // Act
        var response = controller.GetRootResponse();

        // Assert
        Assert.That(response, Is.InstanceOf<OkObjectResult>());
    }
}