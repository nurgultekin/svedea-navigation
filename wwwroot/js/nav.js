window.navBreakpoint = {
    watch: function (dotNetRef, query) {
        const mql = window.matchMedia(query);
        const handler = e => { if (e.matches) dotNetRef.invokeMethodAsync('OnEnterDesktop'); };
        mql.addEventListener('change', handler);
    }
};
