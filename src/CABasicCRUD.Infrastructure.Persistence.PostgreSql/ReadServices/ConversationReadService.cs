using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Conversations.Conversations.Queries;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

// ========================================================================================================================

public sealed class ConversationReadService(ApplicationDbContext dbContext)
    : IConversationReadService
{
    private readonly DbSet<Conversation> _dbSet = dbContext.Set<Conversation>();

    // ========================================================================================================================

    public async Task<Conversation?> GetByIdAsync(ConversationId conversationId)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(conversation => conversation.Id == conversationId);
    }

    // ========================================================================================================================

    public async Task<ConversationDetailsResult?> GetConversationByIdWithDetails(
        ConversationId conversationId
    )
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Id == conversationId)
            .Select(c => new ConversationDetailsResult(
                c.Id,
                c.ConversationType,
                c.Participants.Join(
                        dbContext.Users,
                        cp => cp.UserId,
                        u => u.Id,
                        (cp, u) =>
                            new ConversationParticipantDetail(
                                u.Id,
                                u.Username,
                                u.UserProfile.FullName
                            )
                    )
                    .ToList(),
                c.Messages.OrderByDescending(m => m.CreatedAt)
                    .Join(
                        dbContext.Users,
                        m => m.SenderUserId,
                        u => u.Id,
                        (m, u) =>
                            new MessageDetail(
                                m.Id,
                                m.Content,
                                u.Id,
                                u.Username,
                                u.UserProfile.FullName,
                                m.CreatedAt,
                                m.UpdatedAt
                            )
                    )
                    .ToList(),
                c.CreatedAt,
                c.UpdatedAt
            ))
            .FirstOrDefaultAsync();
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<Conversation>> GetConversationsOfUser(UserId userId)
    {
        // how to return conversations without loading the messages?
        return await _dbSet
            .AsNoTracking()
            .Where(conversation => conversation.Participants.Any(cp => cp.UserId == userId))
            .ToListAsync();
    }

    // ========================================================================================================================

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

// ========================================================================================================================
// ========================================================================================================================
