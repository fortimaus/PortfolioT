using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class UserCommentBindingModel : IUserComment
    {
        public string text { get; set; } = string.Empty;
        public long moderatorId { get; set; }
        public long userId { get; set; }
        public DateTime date { get; set; }

        public long Id { get; set; }
    }
}
