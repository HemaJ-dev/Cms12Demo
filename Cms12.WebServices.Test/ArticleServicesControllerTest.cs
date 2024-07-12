using Cms12.WebServices.Controllers;
using Cms12.WebServices.Models;
using Cms12.WebServices.Services;
using Cms12.WebServices.Test.Logging;
using EPiServer.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Xunit.Abstractions;

namespace Cms12.WebServices.Test
{
    public class ArticleServicesControllerTest : IAssemblyFixture<ServiceLocatorFixture>
    {
        private readonly ServiceLocatorFixture _serviceLocatorFixture;
        private readonly ITestOutputHelper _testOutputHelper;
        ArticleServicesController articleServicesController;
        ArticleServices articleServices;

        public ArticleServicesControllerTest(ServiceLocatorFixture serviceLocatorFixture, ITestOutputHelper testOutputHelper)
        {
            _serviceLocatorFixture = serviceLocatorFixture;
            _testOutputHelper = testOutputHelper;
            articleServices = new ArticleServices();
            articleServicesController = new ArticleServicesController(articleServices);
        }

        [Fact]
        public void GetArticle()
        {

            var logger = XUnitLogger.CreateLogger(_testOutputHelper);

            //Arrange
            //Act
            var result = articleServicesController.GetArticle(11);
            //Assert
            Assert.IsType<ObjectResult>(result);

            var item = result as ObjectResult;
            var resultAsJson = JsonSerializer.Serialize(item.Value);
            logger.Log(LogLevel.Information, resultAsJson);
            Assert.Equal("Article Not Found", item.Value);
        }

        [Fact]
        public void CreateArticle()
        {
            //Arrange
            var logger = XUnitLogger.CreateLogger(_testOutputHelper);
            var article = new ArticleModel();
            article.Title = "Weather updates:IMD issues heavy rain alert for Bihar, Goa today.";
            article.Subject = "Weather updates: The IMD has issued an 'orange alert' for heavy rainfall in Bihar, with a 'red alert' for very heavy rainfall predicted for Friday.";
            article.PublishDate = "07-11-2024";
            article.ImageUrl = "http://localhost:8000//globalassets/articles/images/christian-lue-2xph3hyi00i-unsplash.jpg";
            article.ArticleBody = "<div class=\"storyParagraphFigure\">\n<p>The India Meteorological Department (IMD) has predicted increased rainfall activity in north and northeast India, including East Uttar Pradesh,<a class=\"manualbacklink\" href=\"https://www.hindustantimes.com/topic/delhi-ncr\" target=\"_blank\" rel=\"noopener\">&nbsp;Delhi-NCR</a>, and other regions, in the coming days. The weather body has further issued an &lsquo;orange alert&rsquo; for heavy rainfall on Thursday, July 11, in the state of Bihar and parts of Uttar Pradesh.</p>\n<p><picture></picture>According to the regional meteorological centre in New Delhi, a generally cloudy sky with very light to light rain is expected in Delhi-NCR on Thursday. The regional met centre forecasts a maximum temperature of 36 degrees Celsius and a minimum temperature of 26 degrees Celsius for Thursday. IMD officials indicated on Wednesday that the intensity of rainfall in Delhi is expected to increase in the coming days.</p>\n</div>\n";
            article.Author = "Lingamgunta Nirmitha Rao";
            //Act
            var result = articleServicesController.PublishArticle(article);
            //Assert
            var articleResult = result as ObjectResult;
            var resultAsJson = JsonSerializer.Serialize(articleResult);
            logger.Log(LogLevel.Information, resultAsJson);
            Assert.Equal("Internal Server Error", articleResult.Value);
        }

        [Fact]
        public void UpdateArticle()
        {
            //Arrange
            var logger = XUnitLogger.CreateLogger(_testOutputHelper);
            var article = new ArticleModel();
            article.ArticleId = 10;
            article.Title = "Weather updates:IMD issues heavy rain alert for Bihar, Goa today.";
            article.Subject = "Weather updates: The IMD has issued an 'orange alert' for heavy rainfall in Bihar, with a 'red alert' for very heavy rainfall predicted for Friday.";
            article.PublishDate = "07-12-2024";
            article.ImageUrl = "http://localhost:8000//globalassets/articles/images/christian-lue-2xph3hyi00i-unsplash.jpg";
            article.ArticleBody = "<div class=\"storyParagraphFigure\">\n<p>The India Meteorological Department (IMD) has predicted increased rainfall activity in north and northeast India, including East Uttar Pradesh,<a class=\"manualbacklink\" href=\"https://www.hindustantimes.com/topic/delhi-ncr\" target=\"_blank\" rel=\"noopener\">&nbsp;Delhi-NCR</a>, and other regions, in the coming days. The weather body has further issued an &lsquo;orange alert&rsquo; for heavy rainfall on Thursday, July 11, in the state of Bihar and parts of Uttar Pradesh.</p>\n<p><picture></picture>According to the regional meteorological centre in New Delhi, a generally cloudy sky with very light to light rain is expected in Delhi-NCR on Thursday. The regional met centre forecasts a maximum temperature of 36 degrees Celsius and a minimum temperature of 26 degrees Celsius for Thursday. IMD officials indicated on Wednesday that the intensity of rainfall in Delhi is expected to increase in the coming days.</p>\n</div>\n";
            article.Author = "Lingamgunta Nirmitha Rao";
            //Act
            var result = articleServicesController.UpdateArticle(article);
            //Assert
            var articleResult = result as ObjectResult;
            var resultAsJson = JsonSerializer.Serialize(articleResult);
            logger.Log(LogLevel.Information, resultAsJson);
            Assert.Equal("Please pass valid Article id", articleResult.Value);
        }

        [Fact]
        public void DeleteArticle()
        {
            var logger = XUnitLogger.CreateLogger(_testOutputHelper);

            //Arrange
            //Act
            var result = articleServicesController.DeleteArticle(11);
            //Assert
            Assert.IsType<ObjectResult>(result);

            var item = result as ObjectResult;
            var resultAsJson = JsonSerializer.Serialize(item.Value);
            logger.Log(LogLevel.Information, resultAsJson);
            Assert.Equal("Article Not Found", item.Value);
        }
    }
}