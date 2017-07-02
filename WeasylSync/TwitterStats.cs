using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace WeasylSync {
    public static class TwitterStats {
        public class UserStats {
            public string ScreenName { get; set; }
            public int TweetsRetrieved { get; set; }
            public int RetweetCount { get; set; }
        }

        public static IEnumerable<UserStats> GetFriendStats(ITwitterCredentials credentials, Action<int> setProgressMax = null, Action incrementProgress = null) {
            var me = Auth.ExecuteOperationWithCredentials(credentials, () => User.GetAuthenticatedUser());
            IEnumerable<IUser> followedByMe = Auth.ExecuteOperationWithCredentials(credentials, () => User.GetFriends(me, 250));
            setProgressMax?.Invoke(followedByMe.Count());
            List<UserStats> list = new List<UserStats>();
            foreach (var user in followedByMe) {
                var stats = GetFriendStats(credentials, user);
                incrementProgress?.Invoke();
                if (stats != null) list.Add(stats);
            }
            return list;
        }

        private static UserStats GetFriendStats(ITwitterCredentials credentials, IUser user) {
            Console.Write($"@{user.ScreenName}: ");
            DateTime twoWeeksAgo = DateTime.UtcNow.AddDays(-14);
            IEnumerable<ITweet> timeline = Timeline.GetUserTimeline(user, 100).Where(t => t.CreatedAt > twoWeeksAgo);
            //if (!timeline.Any()) return null;
            int count = timeline.Count();
            int rt_count = timeline.Where(t => t.IsRetweet).Count();
            Console.WriteLine($"last {count} tweets: {rt_count} retweets ({100.0 * rt_count / count}%)");
            return new UserStats {
                ScreenName = user.ScreenName,
                TweetsRetrieved = count,
                RetweetCount = rt_count
            };
        }
    }
}
