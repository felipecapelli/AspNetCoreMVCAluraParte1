using CasaDoCodigo;
using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

class DataService : IDataService
{
    private readonly ApplicationContext contexto;
    private readonly IProdutoRepository produtoRepository;

    public DataService(ApplicationContext contexto, IProdutoRepository produtoRepository)
    {
        this.contexto = contexto;
        this.produtoRepository = produtoRepository;
    }

    public void InicializaDB()
    {
        contexto.Database.Migrate();//no final pode usar .EnSureCreated() no lugar de Migrate(), mas daí não vai poder mais fazer migrações

        List<Livro> livros = GetLivros();

        produtoRepository.SaveProdutos(livros);
    }

    private static List<Livro> GetLivros()
    {
        //Busca arquivo Json e ja carrega alguns dados no banco
        var json = File.ReadAllText("livros.json");

        var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
        return livros;
    }
}
