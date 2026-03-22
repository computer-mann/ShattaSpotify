using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using StreamNote.Database.Commons.Database;
using StreamNote.Database.Commons.Database.Entities;
using StreamNote.GraphQL.Services.Interfaces;

namespace StreamNote.GraphQL.Services.Implementations
{
    public class FirebaseAdminService(
        AppDbContext dbContext,
        ILogger<FirebaseAdminService> logger
        ) : IFirebaseAdminService
    {
        public async Task SendPushNotificationToUserAsync(string userId, string title, string body)
        {
            var userTokens = await dbContext.FcmUserTokens.Where(t => t.UserId == userId).ToListAsync();
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

        public async Task SendPushNotificationToTopicAsync(string topicId, string title, string body)
        {
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Topic = topicId,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            logger.LogInformation("Sent message to topic {TopicId} successfully. Message ID: {MessageId}", topicId, response);
        }

        public async Task<bool> UpsertFCMTokenAsync(string userId, string fcmToken)
        {
            try
            {
                var existingToken = await dbContext.FcmUserTokens
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.FcmToken == fcmToken);

                if (existingToken == null)
                {
                    var newToken = new FcmUserTokens
                    {
                        UserId = userId,
                        FcmToken = fcmToken,
                        Timestamp = DateTime.UtcNow
                    };
                    dbContext.FcmUserTokens.Add(newToken);
                }
                else
                {
                    existingToken.Timestamp = DateTime.UtcNow;
                    dbContext.FcmUserTokens.Update(existingToken);
                }

                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error upserting FCM token for user {UserId}", userId);
                return false;
            }
        }
    }
}
