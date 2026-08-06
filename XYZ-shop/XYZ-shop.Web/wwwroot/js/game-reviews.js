$(document).ready(function () {
    function escapeHtml(value) {
        return $('<div>').text(value ?? '').html();
    }

    function sortReviews() {
        const sortBy = $('#reviews-sort').val();
        const $panel = $('#reviews-panel');
        const $items = $panel.children('.review-item').get();

        $items.sort(function (a, b) {
            const $a = $(a);
            const $b = $(b);

            if (sortBy === 'highest') {
                return Number($b.data('rating')) - Number($a.data('rating'));
            }

            if (sortBy === 'lowest') {
                return Number($a.data('rating')) - Number($b.data('rating'));
            }

            return new Date($b.data('created')) - new Date($a.data('created'));
        });

        $panel.append($items);
    }

    function showToast(message, type = 'error') {
        const toast = $(`
            <div class="toast toast--${type}">
                ${escapeHtml(message)}
            </div>
        `);

        $('#toast-container').append(toast);

        requestAnimationFrame(() => {
            toast.addClass('show');
        });

        setTimeout(() => {
            toast.removeClass('show');
            setTimeout(() => toast.remove(), 250);
        }, 3000);
    }

    $(document).on('change', '#reviews-sort', sortReviews);

    $(document).on('click', '#show-review-form-btn', function () {
        $('#show-review-form-btn').hide();
        $('#review-form-container')
            .removeClass('review-form-hidden')
            .addClass('review-form-visible');
    });

    $(document).on('click', '#cancel-review-btn', function () {
        $('#review-form-container')
            .removeClass('review-form-visible')
            .addClass('review-form-hidden');
        $('#show-review-form-btn').show();
        $('#review-form')[0].reset();
    });

    $(document).on('submit', '#review-form', function (e) {
        e.preventDefault();

        const gameId = $(this).data('game-id');
        const text = ($('#review-text').val() || '').trim();
        const rating = Number($('#review-rating').val());

        if (!text || !rating || rating < 1 || rating > 10) {
            showToast('Fill all fields');
            return;
        }

        if (text.length < 3) {
            showToast('Review must be at least 3 characters');
            return;
        }

        $.ajax({
            url: '/api/GameReview/Add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                gameId: gameId,
                text: text,
                rating: rating
            }),
            success: function (review) {
                const avatarUrl = review.authorAvatarUrl || '/images/default-avatar.png';
                const createdAtIso = review.createdAt || new Date().toISOString();
                const createdAt = new Date(createdAtIso).toLocaleString();

                $('#reviews-empty').hide();
                $('#reviews-panel').prepend(`
                    <article class="review-item"
                             data-rating="${escapeHtml(String(review.rating))}"
                             data-created="${escapeHtml(createdAtIso)}">
                        <div class="review-item-header">
                            <img class="user-avatar-small" src="${escapeHtml(avatarUrl)}" alt="" />
                            <b>${escapeHtml(review.author)}</b>
                            <span class="review-rating">${escapeHtml(String(review.rating))} / 10</span>
                            <span class="review-dates"><time>${escapeHtml(createdAt)}</time></span>
                        </div>
                        <div>${escapeHtml(review.text)}</div>
                    </article>
                `);

                sortReviews();
                showToast('Review added', 'success');

                $('#show-review-form-btn').remove();
                $('#review-form-container').remove();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.error || 'Error sending review');
            }
        });
    });
});
