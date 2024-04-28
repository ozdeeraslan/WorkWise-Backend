using ApplicationCore.DTOs.UserDto;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.ValidationRules.CompanyManagerValidations;
using ApplicationCore.ValidationRules.CompanyValidations;
using ApplicationCore.ValidationRules.ExpenseValidations;
using ApplicationCore.ValidationRules.PasswordValidations;
using ApplicationCore.ValidationRules.SiteOwnerValidations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(s => s.AddDefaultPolicy(
    p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

// Context
//var cs = builder.Configuration.GetConnectionString("BaglantiCumlem");
//builder.Services.AddDbContext<AppIdentityDbContext>
//    (optionsBuilder => optionsBuilder.UseSqlServer(cs));

var cs = builder.Configuration.GetConnectionString("WorkWiseDbContext");
builder.Services.AddDbContext<WorkWiseContext>(optionsBuilder => optionsBuilder.UseSqlServer(cs));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WorkWiseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection
            ("JwtSettings:SecretKey").Value))
    };
});



//builder.Services.AddDbContext<WorkWiseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WorkWiseDbContext")));
//builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppIdentityDbContext")));

//builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IUserRepository<AppUser>, UserRepository>();
builder.Services.AddScoped<IRepository<Company>, CompanyRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAdvanceService, AdvanceService>();
builder.Services.AddScoped<ILeaveValidateService, LeaveValidateService>();
builder.Services.AddScoped<IAnnualLeaveService, AnnualLeaveService>();



builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<AddCompanyValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<AddCompanyManagerValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<PasswordValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<AddExpenseValidation>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



using (var scope = app.Services.CreateScope())
{
    var workWiseContext = scope.ServiceProvider.GetRequiredService<WorkWiseContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var companyRepo = scope.ServiceProvider.GetRequiredService<IRepository<Company>>();
    

    await WorkWiseContextSeed.SeedAsync(roleManager, userManager, companyRepo);
}

app.Run();
