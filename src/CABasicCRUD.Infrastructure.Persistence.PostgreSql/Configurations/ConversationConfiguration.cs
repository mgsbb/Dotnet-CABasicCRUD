using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Messages;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversations");

        builder.HasKey(conversation => conversation.Id);
        builder
            .Property(conversation => conversation.Id)
            .HasConversion(conversation => conversation.Value, value => (ConversationId)value);

        builder.Property(conversation => conversation.CreatedAt).IsRequired();

        builder.Property(conversation => conversation.UpdatedAt).IsRequired(false);

        // Note: No foreign key to User, as User belongs to a different bounded context
        builder.Property(conversation => conversation.CreatedById).IsRequired();
        builder
            .Property(conversation => conversation.CreatedById)
            .HasConversion(userId => userId.Value, value => (UserId)value);

        // builder.Ignore(conversation => conversation.Participants);
        // builder.Ignore(conversation => conversation.Messages);

        builder
            .Property(conversation => conversation.ConversationType)
            .IsRequired()
            .HasConversion<string>();

        builder.Metadata.FindNavigation(nameof(Conversation.Messages))!.SetField("_messages");
        builder
            .Metadata.FindNavigation(nameof(Conversation.Participants))!
            .SetField("_participants");

        ConfigureParticipants(builder);

        ConfigureMessages(builder);
    }

    private static void ConfigureParticipants(EntityTypeBuilder<Conversation> builder)
    {
        builder.OwnsMany<ConversationParticipant>(
            conversation => conversation.Participants,
            cpBuilder =>
            {
                cpBuilder.ToTable("ConversationParticipants");

                cpBuilder.WithOwner();

                cpBuilder.HasKey(cp => new { cp.ConversationId, cp.UserId });

                cpBuilder
                    .Property(cp => cp.ConversationId)
                    .HasConversion(
                        ConversationId => ConversationId.Value,
                        value => (ConversationId)value
                    );

                cpBuilder
                    .Property(cp => cp.UserId)
                    .HasConversion(UserId => UserId.Value, value => (UserId)value);
            }
        );
    }

    private static void ConfigureMessages(EntityTypeBuilder<Conversation> builder)
    {
        builder.OwnsMany<Message>(
            conversation => conversation.Messages,
            messageBuilder =>
            {
                messageBuilder.ToTable("Messages");

                // Foreign key, due to same bounded context
                messageBuilder.WithOwner();

                messageBuilder.HasKey(message => message.Id);
                messageBuilder
                    .Property(message => message.Id)
                    .HasConversion(messageId => messageId.Value, value => (MessageId)value);

                messageBuilder.Property(message => message.CreatedAt).IsRequired();

                messageBuilder.Property(message => message.UpdatedAt).IsRequired(false);

                messageBuilder.Property(message => message.Content).IsRequired();

                // No foreign key
                messageBuilder.Property(message => message.SenderUserId).IsRequired();
                messageBuilder
                    .Property(message => message.SenderUserId)
                    .HasConversion(userId => userId.Value, value => (UserId)value);
            }
        );
    }
}
