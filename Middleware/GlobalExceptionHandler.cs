using System.Net;
using System.Text.Json;
using MentalHealth.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MentalHealth.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Status = error switch
                    {
                        DuplicateNameException => StatusCodes.Status422UnprocessableEntity,
                        ForeignKeyViolationException => StatusCodes.Status422UnprocessableEntity,
                        DbUpdateException => StatusCodes.Status422UnprocessableEntity,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    Title = error switch
                    {
                        DuplicateNameException => "Validation Error",
                        ForeignKeyViolationException => "Validation Error",
                        DbUpdateException => "Database Constraint Violation",
                        _ => "Internal Server Error"
                    },
                    Detail = error switch
                    {
                        DbUpdateException dbEx => HandleDbUpdateException(dbEx),
                        _ => error.Message
                    }
                };

                response.StatusCode = errorResponse.Status;
                var result = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
        }

        private string HandleDbUpdateException(DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                return pgEx.SqlState switch
                {
                    "23503" => $"Referenced record does not exist. Constraint: {pgEx.ConstraintName}",
                    "23505" => $"Duplicate value violation. Constraint: {pgEx.ConstraintName}",
                    _ => "A database error occurred."
                };
            }
            
            return "A database error occurred.";
        }
    }

    public class ErrorResponse
    {
        public ErrorResponse()
        {
            Title = string.Empty;
            Detail = string.Empty;
        }

        public int Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
} 