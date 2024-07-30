using AutoMapper;
using Mango.MessageService;
using Mangoes.Services.OrderAPI;
using Mangoes.Services.OrderAPI.Data;
using Mangoes.Services.OrderAPI.Service.IService;
using Mangoes.Services.OrderAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDBContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDB"));
        });
        IMapper mapper = MappingConfig.Register().CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<BackendApiDelegatingHandler>();
        builder.Services.AddScoped<IProductServicecs,ProductServices>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddHttpClient("Products", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPIUrl"])).AddHttpMessageHandler<BackendApiDelegatingHandler>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: Bearer Generated-JWT-Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },new string[]{}
        }
            });
        });

        var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
        var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
        var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
        var key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true
                };
            });
        builder.Services.AddAuthorization();

        Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secret_Key").Get<string>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        ApplyMigration();

        app.Run();

        void ApplyMigration()
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}