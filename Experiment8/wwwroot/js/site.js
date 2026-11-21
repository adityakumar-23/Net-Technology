// Inventra - Site Javascript Utilities
document.addEventListener('DOMContentLoaded', function () {
    // 1. Sidebar Toggle for Mobile Devices
    const sidebarToggle = document.getElementById('sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function (e) {
            e.preventDefault();
            document.getElementById('wrapper').classList.toggle('toggled');
        });
    }

    // 2. Active Navigation Link Highlighting
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('#sidebar-wrapper .nav-link');
    navLinks.forEach(function (link) {
        const href = link.getAttribute('href');
        if (href) {
            const cleanHref = href.toLowerCase();
            if ((currentPath === '/' || currentPath === '/dashboard' || currentPath === '/dashboard/index') &&
                (cleanHref === '/' || cleanHref === '/dashboard' || cleanHref === '/dashboard/index')) {
                link.classList.add('active');
            } else if (currentPath.startsWith(cleanHref) && cleanHref !== '/' && cleanHref !== '/dashboard') {
                link.classList.add('active');
            }
        }
    });

    // 3. Current Live Date & Time Display in Topbar
    const dateDisplay = document.getElementById('currentDateDisplay');
    if (dateDisplay) {
        const options = { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' };
        dateDisplay.textContent = new Date().toLocaleDateString('en-IN', options);
    }

    // 4. Auto-dismiss Alert Messages after 6 seconds
    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        });
    }, 6000);
});
