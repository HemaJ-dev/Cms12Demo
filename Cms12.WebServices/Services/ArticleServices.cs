using Cms12.WebServices.Models;
using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.Web.Routing;
using EPiServer.Security;
using System.Globalization;
using Cms12.Models.Pages;


namespace Cms12.WebServices.Services
{
    /// <summary>
    /// Interface for CRUD operations for Article pages in CMS
    /// </summary>
    public interface IArticleServices
    {
        ArticleModel GetArticle(int contentId);
        int CreateArticle(ArticleModel article, bool isUpdate = false);
        bool? DeleteArticle(int contentId);
    }


    /// <summary>
    /// Implementation for CRUD operations for Article pages in CMS
    /// </summary>
    public class ArticleServices : IArticleServices
    {

        private readonly IContentRepository _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        private readonly IUrlResolver _urlResolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
        private readonly EPiServer.Logging.ILogger logger = LogManager.GetLogger(typeof(ArticleServices));

        /// <summary>
        /// Gets Article from CMS based on content id.
        /// </summary>
        /// <typeparam name="contentId"></typeparam>
        /// <returns>Article</returns>
        public ArticleModel GetArticle(int contentId)
        {
            try
            {
                #region Get the article from CMS database.
                var articleReference = new ContentReference(contentId);
                ArticlePage article = _contentRepository.Get<ArticlePage>(articleReference);
                #endregion
                #region Return null if Article not found.
                if (article == null)
                {
                    return null;
                }
                #endregion
                #region Map the article data with response object.
                var imageUrl = article.ArticleImage != null ? "http://localhost:8000/" + _urlResolver.GetUrl(new ContentReference(article.ArticleImage.ID)) : null;
                ArticleModel articleModel = new ArticleModel
                {
                    ArticleId = article.ContentLink.ID,
                    Title = article.Title,
                    Author = article.Author ?? string.Empty,
                    ArticleBody = article.ArticleBody.ToString(),
                    Subject = article.Subject ?? string.Empty,
                    PublishDate = article.PublishDate.ToString("MM-dd-yyyy"),
                    ImageUrl = imageUrl ?? string.Empty
                };
                #endregion

                return articleModel;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Publish or Update an Article in CMS based on optional parameter.
        /// </summary>
        /// <typeparam name="articleModel"></typeparam>
        /// <typeparam name="isUpdate"></typeparam>
        /// <returns>Article id</returns>
        public int CreateArticle(ArticleModel articleModel, bool isUpdate = false)
        {
            try
            {
                #region Get Default article page to create or Create writableclone to update the existing article.
                ArticlePage article = null;
                if (isUpdate)
                {
                    article = _contentRepository.Get<ArticlePage>(new ContentReference(articleModel.ArticleId));
                    if (article == null)
                    {
                        return 0;
                    }
                    article = article.CreateWritableClone() as ArticlePage;
                }
                else
                {
                    article = _contentRepository.GetDefault<ArticlePage>(new ContentReference(1));
                }
                #endregion
                #region Set values from input
                article.Name = articleModel.Title;
                article.Title = articleModel.Title;
                article.PublishDate = DateTime.ParseExact(articleModel.PublishDate, "MM-dd-yyyy", new CultureInfo("en-GB"));
                article.Subject = articleModel.Subject ?? string.Empty;
                article.Author = articleModel.Author ?? string.Empty;
                article.ArticleBody = new XhtmlString(articleModel.ArticleBody);
                if (articleModel.ImageUrl != null)
                {
                    var imageReference = _urlResolver.Route(new UrlBuilder(articleModel.ImageUrl));
                    article.ArticleImage = imageReference != null ? imageReference.ContentLink : new ContentReference();
                }
                #endregion
                #region publish the article in CMS
                var articlePage = _contentRepository.Publish(article, AccessLevel.NoAccess);
                #endregion
                return articlePage.ID;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes an Article from CMS based on content id.
        /// </summary>
        /// <typeparam name="contentId"></typeparam>
        /// <returns>True or False</returns>
        public bool? DeleteArticle(int contentId)
        {
            try
            {
                #region Get the article from the CMS database.
                ArticlePage article;
                var articleReference = new ContentReference(contentId);
                _contentRepository.TryGet(articleReference, out article);
                #endregion
                #region Delete it from the CMS database
                if (article != null)
                {
                    _contentRepository.Delete(articleReference, true, AccessLevel.NoAccess);
                    return true;
                }
                #endregion
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }
    }
}
