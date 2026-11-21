// Experiment 9: Placement Management Dashboard Scripts
document.addEventListener('DOMContentLoaded', function () {
    // 1. Sidebar Toggle
    const sidebarToggle = document.getElementById('sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function (e) {
            e.preventDefault();
            document.getElementById('wrapper').classList.toggle('toggled');
        });
    }

    // 2. Active Sidebar Link Highlighting
    const path = window.location.pathname.toLowerCase();
    const links = document.querySelectorAll('#sidebar-wrapper .nav-link');
    links.forEach(function (link) {
        const href = link.getAttribute('href');
        if (href) {
            const cleanHref = href.toLowerCase();
            if ((path === '/' || path.endsWith('default.aspx')) && cleanHref.endsWith('default.aspx')) {
                link.classList.add('active');
            } else if (path.includes(cleanHref) && !cleanHref.endsWith('default.aspx')) {
                link.classList.add('active');
            }
        }
    });

    // 3. Current Date in Top Header
    const dateSpan = document.getElementById('topLiveDate');
    if (dateSpan) {
        dateSpan.textContent = new Date().toLocaleDateString('en-IN', {
            weekday: 'short',
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        });
    }

    // 4. Auto-dismiss Alert messages after 6 seconds
    setTimeout(function () {
        const alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            alert.classList.remove('show');
            setTimeout(function () { alert.remove(); }, 300);
        });
    }, 6000);
});

