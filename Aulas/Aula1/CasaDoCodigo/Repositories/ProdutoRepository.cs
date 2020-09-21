using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(Applicationcontext applicationcontext) : base(applicationcontext)
        {
        }

        public IList<Produto> GetProdutos()
        {
            return applicationcontext.Set<Produto>().ToList();
        }

        public void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSets.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    // Adiciono essas informaçoes em memoria
                    dbSets.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
                }
               
            }

            applicationcontext.SaveChanges();
        }

    }
    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }

}
