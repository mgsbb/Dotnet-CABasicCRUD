// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleSidebar() {
    const layout = document.getElementById('app-layout');

    if (window.innerWidth >= 768) {
        // Desktop collapse
        layout.classList.toggle('sidebar-collapsed');
        console.log('if');
        layout.classList.toggle('sidebar-expanded');
    } else {
        // Mobile drawer
        console.log('else');
        layout.classList.toggle('sidebar-open');
    }
}
