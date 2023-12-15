const connection = new signalR.HubConnectionBuilder()
    .withUrl("/alerthub")
    .configureLogging(signalR.LogLevel.Information)
    .build();


connection.on("Order", (data) => {

    console.log("message Receiver")
    console.log(data)
    let menuItem = AlertMenuItem(data);

    console.log(menuItem);
    $(".alert-menu").prepend(menuItem)

    const simpleBar = new SimpleBar(document.querySelectorAll('.alert-menu'));
    simpleBar.getContentElement();

    
});        


async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();


function AlertMenuItem(data) {

    let dropdownMenuItem = `<a href="#" class="dropdown-item py-xl">
        <small class="float-end text-muted ps-2">${data.CreatedDate}</small>
        <div class="media">
            <div class="avatar-md bg-soft-primary">
                <i class="ti ti-chart-arcs"></i>
            </div>
            <div class="media-body align-self-center ms-2 text-truncate">
                <h6 class="my-0 fw-normal text-dark">${data.Title}</h6>
                <small class="text-muted mb-0">${data.Content}</small>
            </div><!--end media-body-->
        </div><!--end media-->
    </a>`


    return dropdownMenuItem;
}   