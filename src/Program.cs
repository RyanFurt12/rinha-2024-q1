using Microsoft.AspNetCore.Mvc;
using rinha_2024_q1.data;
using rinha_2024_q1.model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MySQLService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("clientes");

app.MapPost("{id}/transacoes",(int id, [FromBody]Transaction transaction, MySQLService mySQLService) => {
    bool idExist = mySQLService.CheckIfClientExists(id);
    if (!idExist) return Results.NotFound("Cliente não encontrado");
    
    if (transaction.tipo.ToLower() != "c" &
        transaction.tipo.ToLower() != "d" ||
        transaction.descricao.Length > 10 ||
        transaction.descricao.Length == 0 ||
        transaction.valor % 1 != 0 ||
        transaction.valor < 0) 
            return Results.UnprocessableEntity("Erro no corpo da requisição");
    
    if (transaction.tipo == "d" & !mySQLService.TestClientHasBalance(id, transaction.valor))
            return Results.UnprocessableEntity("Saldo inconsistente para essa transação");

    var response = mySQLService.PostTransaction(id, transaction);

    return Results.Ok(response);
});

app.MapGet("{id}/extrato",(int id, MySQLService mySQLService) => {
    var extrato = mySQLService.GetClientExtract(id);
    if(extrato == null) return Results.NotFound("Cliente não encontrado");

    return Results.Ok(extrato);
});

app.Run();