
# Rinha de Backend - 2024/q1
## Feito por Ryan Furtado

<div>
  <img style="height:80px" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-original.svg" />
  <img style="height:80px" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/dotnetcore/dotnetcore-original.svg" />
  <img style="height:80px" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/mysql/mysql-original-wordmark.svg" />
</div>

- `.net 8` with MinimalAPI, without DTO. 
- `mysql` as DataBase.

- [Repositório da api](https://github.com/RyanFurt12/rinha-2024-q1)

<hr>

## Social

<div> 
  <a href="https://www.instagram.com/ryanfurt_12/" target="_blank"><img src="https://img.shields.io/badge/-Instagram-%23E4405F?style=for-the-badge&logo=instagram&logoColor=white" target="_blank"></a>
  <a href="https://www.linkedin.com/in/ryanfurtadoa/" target="_blank"><img src="https://img.shields.io/badge/-LinkedIn-%230077B5?style=for-the-badge&logo=linkedin&logoColor=white" target="_blank"></a> 
  <a href="https://github.com/RyanFurt12" target="_blank"><img src="https://img.shields.io/badge/-GitHub-%23303030?style=for-the-badge&logo=github&logoColor=white" target="_blank"></a> 
</div>          


OBS: Docker ainda não esta funcionando

## Documentação da API

#### Retorna o extrato do cliente informado

```
  GET /clientes/{id}/extrato
```

#### Retorna um item

```
  POST /clientes/{id}/transacao
```

| Var   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**: O ID do cliente desejado. |


