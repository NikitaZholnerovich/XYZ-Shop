$(document).ready(function () {
    function escapeHtml(value) {
        return $('<div>').text(value ?? '').html();
    }

    function panelLabels() {
        const $panel = $('#reviews-panel');
        return {
            edited: $panel.data('edited-label') || 'Edited',
            confirmDelete: $panel.data('confirm-delete') || 'Delete this review?',
            save: $panel.data('save-label') || 'Save',
            cancel: $panel.data('cancel-label') || 'Cancel',
            edit: $panel.data('edit-label') || 'Edit',
            delete: $panel.data('delete-label') || 'Delete'
        };
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

    function manageButtonsHtml(labels) {
        return `
            <div class="review-actions">
                <button type="button" class="btn btn--small review-edit-btn">${escapeHtml(labels.edit)}</button>
                <button type="button" class="btn btn--small review-delete-btn">${escapeHtml(labels.delete)}</button>
            </div>`;
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

    $(document).on('click', '.review-edit-btn', function () {
        const $item = $(this).closest('.review-item');
        if ($item.find('.review-edit-form').length) {
            return;
        }

        const labels = panelLabels();
        const text = $item.find('.review-text').text();
        const rating = $item.data('rating');

        $item.find('.review-text, .review-actions').hide();
        $item.append(`
            <div class="review-edit-form">
                <textarea class="review-edit-text" maxlength="5000">${escapeHtml(text)}</textarea>
                <input class="review-edit-rating" type="number" min="1" max="10" value="${escapeHtml(String(rating))}" />
                <div class="review-edit-actions">
                    <button type="button" class="btn btn--small review-save-btn">${escapeHtml(labels.save)}</button>
                    <button type="button" class="btn btn--small review-cancel-edit-btn">${escapeHtml(labels.cancel)}</button>
                </div>
            </div>
        `);
    });

    $(document).on('click', '.review-cancel-edit-btn', function () {
        const $item = $(this).closest('.review-item');
        $item.find('.review-edit-form').remove();
        $item.find('.review-text, .review-actions').show();
    });

    $(document).on('click', '.review-save-btn', function () {
        const $item = $(this).closest('.review-item');
        const id = Number($item.data('id'));
        const text = ($item.find('.review-edit-text').val() || '').trim();
        const rating = Number($item.find('.review-edit-rating').val());
        const labels = panelLabels();

        if (!text || !rating || rating < 1 || rating > 10 || text.length < 3) {
            showToast('Fill all fields');
            return;
        }

        $.ajax({
            url: '/api/GameReview/Edit',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ id, text, rating }),
            success: function (response) {
                const modifiedAt = response.modifiedAt
                    ? new Date(response.modifiedAt).toLocaleString()
                    : '';

                $item.data('rating', response.rating);
                $item.attr('data-rating', response.rating);
                $item.find('.review-rating').text(`${response.rating} / 10`);
                $item.find('.review-text').text(response.text);

                const $dates = $item.find('.review-dates');
                $dates.find('.review-edited').remove();
                if (modifiedAt) {
                    $dates.append(`
                        <span class="review-edited">
                            · ${escapeHtml(labels.edited)}
                            <time>${escapeHtml(modifiedAt)}</time>
                        </span>
                    `);
                }

                $item.find('.review-edit-form').remove();
                $item.find('.review-text, .review-actions').show();
                sortReviews();
                showToast('Review updated', 'success');
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.error || 'Error updating review');
            }
        });
    });

    $(document).on('click', '.review-delete-btn', function () {
        const labels = panelLabels();
        if (!window.confirm(labels.confirmDelete)) {
            return;
        }

        const $item = $(this).closest('.review-item');
        const id = Number($item.data('id'));

        $.ajax({
            url: '/api/GameReview/Delete',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ id }),
            success: function () {
                $item.remove();
                if (!$('#reviews-panel .review-item').length) {
                    $('#reviews-empty').show();
                }
                showToast('Review deleted', 'success');
                // restore write form for own review / keep UI consistent
                location.reload();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.error || 'Error deleting review');
            }
        });
    });

    $(document).on('submit', '#review-form', function (e) {
        e.preventDefault();

        const gameId = $(this).data('game-id');
        const text = ($('#review-text').val() || '').trim();
        const rating = Number($('#review-rating').val());
        const labels = panelLabels();

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
                             data-id="${escapeHtml(String(review.id))}"
                             data-rating="${escapeHtml(String(review.rating))}"
                             data-created="${escapeHtml(createdAtIso)}">
                        <div class="review-item-header">
                            <img class="user-avatar-small" src="${escapeHtml(avatarUrl)}" alt="" />
                            <b>${escapeHtml(review.author)}</b>
                            <span class="review-rating">${escapeHtml(String(review.rating))} / 10</span>
                            <span class="review-dates"><time>${escapeHtml(createdAt)}</time></span>
                        </div>
                        <div class="review-text">${escapeHtml(review.text)}</div>
                        ${manageButtonsHtml(labels)}
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
