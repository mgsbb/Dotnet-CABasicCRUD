using Microsoft.Extensions.Options;

namespace CABasicCRUD.Infrastructure.Storage;

public sealed class CloudinaryOptionsValidator : IValidateOptions<CloudinaryOptions>
{
    public ValidateOptionsResult Validate(string? name, CloudinaryOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            return ValidateOptionsResult.Fail("Cloudinary Api Key is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiSecret))
        {
            return ValidateOptionsResult.Fail("Cloudinary Api Secret is required.");
        }

        if (string.IsNullOrWhiteSpace(options.CloudName))
        {
            return ValidateOptionsResult.Fail("Cloudinary Cloudname is required.");
        }

        return ValidateOptionsResult.Success;
    }
}
