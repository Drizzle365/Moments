// 如果nav元素存在就执行下面的时间监听，不存在就不执行
const navScroll = function () {
  const nav = document.querySelector('.header-top');
  const header = document.querySelector('.header');
  if (window.scrollY > header.offsetHeight) {
    nav.classList.add('scrolled');
  } else {
    nav.classList.remove('scrolled');
  }
}
if (document.querySelector('.header-top')) {
  document.addEventListener('scroll', navScroll);
} else {
  document.removeEventListener('scroll', navScroll);
}
function scrollToTop() {
  window.scrollTo({ top: 0, behavior: 'smooth' });
}
