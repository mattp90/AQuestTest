var percentageDiscount = 0;

$(document).ready(function () {
    $('#form-order-detail').submit(function (e) {

        // this code prevents form from actually being submitted
        e.preventDefault();
        e.returnValue = false;
        var $form = $(this);

        // Before submit, check quantity for purchase
        $.ajax({
            'url': CheckQuantityForPurchaseUrl,
            'type': 'GET',
            'async': 'false',
            'data': {
                'idOrder': $('#Id').val()
            },
            'success': function (data) {

                if (data.length == 0) {
                    $form.off('submit');
                    $form.submit();
                }
                else {
                    $.alert({
                        animateFromElement: false,
                        theme: 'Bootstrap',
                        title: 'Errore!',
                        content: data.join("<br />"),
                        type: 'red',
                        typeAnimated: true,
                        buttons: {
                            close: {
                                text: 'Chiudi',
                                // btnClass: 'btn-red',
                                action: function () {
                                }
                            }
                        }
                    });
                }
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    });
});

function ApplyCouponClick() {
    $.ajax({
        'url': ApplyCouponUrl,
        'type': 'GET',
        'data': {
            'idOrder': $('#Id').val(),
            'coupon': $('#txtCoupon').val()
        },
        'success': function (data) {
            if (data.result == "success") {
                $.alert({
                    animateFromElement: false,
                    theme: 'Bootstrap',
                    title: 'Ok!',
                    content: 'Coupon applicato correttamente!',
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            text: 'Ok',
                            btnClass: 'btn-green',
                            action: function () {
                                
                                $('#col-total-price').append(
                                    '<br />'+
                                    '<label id="span_total_price_order_discounted" class="cart-quantity-value">' +
                                        data.newTotalPriceOrder +
                                    '</label>');
                                $('#span_total_price_order').css('text-decoration', 'line-through')

                                $('#Coupon').val(data.coupon.Code);
                                $('#IdCoupon').val(data.coupon.Id);
                                percentageDiscount = data.coupon.PercentageDiscount;

                                // Apply discount to total price
                                $('#TotalPriceOrder').val(toItalianFormat(data.newTotalPriceOrder));
                                
                                $('#lbl-coupon-applied').text('Coupon applicato: ' + $('#txtCoupon').val());
                                $('#row-coupons').show();

                                $('#txtCoupon').val('');
                                $('#txtCoupon').prop("disabled", true);
                                $('#btnApplyCoupon').prop("disabled", true);
                            }
                        }
                    }
                });
            }
            else {
                $.alert({
                    animateFromElement: false,
                    theme: 'Bootstrap',
                    title: 'Errore!',
                    content: 'Coupon inesistente.',
                    type: 'red',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            text: 'Ok',
                            btnClass: 'btn-red',
                            action: function () {

                            }
                        }
                    }
                });
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function AddSingleItem(idOrder, idProduct, indexProduct) {
    var newQuantity = parseInt($('#Products_' + indexProduct + '__Quantity').val()) + 1;

    $.ajax({
        'url': AddProductToCartUrl,
        'type': 'GET',
        'data': {
            'idProduct': idProduct,
            'idOrder': idOrder,
            'newQuantity': newQuantity
        },
        'success': function (data) {
            if (data.result == "success") {

                var _newTotalPriceOrder = data.newTotalPriceOrder.toFixed(2);
                var _newTotalPriceOrderWithoutDiscount = data.newTotalPriceWithoutDiscount.toFixed(2);
                var _newTotalPriceItem = data.newTotalPriceItem.toFixed(2);

                $('#Products_' + indexProduct + '__Quantity').val(newQuantity);
                $('#Products_' + indexProduct + '__TotalPriceItem').val(toItalianFormat(_newTotalPriceItem));
                $('#TotalPriceOrder').val(toItalianFormat(_newTotalPriceOrder));

                $('#span_quantity_product_' + indexProduct).text(toItalianFormat(newQuantity));
                $('#span_total_price_product_' + indexProduct).text(toItalianFormat(_newTotalPriceItem));
                $('#span_total_price_order').text(toItalianFormat(_newTotalPriceOrderWithoutDiscount));
                $('#span_total_price_order_discounted').text(toItalianFormat(_newTotalPriceOrder));
            }
            else {
                $.alert({
                    animateFromElement: false,
                    theme: 'Bootstrap',
                    title: 'Attenzione!',
                    content: "Raggiunta quantità massima presente in magazzino.",
                    type: 'orange',
                    typeAnimated: true,
                    buttons: {
                        close: {
                            text: "Ok.",
                            action: function () {
                            }
                        }
                    }
                });
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function RemoveSingleItem(idOrder, idProduct, indexProduct) {
    var newQuantity = parseInt($('#Products_' + indexProduct + '__Quantity').val()) - 1;

    if (newQuantity == 0) {

        if ($('#tbl-list-items tr:visible').length == 3) {
            // is last item in cart; generate an alert; if confermed, the order will be deleted
            $.confirm({
                animateFromElement: false,
                theme: 'Bootstrap',
                title: 'Attenzione!',
                content: "Si sta eliminando l'ultimo oggetto nel carrello; se si procede, l'ordine verrà cancellato.",
                type: 'orange',
                typeAnimated: true,
                buttons: {
                    Ok: {
                        text: 'Procedi',
                        btnClass: 'btn-orange',
                        action: function () {
                            DeleteOrder(idOrder);
                        }
                    },
                    close: {
                        text: "Chiudi",
                        action: function () {
                        }
                    }
                }
            });
        }
        else {
            RemoveAllItems(idOrder, idProduct, indexProduct)
        }
    }
    else {
        
        $.ajax({
            'url': RemoveProductToCartUrl,
            'type': 'GET',
            'data': {
                'idProduct': idProduct,
                'idOrder': idOrder,
                'newQuantity': newQuantity
            },
            'success': function (data) {
                if (data.result == "success") {

                    var _newTotalPriceOrder = data.newTotalPriceOrder.toFixed(2);
                    var _newTotalPriceItem = data.newTotalPriceItem.toFixed(2);

                    $('#Products_' + indexProduct + '__Quantity').val(newQuantity);
                    $('#Products_' + indexProduct + '__TotalPriceItem').val(toItalianFormat(_newTotalPriceItem));
                    $('#TotalPriceOrder').val(toItalianFormat(_newTotalPriceOrder));

                    $('#span_quantity_product_' + indexProduct).text(toItalianFormat(newQuantity));
                    $('#span_total_price_product_' + indexProduct).text(toItalianFormat(_newTotalPriceItem));
                    $('#span_total_price_order').text(toItalianFormat(_newTotalPriceOrder));
                }
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }
}

function RemoveAllItems(idOrder, idProduct, indexProduct) {
    if ($('#tbl-list-items tr:visible').length == 3) {
        // is last item in cart; generate an alert; if confermed, the order will be deleted
        $.confirm({
            animateFromElement: false,
            theme: 'Bootstrap',
            title: 'Attenzione!',
            content: "Si sta eliminando l'ultimo oggetto nel carrello; se si procede, l'ordine verrà cancellato.",
            type: 'orange',
            typeAnimated: true,
            buttons: {
                Ok: {
                    text: 'Procedi',
                    btnClass: 'btn-orange',
                    action: function () {
                        DeleteOrder(idOrder);
                    }
                },
                close: {
                    text: "Chiudi",
                    action: function () {
                    }
                }
            }
        });
    }
    else {
        $.ajax({
            'url': RemoveProductFromOrderUrl,
            'type': 'GET',
            'data': {
                'idProduct': idProduct,
                'idOrder': idOrder,
                'newQuantity': 0
            },
            'success': function (data) {
                if (data.result == "success") {

                    var _newTotalPriceOrder = data.newTotalPriceOrder.toFixed(2);

                    $('#TotalPriceOrder').val(toItalianFormat(_newTotalPriceOrder));
                    $('#span_total_price_order').text(toItalianFormat(_newTotalPriceOrder));

                    var row = $('#' + idProduct);
                    row.fadeOut("slow");
                }
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }
}

function DeleteOrder(idOrder) {
    $.ajax({
        'url': DeleteOrderUrl,
        'type': 'GET',
        'data': {
            'idOrder': idOrder
        },
        'success': function (data) {
            window.location.href = baseUrl;
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function ApplyDiscountToTotalPrice(total, discount) {
    var discount = parseFloat(((total / 100) * discount).toFixed(2));
    var _total = parseFloat(total);
    return (_total - discount).toFixed(2);
}

function GetFloat(value) {
    return parseFloat(value.toString().replace(',', '.')).toFixed(2);
}

function toItalianFormat(value) {
    return value.toString().replace('.', ',')
}