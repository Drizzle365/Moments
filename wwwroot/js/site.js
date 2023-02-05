function friendsOnload(target) {
    let h = document.body.scrollHeight;
    parent.postMessage(h, target);
}