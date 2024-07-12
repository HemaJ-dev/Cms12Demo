using Microsoft.AspNetCore.Mvc;
using Cms12.WebServices.Models;
using Cms12.WebServices.Services;


namespace Cms12.WebServices.Controllers
{    
    [ApiController]
    [Route("api/ArticleServices")]
    public class ArticleServicesController : ControllerBase
    {
        public readonly IArticleServices _articleService;

        public ArticleServicesController(IArticleServices articleServices)
        {
            _articleService = articleServices;
        }

        [HttpGet(Name = "GetArticle")]
        public IActionResult GetArticle(int contentId)
        {
            try
            {
                //content id cannot be 0 or less than 0
                if (contentId <= 0)
                {
                    return BadRequest("Please send valid Article Id");
                }
                //Get the article from Content Repository
                var article = _articleService.GetArticle(contentId);               
                if (article != null)
                {
                    return new JsonResult(article);
                }
                //if article is null from GetArticle service, it is not found in the database.
                return StatusCode(404, "Article Not Found");
            }
            catch (Exception ex)
            {
                //return error message if any exception occurs while fetching the content.
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost(Name = "PublishArticle")]
        public IActionResult PublishArticle([FromBody] ArticleModel articleModel)
        {
            try
            {
                //validate the input object as it has few required fields to create the object.
                if (!ModelState.IsValid)
                {
                    return BadRequest("Request object is not valid");
                }
                //create and publish article in CMS
                int articleId = _articleService.CreateArticle(articleModel);
                return new JsonResult(articleModel.Title + "is created successfully with id: " + articleId);
            }
            catch (Exception ex)
            {
                //return error message if any exception occurs while creating the content.
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(Name = "UpdateArticle")]
        public IActionResult UpdateArticle([FromBody] ArticleModel articleModel)
        {
            try
            {
                //ArticleId should be greater than 0 as this is an update.
                if (articleModel.ArticleId <= 0)
                {
                    return BadRequest("Please pass valid Article id");
                }
                //validate input object as few fields are mandatory for creating/updating the Article.
                if (!ModelState.IsValid)
                {
                    return BadRequest("Request object is not valid");
                }
                //pass the second parameter as true to inform that this call is for update not for creation.
                int articleId = _articleService.CreateArticle(articleModel, true);
                if(articleId == 0)
                {
                    return StatusCode(404, "Please pass valid Article id");
                }
                return new JsonResult(articleModel.Title + "is updated successfully");
            }
            catch (Exception ex)
            {
                //return error message if any exception occurs while updating the content.
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteArticle")]
        public IActionResult DeleteArticle([FromBody] int contentId)
        {
            //Content id should be valid to delete the article
            if (contentId <= 0)
            {
                return BadRequest("Please send valid Article Id");
            }
            //Delete the article from CMS.
            bool? isDeleted = _articleService.DeleteArticle(contentId);
            if (isDeleted == null)
            {
                return StatusCode(404, "Article Not Found");
            }
            if (isDeleted != null && true)
            {
                return Ok(new { Status = "Article deleted successfully." });
            }
            return StatusCode(500, "Internal Server Error");
        }
    }
}
