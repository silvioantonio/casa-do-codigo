


class Carrinho {


    clickIncremento(btn) {

        let data = this.getData(btn);
        data.Quantidade++;
        this.postQuantidade(data);

    }

    clickDecremento(btn) {
        let data = this.getData(btn);
        data.Quantidade--;
        this.postQuantidade(data);
    }

    updateQuantidade(input) {
        let data = this.getData(input);
        this.postQuantidade(data);
    }

    getData(elemento) {
        let linhaItem = $(elemento).parents('[item-id]');

        let itemId = $(linhaItem).attr('item-id');

        let novaQuantidade = $(linhaItem).find('input').val();

        let data = {
            Id: itemId,
            Quantidade: novaQuantidade
        };

        return data;
    }

    postQuantidade(data) {
        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).done(function (response) {

            let itemPedido = response.itemPedido;
            let linhaDoItem = $('[item-id=' + itemPedido.Id + ']');
            linhaDoItem.find('input').val(itemPedido.Quantidade);
            linhaDoItem.find('[subtotal]').html((itemPedido.subtotal).duasCasas());

            let carrinhoViewModel = response.carrinhoViewModel;

            $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');

            $('[total]').html((carrinhoViewModel.Total).duasCasas());

            if (itemPedido.Quantidade == 0) {
                linhaDoItem.remove();
            }

        });
    }
}

var carrinho = new Carrinho();




