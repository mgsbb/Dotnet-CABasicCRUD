using CABasicCRUD.Application.Features.Auth.RegisterUser;
using CABasicCRUD.Application.Features.Comments.CreateComment;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Seeding;

public sealed class ApplicationCommandSeeder(
    IMediator mediator,
    IOptions<DatabaseSeedOptions> options,
    ILogger<ApplicationCommandSeeder> logger
)
{
    private readonly IMediator _mediator = mediator;
    private readonly DatabaseSeedOptions _options = options.Value;
    private readonly ILogger<ApplicationCommandSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        if (!_options.IsSeedDatabase)
        {
            return;
        }

        _logger.LogInformation("Running database seeder.");

        var user1 = await _mediator.Send(
            new RegisterUserCommand("User1", "user1@email.com", _options.SeedUserPassword)
        );

        var user2 = await _mediator.Send(
            new RegisterUserCommand("User2", "user2@email.com", _options.SeedUserPassword)
        );

        var post1 = await _mediator.Send(
            new CreatePostCommand("New post tile 1", "New post content 1", user1.Value!.Id)
        );

        var comment1 = await _mediator.Send(
            new CreateCommentCommand("Comment body1", post1.Value!.Id, user2.Value!.Id)
        );
    }
}
