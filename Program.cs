using Microsoft.AspNetCore.Mvc;
using GuilhermeGabriel.Requests;
using Microsoft.EntityFrameworkCore;

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

    var consumoExistente = await context.Consumos
        .Where(c => c.Cpf == request.Cpf && c.Mes == request.Mes && c.Ano == request.Ano)
        .FirstOrDefaultAsync(); 

    if (consumoExistente != null)
    {
        return Results.BadRequest("Já existe um consumo registrado para este CPF, mês e ano.");
    }

    double consumoFaturado = 0.0;

    if (request.M3Consumidos < 10)
    {
        consumoFaturado = 10;
    }
    else
    {
        consumoFaturado = request.M3Consumidos;
    }

    double tarifa = 0.0;
    if (request.M3Consumidos < 11)
    {
        tarifa =  request.M3Consumidos /2.50;
    }
    else if (request.M3Consumidos < 21)
    {
        tarifa =  request.M3Consumidos / 3.50 ;
    }
    else if (request.M3Consumidos < 51)
    {
        tarifa = request.M3Consumidos / 5 ;
    } else
    {
        tarifa =  request.M3Consumidos / 6.50 ;
    }

    double adicionalBandeira = 0.0;
    if (request.Bandeira == "Amarela")
    {
        adicionalBandeira = consumoFaturado * 0.1;
    }
    else if (request.Bandeira == "Vermelha")
    {
        adicionalBandeira = consumoFaturado * 0.2;
    }

    double taxaDeEsgoto = 0;
    double valorAgua = request.M3Consumidos * tarifa;
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
        ValorAgua = valorAgua,
        TotalGeral = totalGeral,
        ConsumoFaturado = consumoFaturado
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
     var consumo = await context.Consumos
        .Where(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano)
        .FirstOrDefaultAsync();

    if (consumo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(consumo);
});

app.MapDelete("/api/consumo/remover/{cpf}/{mes}/{ano}", async (string cpf, int mes, int ano, AppDbContext context) =>
{
   var consumo = await context.Consumos
        .Where(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano)
        .FirstOrDefaultAsync();

    if (consumo == null)
    {
        return Results.NotFound();
    }

    context.Consumos.Remove(consumo);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/api/consumo/total-geral", async (AppDbContext context) =>
{
    var totalGeral = await context.Consumos
        .SumAsync(c => c.TotalGeral); 

    if (totalGeral == 0)
    {
        return Results.NotFound("Nenhum consumo registrado.");
    }

    return Results.Ok(new { totalGeral });
});

app.Run();