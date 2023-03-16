using System.Text.Json.Serialization;
using Cryptocop.Software.API.Middlewares;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Implementations;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Implementations;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//REGISTER SERVICES
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
//builder.Services.AddTransient<IQueueService, QueueService>();
//builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
var jwtConfig = builder.Configuration.GetSection("jwtConfig");
builder.Services.AddTransient<ITokenService>((c) => new TokenService(
    jwtConfig.GetValue<string>("secret"),
    jwtConfig.GetValue<string>("expirationInMinutes"),
    jwtConfig.GetValue<string>("issuer"),
    jwtConfig.GetValue<string>("audience")
));

//REGISTER REPOSITORIES
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// SETTING JWT AS THE DEFAULT AUTHORIZATION SCHEME AND ADDING THE CUSTOM MIDDLEWARE
builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtTokenAuthentication(builder.Configuration);

// FOR POSTGRES DATABASE
builder.Services.AddDbContext<CryptocopDbContext>(options =>
    options.UseNpgsql(
    builder.Configuration.GetConnectionString("CryptocopConnectionString"),
    b => b.MigrationsAssembly("Cryptocop.Software.API")
        ));

//Typed injection FOR EXCHANGE API
builder.Services.AddHttpClient<IExchangeService, ExchangeService>(client => 
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeApiBaseUrl"));
});

//Typed injection FOR CRYPTOCURRENCY API
builder.Services.AddHttpClient<ICryptoCurrencyService, CryptoCurrencyService>(client => 
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CryptoApiBaseUrl"));
});

//Typed injection FOR CRYPTOCURRENCY API Single Item
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>(client => 
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CryptoSingleApiBaseUrl"));
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cryptocop API",
        Description = "An ASP.NET Core Web API for managing cryptcurrency shop",
        Contact = new OpenApiContact
        {
            Name = "Elfar Snær Arnarson",
            Url = new Uri("https://github.com/PanamaP/CryptoCop")
        },
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type= SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

     options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else if (app.Environment.IsProduction()) 
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cryptocop v1")); 
}



app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();