$(document).ready(function () {
    const url = '/steam/notification';
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewGameAdded', function (title, imageUrl) {
        const notificationDiv = $('<div>');
        notificationDiv.addClass('notification');
        notificationDiv.text(`New game added: ${title}`);

        if (imageUrl) {
            const img = $('<img>').attr('src', imageUrl).addClass('preview');
            notificationDiv.append(img);
        }

        notificationDiv.on('click', hideNotification);
        $('.notifications').append(notificationDiv);
    });

    function hideNotification() {
        $(this).fadeOut(500, function () { $(this).remove(); });
    }

    hub.start().catch(function (err) {
        console.error('Steam notification hub connection failed:', err);
    });
});