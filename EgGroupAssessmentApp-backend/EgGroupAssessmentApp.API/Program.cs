using System.Text;
using EgGroupAssessmentApp.Core.Interfaces;
using EgGroupAssessmentApp.Infrastructure.DataContext;
using EgGroupAssessmentApp.Infrastructure.Repositories;
using EgGroupAssessmentApp.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrEmpty(connectionString)) {
        options.UseSqlServer(connectionString, sqlOptions => {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    }
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "Development_Secret_Key_At_Least_32_Chars_Long";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AssessmentApp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AssessmentAppUsers";

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Assessment App API",
        Version = "v1",
        Description = "Full-Stack Web Application Assessment - .NET 8 Web API",
        Contact = new OpenApiContact {
            Name = "Assessment App",
            Email = "contact@assessmentapp.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API V1");
    });
}

app.UseExceptionHandler(errorApp => {
    errorApp.Run(async context => {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (exceptionFeature != null) {
            logger.LogError(exceptionFeature.Error, "Unhandled exception occurred");
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new {
            StatusCode = 500,
            Message = "An unexpected error occurred",
            Errors = new[] { "Please contact support if this problem persists" }
        });
    });
});

app.UseStaticFiles();

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
