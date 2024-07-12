using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cms12.Models.Pages
{
    [ContentType(DisplayName ="Article Page", Description ="Use this page to create an Article", GUID = "7cfb3ba2-9f2f-4bc0-a543-687ec0602b3f")]
    public class ArticlePage : PageData
    {
        [Required]
        [StringLength(90)]
        [Display(Order = 1, GroupName = SystemTabNames.Content)]
        public virtual string Title { get; set; }

        [Required]
        [Display(Order = 2, GroupName = SystemTabNames.Content, Name = "Publish Date")]
        public virtual DateTime PublishDate { get; set; }

        [UIHint(UIHint.Textarea)]
        [StringLength(300)]
        [Display(Order = 3, GroupName = SystemTabNames.Content)]
        public virtual string Subject { get; set; }

        [UIHint(UIHint.Image)]
        [AllowedTypes(typeof(ImageData))]
        [Display(Order = 4, GroupName = SystemTabNames.Content, Name ="Article Image")]
        public virtual ContentReference ArticleImage { get; set; }

        [Required]
        [Display(Order = 5, GroupName = SystemTabNames.Content, Name = "Article Body")]
        public virtual XhtmlString ArticleBody { get; set; }

        [Display(Order = 6, GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

    }
}
