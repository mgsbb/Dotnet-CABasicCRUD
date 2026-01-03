using Microsoft.Extensions.Options;

namespace CABasicCRUD.Infrastructure.Authentication;

internal sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            return ValidateOptionsResult.Fail("Jwt Secret Key is required.");
        }

        if (options.SecretKey.Length < 32)
        {
            return ValidateOptionsResult.Fail("Jwt Secret Key must be atleast 32 characters");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail("Jwt Issuer is required.");
        }
        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail("Jwt Audience is required.");
        }
        if (options.ExpiryInMinutes <= 0)
        {
            return ValidateOptionsResult.Fail("Jwt ExpiryInMinutes must be greater than 0.");
        }

        return ValidateOptionsResult.Success;
    }
}
