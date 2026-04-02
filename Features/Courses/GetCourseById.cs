using MediatR;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;

namespace breakincycleapi.Features.Courses;

// 1. The Query (What the feature needs as input)
public record GetCourseByIdQuery(Guid CourseId) : IRequest<CourseResponse?>;

// 2. The Output (What the feature returns)
// It's good practice to not return your DB entity directly, but map it to a response record
public record CourseResponse(Guid CourseId, string Name, string? Description, DateTime CreatedAt);

// 3. The Handler (The business logic / data access)
public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, CourseResponse?>
{
    private readonly AppDbContext _context;

    public GetCourseByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponse?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .AsNoTracking() // Good for read-only queries
            .FirstOrDefaultAsync(c => c.CourseId == request.CourseId, cancellationToken);

        if (course == null)
            return null;

        return new CourseResponse(
            course.CourseId,
            course.Name,
            course.Description,
            course.Createdat);
    }
}

// 4. The Endpoint (Minimal API)
public static class GetCourseByIdEndpoint
{
    public static void MapGetCourseByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/courses/{courseId:guid}", async (Guid courseId, IMediator mediator) =>
        {
            var query = new GetCourseByIdQuery(courseId);
            var result = await mediator.Send(query);

            if (result == null)
            {
                return Results.NotFound($"Course with ID {courseId} not found.");
            }

            return Results.Ok(result);
        })
        .WithName("GetCourseById")
        .WithTags("Courses");
    }
}
