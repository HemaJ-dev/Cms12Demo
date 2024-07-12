using System.ComponentModel.DataAnnotations;

namespace Cms12.WebServices.Models
{
    public class ArticleModel
    {
        public int ArticleId { get; set; }

        [Required]
        [StringLength(90)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Subject { get; set; }

        [Required]
        public string PublishDate { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string ArticleBody { get; set; }

        public string Author { get; set; }

    }
}
