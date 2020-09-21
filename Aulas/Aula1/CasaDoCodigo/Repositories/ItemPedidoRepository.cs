using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IItemPedidoRepository {
        void UpdateQuantidade(ItemPedido itemPedido);
    }
    public class ItemPedidoRepository : BaseRepository<ItemPedido>, IItemPedidoRepository
    {
        public ItemPedidoRepository(Applicationcontext applicationcontext) : base(applicationcontext)
        {
        }

        public void UpdateQuantidade(ItemPedido itemPedido)
        {
            
        }
    }
}
