namespace StreamNote.GraphQL.Services.Implementations
{
    public interface IFirebaseAdminService
    {
        Task<bool> UpsertFCMTokenAsync(string userId, string fcmToken);
        Task SendPushNotificationAsync(string userId, string title, string body);
    }
}
