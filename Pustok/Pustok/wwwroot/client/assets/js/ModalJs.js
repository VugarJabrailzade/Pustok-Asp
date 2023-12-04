$(document).ready(function () {

    let orderViewBtn = document.querySelectorAll(".modal-btn-order");

    orderViewBtn.forEach(bt => {
        bt.addEventListener("click", (e) => {
            e.preventDefault();
            var url = $(e.target).closest('a').attr('href')
            console.log(url)

            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    console.log(data)
                    $(".modal-body-order").html(data);
                },
                error: function () {
                    console.log("ishlemir")
                }
            })
        })
    })

})