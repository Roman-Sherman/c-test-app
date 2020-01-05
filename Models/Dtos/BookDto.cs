using System.ComponentModel.DataAnnotations;

namespace aspdotnet_managesys.Models.Dtos
{
    public class RegBook
    {
        [MaxLength(50, ErrorMessage = "書籍タイトルは、{1}文字以内で入力してください。")]
        [MinLength(3, ErrorMessage = "書籍タイトルは、{1}文字以上で入力してください。")]
        public string Title { get; set; }
        [MaxLength(20, ErrorMessage = "ISBNは、{1}文字以内で入力してください。")]
        [MinLength(10, ErrorMessage = "ISBNは、{1}文字以上で入力してください。")]
        public string Isbn { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int FormatId { get; set; }

        public Book create()
        {
            Category c = new Category { Id = CategoryId, Name = "" };
            Format f = new Format { Id = FormatId, Name = "" };
            return new Book
            {
                Title = this.Title,
                Isbn = this.Isbn,
                Category = c,
                Format = f
            };
        }
    }

    public class ChgBook
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "書籍タイトルは、{1}文字以内で入力してください。")]
        [MinLength(3, ErrorMessage = "書籍タイトルは、{1}文字以上で入力してください。")]
        public string Title { get; set; }
        [MaxLength(20, ErrorMessage = "ISBNは、{1}文字以内で入力してください。")]
        [MinLength(10, ErrorMessage = "ISBNは、{1}文字以上で入力してください。")]
        public string Isbn { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int FormatId { get; set; }
    }
}