// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById('feed-filter')?.addEventListener('change', (e) => {
    e.currentTarget.requestSubmit();
});

document.addEventListener('submit', async (e) => {
    const form = e.target.closest('form.like-form, form.delete-form, form.follow-form');
    if (!form) return;               // ignore every other form

    e.preventDefault();              // stop the normal page navigation

    const isDelete = form.classList.contains('delete-form');
    
    const isFollow = form.classList.contains('follow-form');

    if (isDelete && !confirm('Delete this post? This cannot be undone.')) return;

    const button = form.querySelector('button');
    if (button.disabled) return;     // ignore clicks while a request is in flight
    button.disabled = true;

    try {
        const response = await fetch(form.action, {
            method: 'POST',
            body: new FormData(form),    // includes the anti-forgery token
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        // Anonymous or forbidden requests get redirected (login / access denied),
        // and fetch follows that silently, so detect it and go there.
        if (response.redirected) {
            window.location.href = response.url;
            return;
        }

        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }

        if (isDelete) {
            form.closest('.card').remove();
            return;                  // the form is gone, so there's nothing to re-enable
        }

        const data = await response.json();
        
        if(isFollow) {
            button.textContent = data.isNowFollowing ? 'Unfollow' : 'Follow';
            const count = form.closest('.card')?.querySelector('.follower-count');
            if (count) {
                count.textContent = `${data.followerCount} followers`;
            }
            button.disabled = false;
            return;
        }

        button.textContent = data.liked ? 'Unlike' : 'Like';

        const count = form.closest('.card')?.querySelector('.like-count');
        if (count) {
            count.textContent = `${data.likeCount} ${data.likeCount === 1 ? 'Like' : 'Likes'}`;
        }
    } catch (err) {
        console.error('Request failed:', err);
        alert('Something went wrong. Please try again.');
    } finally {
        button.disabled = false;
    }
});

const modalEl = document.getElementById('replyModal');
const modal = new bootstrap.Modal(modalEl);
const contentEl = document.getElementById('replyModalContent');

document.querySelectorAll('[data-reply-post]').forEach(btn => {
    btn.addEventListener('click', async () => {
        const res = await fetch(`/Replies/ReplyModal?postId=${btn.dataset.replyPost}`);
        contentEl.innerHTML = await res.text();
        wireReplyForm();
        modal.show();
    });
});

function wireReplyForm() {
    const form = document.getElementById('replyForm');
    if (!form) return;
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const err = document.getElementById('replyError');
        err.textContent = '';
        const res = await fetch(form.action, { method: 'POST', body: new FormData(form) });
        if (res.ok) {
            const html = await res.text();
            document.getElementById('repliesList').insertAdjacentHTML('beforeend', html);
            form.querySelector('textarea').value = '';
            form.querySelector('textarea').focus();
        } else {
            err.textContent = (await res.text()) || 'Could not post reply.';
        }
    });
}