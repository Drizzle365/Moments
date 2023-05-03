document.addEventListener('scroll', function() {
    const nav = document.querySelector('.header-top');
    const header = document.querySelector('.header');
    if (window.scrollY > header.offsetHeight) {
        nav.classList.add('scrolled');
    } else {
        nav.classList.remove('scrolled');
    }
});
function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}