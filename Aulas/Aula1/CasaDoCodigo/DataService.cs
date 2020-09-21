using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly Applicationcontext applicationcontext;
        private readonly IProdutoRepository produtoRepository;

        public DataService(Applicationcontext applicationcontext, IProdutoRepository produtoRepository)
        {
            this.applicationcontext = applicationcontext;
            this.produtoRepository = produtoRepository;
        }

        public void InicializaDB()
        {
            applicationcontext.Database.EnsureCreated();

            List<Livro> livros = GetLivros();

            produtoRepository.SaveProdutos(livros);
        }

        

        private static List<Livro> GetLivros()
        {
            var json = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }

    
}
