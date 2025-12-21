using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
