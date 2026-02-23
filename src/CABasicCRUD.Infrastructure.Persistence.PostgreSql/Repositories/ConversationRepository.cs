using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class ConversationRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Conversation, ConversationId>(dbContext),
        IConversationRepository { }
