using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using ViewModels.Data;
using ViewModels.Enum;
using WebAPIEndpoints.Controllers;

namespace UnitTests
{
    public class BusinessLogicTests
    {
        private IConfiguration _configuration;
        private Mock<ILogger<BusinessService>> _mockLog;
        private IBusinessService _businessService;

        [SetUp]
        public void Setup()
        {
            //Arrange
            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"ShakespeareTranslatorEndpoint", "https://api.funtranslations.com/translate/shakespeare.json?text={0}"},
                {"PokemonEndpoint", "https://pokeapi.co/api/v2/pokemon-species/{0}/"},
                {"MaxRetryAttempts", "10"},
                {"PauseBetweenFailures", "2"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockLog = new Mock<ILogger<BusinessService>>();
            _businessService = new BusinessService(_mockLog.Object, _configuration);
        }

        [Test]
        public void TestResponseType()
        {
            // Act
            var response = _businessService.GetTranslation("Pikachu").Result;
            // Assert
            Assert.IsInstanceOf(typeof(ResultViewModel), response);
        }

        [Test]
        public void TestOnData()
        {
            // Act
            var responseOK = _businessService.GetTranslation("Pikachu").Result;
            var responseNO_DATA = _businessService.GetTranslation(null).Result;

            // Assert
            Assert.AreEqual(responseOK.HasError, false);
            Assert.AreEqual(responseOK.Status, ResultEnum.RESULT.OK.ToString());
            Assert.IsTrue(responseOK.Translation.Length > 0);

            Assert.AreEqual(responseNO_DATA.HasError, true);
            Assert.AreEqual(responseNO_DATA.Status, ResultEnum.RESULT.NO_DATA.ToString());
            Assert.IsNull(responseNO_DATA.Translation);
        }


    }
}
