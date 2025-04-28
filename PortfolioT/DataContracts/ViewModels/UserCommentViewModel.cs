using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class UserCommentViewModel : IUserComment
    {
        public string text { get; set; } = string.Empty;
        public long moderatorId { get; set; }
        public long userId { get; set; }
        public DateTime date { get; set; }

        public string moderatorName { get; set; } = string.Empty;
        public long Id { get; set; }
    }
}
