using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class ConversationRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Conversation, ConversationId>(dbContext),
        IConversationRepository
{
    public async Task<IReadOnlyList<Conversation>> GetConversationsOfUser(UserId userId)
    {
        // how to return conversations without loading the messages?
        var conversations = await _dbSet
            .AsNoTracking()
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .ToListAsync();

        return conversations;
    }
}
