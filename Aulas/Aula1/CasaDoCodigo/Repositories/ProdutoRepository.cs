using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly Applicationcontext applicationcontext;

        public ProdutoRepository(Applicationcontext applicationcontext)
        {
            this.applicationcontext = applicationcontext;
        }

        public void SaveProdutos(List<Livro> livros)
        {
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
