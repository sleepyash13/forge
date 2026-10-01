using System.Text.RegularExpressions;
using Forge.Application.DTOs.Projects.Repository;

namespace Forge.Application.Validators
{
    public static class ProjectRepositoryConfigurationValidator
    {
        public static void Validate(CreateRepositoryRequest request)
        {
            ValidateCommon(request.RepositoryUrl, request.DefaultBranch,
                request.BuildCommand, request.DeployConfiguration);
        }

        public static void Validate(UpdateRepositoryRequest request)
        {
            ValidateCommon(request.RepositoryUrl, request.DefaultBranch,
                request.BuildCommand, request.DeployConfiguration);
        }

        private static void ValidateCommon(string? repositoryUrl, string? defaultBranch,
            string? buildCommand, string? deployConfiguration)
        {
            if (string.IsNullOrWhiteSpace(repositoryUrl))
                throw new ArgumentException("Repository URL is required.");

            if (!Uri.TryCreate(repositoryUrl.Trim(), UriKind.Absolute, out var uri))
                throw new ArgumentException("Repository URL must be a valid URL.");

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException("Repository URL must use HTTP or HTTPS.");

            if (repositoryUrl.Trim().Length > 500)
                throw new ArgumentException("Repository URL cannot exceed 500 characters.");

            if (string.IsNullOrWhiteSpace(defaultBranch))
                throw new ArgumentException("Default branch is required.");

            if (defaultBranch.Trim().Length > 200)
                throw new ArgumentException("Default branch cannot exceed 200 characters.");

            if (!Regex.IsMatch(defaultBranch.Trim(), @"^[A-Za-z0-9._/\-]+$"))
                throw new ArgumentException("Default branch contains invalid characters.");

            if (buildCommand?.Length > 1000)
                throw new ArgumentException("Build command cannot exceed 1000 characters.");

            if (deployConfiguration?.Length > 10000)
                throw new ArgumentException("Deploy configuration cannot exceed 10000 characters.");
        }
    }
}