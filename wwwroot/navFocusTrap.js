window.trapFocus = (overlayId) => {
    const el = document.getElementById(overlayId);
    if (!el) return;
    const sel = 'a[href], button:not([disabled]), input, [tabindex]:not([tabindex="-1"])';
    el.addEventListener('keydown', function(e) {
        if (e.key !== 'Tab') return;
        const focusable = Array.from(el.querySelectorAll(sel));
        const first = focusable[0];
        const last = focusable[focusable.length - 1];
        if (e.shiftKey && document.activeElement === first) {
            e.preventDefault(); last.focus();
        } else if (!e.shiftKey && document.activeElement === last) {
            e.preventDefault(); first.focus();
        }
    });
};

window.focusElement = (id) => document.getElementById(id)?.focus();
