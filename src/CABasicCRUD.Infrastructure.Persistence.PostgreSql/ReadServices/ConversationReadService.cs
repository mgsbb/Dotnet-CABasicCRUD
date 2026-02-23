using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

public sealed class ConversationReadService(ApplicationDbContext dbContext)
    : IConversationReadService
{
    private readonly DbSet<Conversation> _dbSet = dbContext.Set<Conversation>();

    public async Task<Conversation?> GetByIdAsync(ConversationId conversationId)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(conversation => conversation.Id == conversationId);
    }

    public async Task<IReadOnlyList<Conversation>> GetConversationsOfUser(UserId userId)
    {
        // how to return conversations without loading the messages?
        return await _dbSet
            .AsNoTracking()
            .Where(conversation => conversation.Participants.Any(cp => cp.UserId == userId))
            .ToListAsync();
    }

    public async Task<Conversation?> GetPrivateConversationAsync(
        UserId initiatorUserId,
        UserId participantUserId,
        CancellationToken cancellationToken
    )
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.ConversationType == ConversationType.Private)
            .Where(c => c.Participants.Count == 2)
            .Where(c => c.Participants.Any(p => p.UserId == initiatorUserId))
            .Where(c => c.Participants.Any(p => p.UserId == participantUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
