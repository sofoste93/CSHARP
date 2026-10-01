(() => {
    const key = "violet-pulsar-settings-v2";
    const defaults = { theme: "dark", compact: false, reducedMotion: false };
    let settings;
    try { settings = { ...defaults, ...JSON.parse(localStorage.getItem(key)) }; } catch { settings = { ...defaults }; }

    const apply = () => {
        const resolved = settings.theme === "system"
            ? (matchMedia("(prefers-color-scheme: light)").matches ? "light" : "dark")
            : settings.theme;
        document.documentElement.dataset.theme = resolved;
        document.body.classList.toggle("compact", settings.compact);
        document.body.classList.toggle("reduced-motion", settings.reducedMotion);
    };
    apply();

    const dialog = document.querySelector("[data-settings-dialog]");
    document.querySelector("[data-settings-open]")?.addEventListener("click", () => {
        dialog.querySelector("[data-theme]").value = settings.theme;
        dialog.querySelector("[data-compact]").checked = settings.compact;
        dialog.querySelector("[data-reduced-motion]").checked = settings.reducedMotion;
        dialog.querySelector("[data-culture]").value = window.pulsarCulture || "en";
        dialog.showModal();
    });
    document.querySelector("[data-settings-close]")?.addEventListener("click", () => dialog.close());
    document.querySelector("[data-settings-form]")?.addEventListener("submit", () => {
        settings = {
            theme: dialog.querySelector("[data-theme]").value,
            compact: dialog.querySelector("[data-compact]").checked,
            reducedMotion: dialog.querySelector("[data-reduced-motion]").checked
        };
        localStorage.setItem(key, JSON.stringify(settings));
        apply();
    });
    matchMedia("(prefers-color-scheme: light)").addEventListener("change", apply);

    document.querySelector("[data-menu-toggle]")?.addEventListener("click", () => document.querySelector("[data-menu]")?.classList.toggle("open"));
    document.querySelector("[data-notification] button")?.addEventListener("click", event => event.currentTarget.closest("[data-notification]").remove());
    if (document.querySelector("[data-notification]")) setTimeout(() => document.querySelector("[data-notification]")?.remove(), 4500);

    document.querySelectorAll("[maxlength]").forEach(field => {
        const counter = document.querySelector(`[data-count-for="${field.name}"]`);
        const update = () => { if (counter) counter.textContent = `${field.value.length} / ${field.maxLength}`; };
        field.addEventListener("input", update); update();
    });
})();
