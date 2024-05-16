using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using System.Globalization;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
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

app.UseCors("AllowAllOrigins");

//PARA TESTE DE FUNCIONAMENTO DA API
app.MapGet("/helloworld", () => "Hello World!");

app.MapGet("/alunos", async (AppDb db) =>
{
    try
    {
        var alunos = await db.Alunos.ToListAsync();
        return alunos != null ? Results.Ok(alunos) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao buscar os alunos. Por favor, tente novamente mais tarde.");
    }
});

app.MapGet("/alunos/{id}", async (int id, AppDb db) =>
{
    try
    {
        var aluno = await db.Alunos.FindAsync(id);
        return aluno != null ? Results.Ok(aluno) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao buscar o item. A API pode estar indisponível no momento. Por favor, tente novamente mais tarde.");
    }
});

app.MapPost("/alunos", async (AppDb db, Aluno aluno) =>
{
    try
    {   
        // Vem do front como string
        var dataFormatada = aluno.GetFormattedNascimento();

        db.Alunos.Add(aluno);
        await db.SaveChangesAsync();

        return Results.Created($"/alunos/{aluno.iCodAluno}", aluno);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao adicionar o aluno. Por favor, tente novamente mais tarde.");
    }
});

app.MapPut("/alunos/{id}", async (int id, Aluno inputAluno, AppDb db) =>
{
    try
    {
        var aluno = await db.Alunos.FindAsync(id);
        if (aluno is null) return Results.NotFound();

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
        return Results.Problem("Ocorreu um erro ao alterar os dados do aluno. Por favor, tente novamente mais tarde.");
    }
});

app.MapDelete("/alunos/{id}", async (int id, AppDb db) =>
{
    try
    {
        var aluno = await db.Alunos.FindAsync(id);
        if (aluno is null) return Results.NotFound();

        db.Alunos.Remove(aluno);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao deletar o aluno. Por favor, tente novamente mais tarde.");
    }
});

app.MapGet("/escola", async (AppDb db) =>
{
    try
    {
        var escolas = await db.Escola.ToListAsync();
        return escolas != null ? Results.Ok(escolas) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao buscar os alunos. Por favor, tente novamente mais tarde.");
    }
});

app.MapGet("/escola/{id}", async (int id, AppDb db) =>
{
    try
    {
        var escola = await db.Escola.FindAsync(id);
        return escola != null ? Results.Ok(escola) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro a escola. A API pode estar indisponível no momento. Por favor, tente novamente mais tarde.");
    }
});

app.MapPost("/escola", async (AppDb db, Escola escola) =>
{
    try
    {
        db.Escola.Add(escola);
        await db.SaveChangesAsync();

        return Results.Created($"/alunos/{escola.iCodEscola}", escola);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        // Retornar erro 500 + mensagem
        return Results.Problem("Ocorreu um erro ao adicionar uma escola. Por favor, tente novamente mais tarde.");
    }
});

app.MapPut("/escola/{id}", async (int id, Escola inputEscola, AppDb db) =>
{
    try
    {
        var escola = await db.Escola.FindAsync(id);
        if (escola is null) return Results.NotFound();

        escola.iCodEscola = inputEscola.iCodEscola;
        escola.sDescricao = inputEscola.sDescricao;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao alterar os dados do aluno. Por favor, tente novamente mais tarde.");
    }
});

app.MapDelete("/escola/{id}", async (int id, AppDb db) =>
{
    try
    {
        var escola = await db.Escola.FindAsync(id);
        if (escola is null) return Results.NotFound();

        db.Escola.Remove(escola);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Ocorreu um erro ao deletar a escola. Por favor, tente novamente mais tarde.");
    }
});

app.Run();
