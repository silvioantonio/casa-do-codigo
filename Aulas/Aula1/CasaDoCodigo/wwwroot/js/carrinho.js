


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

        return data = {
            Id: itemId,
            Quantidade: novaQuantidade
        };
    }

    postQuantidade(data) {
        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: data
        }).done(function(response) {
            location.reload()
        });
    }
}

var carrinho = new Carrinho();




