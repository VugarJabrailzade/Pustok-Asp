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
                        postBtn.addEventListener("click", (e) =>
                        {
                            e.preventDefault();
                            var href = e.target.parentElement.children[0].href;
                            var name = e.target.parentElement.children[1].children[1].value;
                            console.log(name)

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
                                     
                                }
                            })
                        })
                    })
                }
            })
        })
    })


   
  
});


