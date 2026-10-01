using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.ProjectRepositories.Queries.GetRepository;

public sealed record GetRepositoryQuery(Guid UserId, Guid ProjectId) : IRequest<RepositoryResponse?>;
