using LoginJwt.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoginJwt.Services;
public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;
    public UserService(IOptions<DatabaseSettings> dataBaseSetting)
    {
        var mongoClient = new MongoClient(
            dataBaseSetting.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dataBaseSetting.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            dataBaseSetting.Value.CollectionName);
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
}