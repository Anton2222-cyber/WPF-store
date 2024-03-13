using Data.Context;
using Data.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Mapper;
using WebApi.Models.Category;
using WebApi.Seeders;
using WebApi.Services;
using WebApi.Services.Interfaces;
using WebApi.Validators.Product;



var builder = WebApplication.CreateBuilder(args);

var assemblyName = AssemblyService.GetAssemblyName();

builder.Services.AddDbContext<DataContext>(
	options => {
		options.UseSqlite(
			builder.Configuration.GetConnectionString("Sqlite"),
			sqliteOptions => sqliteOptions.MigrationsAssembly(assemblyName)
		);

		if (builder.Environment.IsDevelopment()) {
			options.EnableSensitiveDataLogging();
		}
	}
);

builder.Services
	.AddIdentity<User, Role>(options => {
		options.Stores.MaxLengthForKeys = 128;

		options.Password.RequiredLength = 5;
		options.Password.RequireDigit = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	})
	.AddEntityFrameworkStores<DataContext>()
	.AddDefaultTokenProviders();

var singinKey = new SymmetricSecurityKey(
	Encoding.UTF8.GetBytes(
		builder.Configuration["Jwt:SecretKey"]
			?? throw new NullReferenceException("Jwt:SecretKey")
	)
);

builder.Services
	.AddAuthentication(options => {
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options => {
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters() {
			ValidateIssuer = false,
			ValidateAudience = false,
			IssuerSigningKey = singinKey,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	var fileDoc = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
	options.IncludeXmlComments(fileDoc);

	options.AddSecurityDefinition(
		"Bearer",
		new OpenApiSecurityScheme {
			Description = "Jwt Auth header using the Bearer scheme",
			Type = SecuritySchemeType.Http,
			Scheme = "bearer"
		}
	);
	options.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			},
			new List<string>()
		}
	});
});

builder.Services.AddAutoMapper(typeof(AppMapProfile));

builder.Services.AddTransient<IValidator<CategoryCreateViewModel>, CategoryCreateValidator>();
builder.Services.AddTransient<IValidator<CategoryUpdateViewModel>, CategoryUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryCreateValidator>();

builder.Services.AddTransient<ICategoryControllerHelper, CategoryControllerHelper>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IContentSeeder, ContentSeeder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!Directory.Exists(ImageWorker.ImagesDir)) {
	Directory.CreateDirectory(ImageWorker.ImagesDir);
}

app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(ImageWorker.ImagesDir),
	RequestPath = "/Data/images"
});

app.UseCors(configuration => configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Content-Disposition"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedNecessaryDataAsync();
await app.SeedPostOfficesAsync();
await app.SeedTestCategoriesAndProducts();

app.Run();
