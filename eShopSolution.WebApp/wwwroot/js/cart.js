
var CartController = function () {
    this.initialize = function () {
        loadData();
        regsiterEvents();
    }
    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                if (res.length === 0) {
                    $('#tbl_cart').hide();
                }
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    html += "<tr>"
                        + "<td> <img width=\"60\" src=\"" + $('#hidBaseAddress').val() + item.image + "\" alt=\"\" /></td>"
                        + "<td>" + item.Description + "</td>"
                        + "<td><div class=\"input-append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"1\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\" size=\"16\" type=\"text\">"
                        + "<button class=\"btn btn-minus\" data-id=\"" + item.productId + "\" type =\"button\"> <i class=\"icon-minus\"></i></button>"
                        + "<button class=\"btn btn-plus\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-plus\"></i></button>"
                        + "<button class=\"btn btn-danger btn-remove\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-remove icon-white\"></i></button>"
                            + "</div>"
                        + "</td>"
                        + "<td>" + numberWithCommas(item.price) + "</td>"
                        + "<td>" + numberWithCommas(item.price * item.quantity) + "</td>"
                        + "</tr>";

                   total += (item.price * item.quantity);
                });
                $('#cart_body').html(html);
                $('#lbl_number_of_items').text(res.length);
                $('#lbl_total').html(numberWithCommas(total));
                
            }
        });
    }
    function regsiterEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        });
        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });
        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }
    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "POST",
            url: "/" + culture + '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                //$('#txt_quantity_' + id).val(quantity);
                $('#lbl_number_of_items_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}
