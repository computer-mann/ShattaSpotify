using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using StreamNote.Database.Commons.Database;
using StreamNote.GraphQL.Services.Interfaces;

namespace StreamNote.GraphQL.Services.Implementations
{
    public class FirebaseAdminService(
        AppDbContext dbContext,
        ILogger<FirebaseAdminService> logger
        ) : IFirebaseAdminService
    {

        public async Task SendPushNotificationAsync(string userId, string title, string body)
        {
            var userTokens =await dbContext.FcmUserTokens.Where(t => t.UserId == userId).ToListAsync();
            var messageList = new List<Message>();
            foreach (var userToken in userTokens)
            {
                messageList.Add(new Message
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Token = userToken.FcmToken,
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High,
                    },
                });
            }
            var batchResponse = await FirebaseMessaging.DefaultInstance.SendEachAsync(messageList);
            logger.LogInformation("Sent {SuccessCount} messages successfully, {FailureCount} messages failed.", batchResponse.SuccessCount, batchResponse.FailureCount);
        }

        public Task<bool> UpsertFCMTokenAsync(string userId, string fcmToken)
        {
            throw new NotImplementedException();
        }
    }
}
