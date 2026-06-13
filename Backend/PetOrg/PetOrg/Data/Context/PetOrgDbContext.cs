using Microsoft.EntityFrameworkCore;

namespace PetOrg.Data.Context;

public sealed class PetOrgDbContext(DbContextOptions<PetOrgDbContext> options) : DbContext(options);
