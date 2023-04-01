using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace IdentityServerPoC.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public int Credits { get; set; }

    public HashSet<Guid> MessageIds { get; set; } = new(); // NOTE: Idempotency in consumers
}
