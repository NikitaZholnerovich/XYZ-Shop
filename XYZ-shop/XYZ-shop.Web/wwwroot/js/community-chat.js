$(document).ready(function () {
    const $messages = $(".messages");
    const currentUserId = Number($messages.data("current-user-id"));

    const hub = new signalR.HubConnectionBuilder()
        .withUrl("/steam/community-chat")
        .build();

    hub.on("SendChatMessage", function (userId, userName, avatarUrl, message) {
        appendMessage({
            userId: userId,
            userName: userName,
            avatarUrl: avatarUrl || "/images/default-avatar.png",
            message: message,
            isOwn: Number(userId) === currentUserId
        });
    });

    hub.start();

    $("#sendButton").on("click", function () {
        const message = $("#messageInput").val().trim();
        if (!message) {
            return;
        }

        $.get(`/api/Chat/SendChatMessage?message=${encodeURIComponent(message)}`, function () {
            $("#messageInput").val("");
        });
    });

    function appendMessage(data) {
        const sideClass = data.isOwn ? "message-own" : "message-other";
        const $row = $(`
            <div class="message ${sideClass}">
                <img class="message-avatar" src="${escapeHtml(data.avatarUrl)}" alt="" />
                <div class="message-body">
                    <div class="message-author">${escapeHtml(data.userName)}</div>
                    <div class="message-text">${escapeHtml(data.message)}</div>
                </div>
            </div>
        `);

        $messages.append($row);
        $messages.scrollTop($messages[0].scrollHeight);
    }

    function escapeHtml(value) {
        return String(value)
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#39;");
    }
});
