document.addEventListener('scroll', function () {
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

function toast(title, content) {
  const div = document.getElementById('toast-div');
  let html = `<div class="toast-container position-fixed top-0 end-0 p-3">
  <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">${title}</strong>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      ${content}
    </div>
  </div>
</div>`
  div.innerHTML = html;
  document.body.appendChild(div);
  const toast = new bootstrap.Toast(document.getElementById('liveToast'))
  toast.show()
}