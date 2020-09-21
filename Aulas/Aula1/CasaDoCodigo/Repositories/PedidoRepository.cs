using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        Pedido GetPedido();
        void AddItem(string codigo);
        UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);
        Pedido UpdateCadastro(Cadastro cadastro);
    }
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository iItemPedidoRepository;
        private readonly ICadastroRepository cadastroRepository;

        public PedidoRepository(Applicationcontext applicationcontext,
            IHttpContextAccessor contextAccessor,
            IItemPedidoRepository iItemPedidoRepository,
            ICadastroRepository cadastroRepository) : base(applicationcontext)
        {
            this.contextAccessor = contextAccessor;
            this.iItemPedidoRepository = iItemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
        }

        public void AddItem(string codigo)
        {
            var produto = applicationcontext.Set<Produto>().Where(p => p.Codigo == codigo).SingleOrDefault();

            if (produto==null)
            {
                throw new ArgumentException("Produto nao encontrado");
            }

            var pedido = GetPedido();

            var itemPedido = applicationcontext
                .Set<ItemPedido>()
                .Where(ip => ip.Produto.Codigo == codigo && ip.Pedido.Id == pedido.Id)
                .SingleOrDefault();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);

                applicationcontext.Set<ItemPedido>().Add(itemPedido);

                applicationcontext.SaveChanges();

                SetPedidoId(pedido.Id);
            }
        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = dbSets
                .Include(p=>p.Itens)
                    .ThenInclude(i=>i.Produto)
                .Include(p => p.Cadastro)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault();

            if (pedido == null)
            {
                pedido = new Pedido();
                dbSets.Add(pedido);
                applicationcontext.SaveChanges();
            }
            return pedido;
        }

        private int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = iItemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);

                if (itemPedido.Quantidade == 0)
                {
                    iItemPedidoRepository.RemoveItemPedido(itemPedido.Id);
                }

                applicationcontext.SaveChanges();

                var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);

                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
            }

            throw new ArgumentException("Item pedido nao encontrado!");
        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();
            cadastroRepository.Update(pedido.Cadastro.Id, cadastro);
            return pedido;
        }
    }

}
