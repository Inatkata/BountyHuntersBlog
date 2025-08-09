document.addEventListener("submit", async (e) => {
    const form = e.target;
    if (!form.matches("form[id^='like-']")) return;

    e.preventDefault();

    const btn = form.querySelector("[data-like-btn]");
    const countEl = form.querySelector("[data-like-count]");
    const iconEl = form.querySelector("[data-like-icon]");

    const formData = new FormData(form);

    const res = await fetch(form.action, {
        method: "POST",
        headers: { "X-Requested-With": "XMLHttpRequest" },
        body: formData
    });

    if (!res.ok) return; // optionally show toast
    const json = await res.json();

    // json.liked + json.count идват от LikesController. 
    countEl.textContent = json.count;
    if (json.liked) {
        btn.classList.remove("btn-outline-primary");
        btn.classList.add("btn-primary");
        iconEl.textContent = "♥";
    } else {
        btn.classList.add("btn-outline-primary");
        btn.classList.remove("btn-primary");
        iconEl.textContent = "♡";
    }
});
