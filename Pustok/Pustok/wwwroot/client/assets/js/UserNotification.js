const connection = new signalR.HubConnectionBuilder()
    .withUrl("/alerthub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("UpdatedStatus", (user) => {

    console.log("message Receiver")
    console.log(user)
    let menuItem = AlertUserMenu(user);

    console.log(menuItem);
    $(".alert-user-menu").prepend(menuItem)

    const simpleBar = new SimpleBar(document.querySelectorAll('.alert-user-menu'));
    simpleBar.getContentElement();


}); 

connection.on("ReceiveMessage", (message) => {

    let menuItem = BroadcastNotification(message);

    console.log(menuItem);
    $(".alert-user-menu").prepend(menuItem)

    const simpleBar = new SimpleBar(document.querySelectorAll('.alert-user-menu'));
    simpleBar.getContentElement();
    console.log(`Received message: ${message}`);
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

function AlertUserMenu(data) {

    console.log(data)
    let dropdownmenuitemUser = `<a href="#" class="dropdown-item py-xl">
        <small class="float-end text-muted ps-2">${data.createdAt}</small>
        <div class="media">
            <div class="avatar-md bg-soft-primary">
                <i class="ti ti-chart-arcs"></i>
            </div>
            <div class="media-body align-self-center ms-2 text-truncate">
                <h6 class="my-0 fw-normal text-dark">${data.title}</h6>
                <small class="text-muted mb-0">${data.content}</small>
            </div><!--end media-body-->
        </div><!--end media-->
    </a>`


    return dropdownmenuitemUser;
}

function BroadcastNotification(data) {
    let broadcastMessage = `<a href="#" class="dropdown-item py-xl">
        <div class="media">
            <div class="avatar-md bg-soft-primary">
                <i class="ti ti-chart-arcs"></i>
            </div>
            <div class="media-body align-self-center ms-2 text-truncate">
                <small class="text-muted mb-0">${data.content}</small>
            </div><!--end media-body-->
        </div><!--end media-->
    </a>`

    return broadcastMessage;
}

