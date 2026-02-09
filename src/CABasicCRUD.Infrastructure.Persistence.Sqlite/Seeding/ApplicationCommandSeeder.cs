using System.Diagnostics;
using Bogus;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Auth.RegisterUser;
using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Application.Features.Comments.CreateComment;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
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

        Randomizer.Seed = new Random(100);

        var userIds = new List<UserId>();
        var postIds = new List<PostId>();

        var faker = new Faker();

        var stopWatch = Stopwatch.StartNew();

        // 50 users
        for (var i = 0; i < 50; i++)
        {
            // RegisterUserCommand results in domain events being raised, and external systems such as email sender are effected
            Result<AuthResult> result = await _mediator.Send(
                new RegisterUserCommand(
                    faker.Name.FullName(),
                    faker.Internet.Email(),
                    _options.SeedUserPassword
                )
            );

            userIds.Add(result.Value!.Id);
        }

        // each user creates 10 posts, so 500 posts
        foreach (UserId userId in userIds)
        {
            for (var i = 0; i < 10; i++)
            {
                string postContent = string.Join(
                    " ",
                    Enumerable.Range(0, 10).Select(_ => faker.Hacker.Phrase())
                );

                Result<PostResult> result = await _mediator.Send(
                    new CreatePostCommand(faker.Commerce.ProductName(), postContent, userId)
                );

                postIds.Add(result.Value!.Id);
            }
        }

        // each post has 10 comments, so 5000 comments
        foreach (PostId postId in postIds)
        {
            for (var i = 0; i < 10; i++)
            {
                var userId = faker.PickRandom(userIds);

                Result<CommentResult> result = await _mediator.Send(
                    new CreateCommentCommand(faker.Hacker.Phrase(), postId, userId)
                );
            }
        }

        stopWatch.Stop();

        _logger.LogInformation("Seeding took {elapsedTime}", stopWatch.Elapsed);
    }
}
