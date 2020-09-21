using CasaDoCodigo.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly Applicationcontext applicationcontext;

        public DataService(Applicationcontext applicationcontext)
        {
            this.applicationcontext = applicationcontext;
        }

        public void InicializaDB()
        {
            applicationcontext.Database.EnsureCreated();

            var json = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);

            foreach (var livro in livros)
            {
                // Adiciono essas informaçoes em memoria
                applicationcontext.Set<Produto>().Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
            }

            applicationcontext.SaveChanges();
        }
    }

    class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }

}
