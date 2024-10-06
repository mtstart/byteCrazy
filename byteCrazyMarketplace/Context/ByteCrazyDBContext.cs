using byteCrazy.Models;
using System.Data.Entity;

public class ByteCrazy : DbContext
{
    public DbSet<Users> Users { get; set; }
}