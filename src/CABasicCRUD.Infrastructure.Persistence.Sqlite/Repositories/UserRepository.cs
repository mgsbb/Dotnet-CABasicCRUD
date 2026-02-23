using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class UserRepository(ApplicationDbContext dbContext)
    : RepositoryBase<User, UserId>(dbContext),
        IUserRepository { }
