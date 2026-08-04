$(document).ready(function () {

    $('article.card').click(function () {
        const self = $(this);
        self.toggleClass('active');
        const atLeastOneItemForRemove = $('article.card.active').length > 0

        if (atLeastOneItemForRemove) {
            $('.form-actions .remove-game-card').removeAttr('disabled');
        } else {
            $('.form-actions .remove-game-card').attr('disabled', 'disabled');
        }
    })

    $('.form-actions .remove-game-card').click(function () {
        const gameIds = [];
        const $button = $(this);

        $button.attr('disabled', 'disabled');

        $('article.card.active').each((x, element) => {
            const id = $(element).attr('data-id');
            gameIds.push(id);
        });

        if (gameIds.length === 0) {
            return;
        }

        const gameIdsStr = gameIds.join('&gameIds=');

        const url = `/api/Catalog/delete?gameIds=${gameIdsStr}`
        $.get(url)
            .done(function () {
                $('article.card.active').remove();
            })
            .fail(function (xhr) {
                if (xhr.status === 403) {
                    alert('Access denied. Admin rights required.');
                } else {
                    alert('Error deleting games');
                }
                $('article.card.active').removeClass('active');
                $button.removeAttr('disabled');
            });
    });


});
