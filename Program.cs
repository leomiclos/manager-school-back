using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDb>(opt => opt.UseInMemoryDatabase("AppDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TesteTecnicoAPI";
    config.Title = "TesteTecnico v1";
    config.Version = "v1";
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Teste Tecnico API";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

//PARA TESTE DE FUNCIONAMENTO DA API
app.MapGet("/helloworld", () => "Hello World!");


app.MapGet("/alunos", async (AppDb db) =>
{
    var alunos = await db.Alunos.ToListAsync();
    try
    {
        if (alunos.Count == 0)
        {
            return Results.Ok("Não há alunos na lista.");
        }
        return Results.Ok(alunos);
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message);
        // Retornar erro 500 + mensagem
        return Results.Problem("Ocorreu um erro ao buscar os alunos. Por favor, tente novamente mais tarde.");
    }

});


// app.MapGet("/alunos/{id}", async (int id, AppDb db) =>
   

app.MapGet("/alunos/{id}", async (int id, AppDb db) =>
    await db.Alunos.FindAsync(id)
        is Aluno aluno
            ? Results.Ok(aluno)
            : Results.NotFound());


app.MapPost("/alunos", async (AppDb db, Aluno aluno) =>
{
    try
    {
        db.Alunos.Add(aluno);
        await db.SaveChangesAsync();

        return Results.Created($"/alunos/{aluno.iCodAluno}", aluno);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        // Retornar erro 500 + mensagem
        return Results.Problem("Ocorreu um erro ao adicionar o aluno. Por favor, tente novamente mais tarde.");
    }
});

app.MapPut("/alunos/{id}", async (int id, Aluno inputAluno, AppDb db) => 
{

    try
    {
        var aluno = await db.Alunos.FindAsync(id);
        if(aluno is null) return Results.NotFound();

        aluno.sNome = inputAluno.sNome;
        aluno.sCelular = inputAluno.sCelular;
        aluno.sEndereco = inputAluno.sEndereco;
        aluno.iCodEscola = inputAluno.iCodEscola;

        await db.SaveChangesAsync();

        return Results.NoContent();
 


    }
    catch (Exception ex)
    {
        
        Console.WriteLine(ex.Message);
        // Retornar erro 500 + mensagem
        return Results.Problem("Ocorreu um erro ao alterar os dados do aluno. Por favor, tente novamente mais tarde.");    }
});





app.Run();
