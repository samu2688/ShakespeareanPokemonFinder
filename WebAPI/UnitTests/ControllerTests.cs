using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ViewModels.Data;
using WebAPIEndpoints.Controllers;

namespace UnitTests
{
    public class ControllerTests
    {
        private Mock<IBusinessService> _mockBusinessService;
        private PokemonController _controller;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            _mockBusinessService = new Mock<IBusinessService>();
            _controller = new PokemonController(_mockBusinessService.Object);
        }

        [Test]
        public void TestResponseType()
        {
            // Act
            var response = _controller.Get("Pikachu");
            // Assert
            Assert.IsInstanceOf(typeof(JsonResult), response);
        }

        [Test]
        public void TestOnData()
        {
            //Arrange
            var pokemonName = "Pikachu";
            _mockBusinessService.Setup(service => service.GetTranslation(pokemonName))
                .ReturnsAsync(new ResultViewModel
                {
                    HasError = false,
                    Status = "OK",
                    Translation = "Good translation"
                });
            // Act
            var response = _controller.Get(pokemonName);
            // Assert
            Assert.IsNotNull(response);
            var value = (ResultViewModel)((JsonResult)response).Value;
            Assert.AreEqual(value.HasError, false);
            Assert.AreEqual(value.Status, "OK");
            Assert.AreEqual(value.Translation, "Good translation");
        }

        [Test]
        public void TestOnCall()
        {
            //Arrange
            var pokemonName = "Pikachu";

            // Act
            var response = _controller.Get(pokemonName);
            // Assert
            _mockBusinessService.Verify(x => x.GetTranslation(pokemonName), Times.Once());
            _mockBusinessService.Verify(x => x.GetTranslation("no_pokemon"), Times.Never());
        }

        
    }
}
