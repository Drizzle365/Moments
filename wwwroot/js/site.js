// 如果nav元素存在就执行下面的时间监听，不存在就不执行
const navScroll = function () {
    const nav = document.querySelector('.header-top');
    const header = document.querySelector('.header');
    if (nav && header) {
        if (window.scrollY > header.offsetHeight) {
            nav.classList.add('scrolled');
        } else {
            nav.classList.remove('scrolled');
        }
    }
}

document.addEventListener('scroll', navScroll);

function scrollToTop() {
    window.scrollTo({top: 0, behavior: 'smooth'});
}


function runCode(htmlCode) {
    var previewWindow = window.open();
    previewWindow.document.write(htmlCode);
    previewWindow.document.close();
}

