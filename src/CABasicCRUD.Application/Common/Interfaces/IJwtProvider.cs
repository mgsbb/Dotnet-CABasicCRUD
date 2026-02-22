using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
