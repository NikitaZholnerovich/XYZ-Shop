document.addEventListener('DOMContentLoaded', function () {
    const carousel = document.getElementById('featuredCarousel');
    const dots = document.querySelectorAll('#featuredDots .dot');

    if (carousel && dots.length) {
        const setActive = (index) => {
            dots.forEach((dot, i) => dot.classList.toggle('active', i === index));
        };

        dots.forEach((dot, i) => {
            dot.onclick = () => {
                carousel.scrollTo({ left: i * carousel.clientWidth, behavior: 'smooth' });
                setActive(i);
            };
        });

        carousel.onscroll = () => {
            setActive(Math.round(carousel.scrollLeft / carousel.clientWidth));
        };
    }
});
