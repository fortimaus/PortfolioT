using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class UserComment : IUserComment
    {
        public long Id { get; set; }
        [Required]
        public string text { get; set; } = string.Empty;

        [ForeignKey("moderatorId")]
        public long moderatorId { get; set; }

        [ForeignKey("userId")]
        public long userId { get; set; }

        public DateTime date { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        public virtual User moderator { get; set; } = new();

        public virtual User user { get; set; } = new();

        public UserCommentViewModel GetUserCommentViewModel()
        {
            return new UserCommentViewModel()
            {
                Id = Id,
                text = text,
                moderatorId = moderatorId,
                userId = userId,
                date = date,
                moderatorName = moderator.login
            };
        }
    }
}
