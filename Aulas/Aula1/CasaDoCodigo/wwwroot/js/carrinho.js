


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

        return  {
            Id: itemId,
            Quantidade: novaQuantidade
        };

    }

    postQuantidade(data) {

        //Pega o token no frontend
        let token = $('[name=__RequestVerificationToken]').val();

        let headers = {};
        headers['RequestVerificationToken'] = token;

        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        }).done(function (response) {

            let itemPedido = response.itemPedido;
            let linhaDoItem = $('[item-id=' + itemPedido.id + ']');
            linhaDoItem.find('input').val(itemPedido.quantidade);
            linhaDoItem.find('[subtotal]').html(itemPedido.subTotal);

            let carrinhoViewModel = response.carrinhoViewModel;

            $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');

            $('[total]').html(carrinhoViewModel.total);

            if (itemPedido.quantidade == 0) {
                linhaDoItem.remove();
            }
            debugger;
        });
    }
}

var carrinho = new Carrinho();




