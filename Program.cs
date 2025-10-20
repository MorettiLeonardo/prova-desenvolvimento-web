var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapPost("/api/consumo/cadastrar", async (AppDbContext context, [FromBody] CadastrarRequest request) =>
{
    

    context.Add(request);
    await context.SaveChangesAsync();
});

app.MapGet("/api/consumo/listar", async (AppDbContext context) =>
{
    var consumos = context.Consumos.ToList();

    if (consumos == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(consumos);
});

app.MapGet("/api/consumo/buscar/{cpf}/{mes}/{ano}", async (string cpf, string mes, string ano, AppDbContext context) =>
{
    var consumo = context.Consumos.ToList();

    if (consumo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(consumo);
});


app.Run();
