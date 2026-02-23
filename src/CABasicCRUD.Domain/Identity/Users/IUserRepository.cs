using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Identity.Users;

public interface IUserRepository : IRepository<User, UserId> { }
