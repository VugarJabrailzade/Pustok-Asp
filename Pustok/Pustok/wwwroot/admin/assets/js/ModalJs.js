
$(document).ready(function () {
    var addBtn = document.querySelectorAll(".addCate");


    addBtn.forEach(btn => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            var url = e.target.href;

            console.log(url);
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    console.log(data)
                    $(".modal-body").html(data);

                    var addCategoryBtn = document.querySelectorAll(".addCateBtn");

                    addCategoryBtn.forEach(postBtn => {
                        postBtn.addEventListener("click", (e) => {
                            e.preventDefault();
                            var href = e.target.parentElement.children[0].href;
                            var name = e.target.parentElement.children[1].children[1].value;

                            console.log(href)
                            


                            var model =
                            {
                                name: name,
                            }
                            $.ajax({
                                type: "POST",
                                url: href,
                                data: {
                                    model: model
                                },
                                success: function (res) {

                                    $.ajax({
                                        type: "GET",
                                        url: '/admin/category/index',
                                        success: function (res) {
                                            window.location.href = '/admin/category/index'
                                        },
                                    })
                                },
                                error: function (xhr, textStatus, errorThrown) {

                                    var errmessage = xhr.responseJSON.message
                                    $('#errormessage').text(errmessage);
                                }
                            })
                        })
                    })
                }
            })
        })
    })




    var deleteCate = document.querySelectorAll(".deleteCate");

    deleteCate.forEach(btn => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            var href = e.target.parentElement.href

            console.log(href);

            $.ajax({
                type: 'DELETE',
                url: href,
                success: function (result) {
                    $.ajax({
                        type: "get",
                        url: '/admin/category/index',
                        success: function (res) {
                            window.location.href = '/admin/category/index'

                            console.log(res)

                        }
                    })

                }
            })


        })
    })

})




$(document).ready(function () {

    var updateCate = document.querySelectorAll(".updateCate");

    updateCate.forEach(btn => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            var url = e.target.parentElement.href
            var id = e.target.parentElement.previousElementSibling.value;
            console.log(id);

            console.log(url)

            $.ajax({
                type: "GET",
                url: url,
                data: {
                     id: id
                },
                success: function (res) {
                   
                    $(".modal-body").html(res);

                    var uptBtn = document.querySelectorAll(".updCateBtn");

                    uptBtn.forEach(btn => {
                        btn.addEventListener("click", (e) => {
                            e.preventDefault();
                            var urel = e.target.parentElement.parentElement.children[0].children[0].href;
                            var updatedName = e.target.closest('form').querySelector('[name="Name"]').value;

                            console.log(urel)

                            var data = {
                                name: updatedName
                            };

                            $.ajax({
                                type: "POST",
                                url: urel,
                                data: data,
                                success: function (res) {
                                    console.log(res)

                                    $.ajax({
                                        type: "GET",
                                        url: '/admin/category/index',
                                        success: function (res) {
                                            window.location.href = '/admin/category/index'
                                        }
                                       
                                    })
                                }
                            })

                        })
                    })

                },
                error: function (err) {
                    console.log("islemir")
                }
            })

        })
    })


})




