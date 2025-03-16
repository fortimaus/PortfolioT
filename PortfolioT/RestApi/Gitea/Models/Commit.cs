using PortfolioT.RestApi.Gitea.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class Commit
    {
        public string sha { get; set; } = string.Empty;
        public CommitInfo? commit { get; set; } = new CommitInfo();

        public Author author { get; set; }
        public string created { get; set; } = string.Empty;

        public List<CommitFile> files { get; set; } = new List<CommitFile>();

        public Stats stats { get; set; } = new Stats();

        public override string ToString()
        {
            return $"Author: {commit.author.name} Date: {created}" +
                $"files:[{String.Join(" ", files)}] Stats: +{stats.additions} -{stats.deletions}";
        }
        public string GetAuthorAndMessage()
        {
            return $"Author: {commit.author.name} Message: {commit.message}";
        }

        public string GetAuthor()
        {
            if (author == null)
                return "Author: none";
            else
                return $"Author: {author.id}-{author.login}";
        }
    }
}
