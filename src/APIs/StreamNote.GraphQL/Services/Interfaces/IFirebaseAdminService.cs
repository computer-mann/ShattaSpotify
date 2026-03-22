namespace StreamNote.GraphQL.Services.Interfaces
{
    public interface IFirebaseAdminService
    {
        Task<bool> UpsertFCMTokenAsync(string userId, string fcmToken);
        Task SendPushNotificationToUserAsync(string userId, string title, string body);
        Task SendPushNotificationToTopicAsync(string topicId, string title, string body);
    }
}
