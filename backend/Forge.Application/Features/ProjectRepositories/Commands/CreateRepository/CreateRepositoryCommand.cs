using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.ProjectRepositories.Commands.CreateRepository;

public sealed record CreateRepositoryCommand(
    Guid UserId,
    Guid ProjectId,
    CreateRepositoryRequest Request) : IRequest<RepositoryResponse>;
