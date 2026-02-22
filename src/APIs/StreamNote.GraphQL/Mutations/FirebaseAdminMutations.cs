namespace StreamNote.GraphQL.Mutations
{
    [MutationType]
    public class FirebaseAdminMutations
    {
        public async Task<bool> UpsertFCMToken(
            [Service] IFirebaseAdminService firebaseAdminService,
            string userId,
            string fcmToken )
        {
            return await firebaseAdminService.UpsertFCMTokenAsync(userId, fcmToken);
        }
    }
}
