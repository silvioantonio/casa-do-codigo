using CasaDoCodigo.Models;
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
    }
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        public PedidoRepository(Applicationcontext applicationcontext, IHttpContextAccessor contextAccessor) : base(applicationcontext)
        {
            this.contextAccessor = contextAccessor;
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
            }
        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = dbSets
                .Include(p=>p.Itens)
                .ThenInclude(i=>i.Produto)
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

    }
}
