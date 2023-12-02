
let productQuantityIncrease = document.querySelectorAll(".inc");
let productQuantityDecrease = document.querySelectorAll(".dec"); 

productQuantityIncrease.forEach(pq => {
    pq.addEventListener("click", (e) => {
        updateProductAmount("https://localhost:7258/basket/increase-basket-product", e);
    })
})

productQuantityDecrease.forEach(pq => {
    pq.addEventListener("click", (e) => {
        updateProductAmount("https://localhost:7258/basket/decrease-basket-product", e);
    })
})


function updateProductAmount(updateUrl, e) {
    let productDetailsElement = $(e.target).closest(".product-details");
    let productQuantityElement = productDetailsElement.find(".product-quantity");
    let productAmount = productDetailsElement.find(".product-subtotal").find(".amount");
    let basketProductId = productQuantityElement.data("basket-product-id");

    console.log(productDetailsElement)
    console.log(productQuantityElement)
    console.log(productAmount)
    console.log(basketProductId)



    let url = `${updateUrl}/${basketProductId}`;

    $.ajax(url)
        .done(function (data, _, response) {
            if (response.status == 200) {
                productAmount.html(`$${data.total}`);
            }
            else if (response.status == 204) {
                productDetailsElement.remove();
            }
        })
        .fail(function () {
            alert("error");
        });
}