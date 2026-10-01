using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.Projects.Queries.GetMyProjects;

public sealed record GetMyProjectsQuery(Guid UserId) : IRequest<List<ProjectResponse>>;
