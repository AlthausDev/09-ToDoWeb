// wwwroot/js/site.js
window.initializeBootstrapComponents = () => {
    console.log("Initializing Bootstrap components");

    // Re-initialize modal components
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        console.log("Initializing modal:", modal);
        const modalInstance = new bootstrap.Modal(modal);
        modalInstance.handleUpdate();
    });

    // Re-initialize toast components
    const toasts = document.querySelectorAll('.toast');
    toasts.forEach(toast => {
        console.log("Initializing toast:", toast);
        const toastInstance = new bootstrap.Toast(toast);
        toastInstance.show();
    });
};
