using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using login.Model;
using Amazon.SecurityToken.Model;

namespace login.Services;
public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<User> _usersCollection;
    public UserService(IOptions<DatabaseSettings> dataBaseSetting, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(
            dataBaseSetting.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dataBaseSetting.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            dataBaseSetting.Value.CollectionName);

        _configuration = configuration;
    }

    public async Task<List<User>> GetListAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User> GetAsync(long Id) =>
        await _usersCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

    public async Task CreateAsync(User NewUser) =>
        await _usersCollection.InsertOneAsync(NewUser);

    public async Task UpdateAsync(long Id, User NewUser) =>
        await _usersCollection.ReplaceOneAsync(x => x.Id == Id, NewUser);

    public async Task RemoveAsync(long Id) =>
        await _usersCollection.DeleteOneAsync(x => x.Id == Id);

    public string GenerateToken(string email, string password)
    {
        var handler = new JwtSecurityTokenHandler();

        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(password);

        var user = GetByEmail(email);

        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
        var credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        var claims = GenerateClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credential,
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = claims,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public User GetByEmail(string Email)
    {
        var user = _usersCollection.Find(x => x.Email == Email);

        return (User)user;
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();

        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        claims.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

        return claims;
    }
}