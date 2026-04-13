using CABasicCRUD.Domain.MediaItems;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public sealed class MediaRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Media, MediaId>(dbContext),
        IMediaRepository;
