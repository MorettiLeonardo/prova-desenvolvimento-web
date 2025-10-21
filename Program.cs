using Microsoft.AspNetCore.Mvc;
using GuilhermeGabriel.Requests;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapPost("/api/consumo/cadastrar", async (AppDbContext context, [FromBody] CadastrarRequest request) =>
{
    if (request.Mes < 1 || request.Mes > 12)
    {
        return Results.BadRequest("Insira um mes valido");
    }

    if (request.Ano <= 2000)
    {
        return Results.BadRequest("Insira um ano maior que 2000");
    }

    if (request.M3Consumidos < 1)
    {
        return Results.BadRequest("Insira um valor maior que 0 em Metros cubicos");
    }

    double tarifa = 0;
    if (request.M3Consumidos < 11)
    {
        tarifa = 2.50 * request.M3Consumidos;
    }
    else if (request.M3Consumidos < 21)
    {
        tarifa = 3.50 * request.M3Consumidos;
    }
    else if (request.M3Consumidos < 51)
    {
        tarifa = 5 * request.M3Consumidos;
    }
    else
    {
        tarifa = 6.50 * request.M3Consumidos;
    }

    double consumoFaturado = request.M3Consumidos < 10 ? 10 : request.M3Consumidos;

    double adicionalBandeira = 0;
    if (request.Bandeira == "Amarela")
    {
        adicionalBandeira = consumoFaturado + (consumoFaturado * 0.1);
    }
    else if (request.Bandeira == "Vermelha")
    {
        adicionalBandeira = consumoFaturado + (consumoFaturado * 0.2);
    }

    double taxaDeEsgoto = 0;
    double valorAgua = consumoFaturado * tarifa;
    if (request.possuiEsgoto)
    {
        taxaDeEsgoto = (valorAgua + adicionalBandeira) * 0.80;
    }

    double totalGeral = valorAgua + adicionalBandeira + taxaDeEsgoto;

    var consumo = new Consumo
    {
        Cpf = request.Cpf,
        Mes = request.Mes,
        Ano = request.Ano,
        M3Consumidos = request.M3Consumidos,
        Bandeira = request.Bandeira,
        PossuiEsgoto = request.possuiEsgoto,
        Tarifa = tarifa,
        AdicionalBandeira = adicionalBandeira,
        TaxaDeEsgoto = taxaDeEsgoto,
        TotalGeral = totalGeral
    };

    context.Add(consumo);
    await context.SaveChangesAsync();

    return Results.Ok("Consumo registrado");
});


app.MapGet("/api/consumo/listar", (AppDbContext context) =>
{
    var consumos = context.Consumos.ToList();

    if (consumos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(consumos);
});

app.MapGet("/api/consumo/buscar/{cpf}/{mes}/{ano}", async (string cpf, int mes, int ano, AppDbContext context) =>
{
    var consumo = context.Consumos
        .Where(c => c.Cpf == cpf)
        .Where(c => c.Mes == mes)
        .Where(c => c.Ano == ano);

    if (consumo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(consumo);
});

app.MapDelete("/api/consumo/remover/{cpf}/{mes}/{ano}", async (string cpf, int mes, int ano, AppDbContext context) =>
{
    var consumo = context.Consumos
        .Where(c => c.Cpf == cpf)
        .Where(c => c.Mes == mes)
        .Where(c => c.Ano == ano);

    if (consumo == null)
    {
        return Results.NotFound();
    }

    context.Remove(consumo);
    await context.SaveChangesAsync();   

    return Results.Ok(consumo);
});

app.Run();
