using Microsoft.EntityFrameworkCore;

class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options)
        : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Escola> Escola => Set<Escola>();

}