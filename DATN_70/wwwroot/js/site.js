(function () {
    const storageKey = 'winterCart';

    function getCart() {
        try {
            const raw = localStorage.getItem(storageKey);
            const parsed = raw ? JSON.parse(raw) : [];
            return Array.isArray(parsed) ? parsed : [];
        } catch {
            return [];
        }
    }

    function notify(cart) {
        window.dispatchEvent(new CustomEvent('winterCartChanged', { detail: cart }));
    }

    function saveCart(cart) {
        localStorage.setItem(storageKey, JSON.stringify(cart));
        notify(cart);
    }

    function normalizeQuantity(quantity) {
        const parsed = Number.parseInt(quantity, 10);
        return Number.isNaN(parsed) || parsed < 1 ? 1 : parsed;
    }

    function addItem(item) {
        const cart = getCart();
        const existing = cart.find(x => x.chiTietSanPhamID === item.chiTietSanPhamID);
        const safeQuantity = normalizeQuantity(item.soLuong);

        if (existing) {
            existing.soLuong += safeQuantity;
        } else {
            cart.push({
                ...item,
                soLuong: safeQuantity
            });
        }

        saveCart(cart);
        return cart;
    }

    function updateQuantity(productDetailId, quantity) {
        const cart = getCart();
        const item = cart.find(x => x.chiTietSanPhamID === productDetailId);

        if (!item) {
            return cart;
        }

        item.soLuong = normalizeQuantity(quantity);
        saveCart(cart);
        return cart;
    }

    function removeItem(productDetailId) {
        const cart = getCart().filter(x => x.chiTietSanPhamID !== productDetailId);
        saveCart(cart);
        return cart;
    }

    function clear() {
        localStorage.removeItem(storageKey);
        notify([]);
    }

    function getTotalQuantity(cart = getCart()) {
        return cart.reduce((sum, item) => sum + item.soLuong, 0);
    }

    function getSubtotal(cart = getCart()) {
        return cart.reduce((sum, item) => sum + (item.donGia * item.soLuong), 0);
    }

    function formatCurrency(value) {
        return Number(value || 0).toLocaleString('vi-VN') + ' đ';
    }

    function updateCartBadge(cart = getCart()) {
        const badge = document.getElementById('cartBadge');
        if (!badge) {
            return;
        }

        badge.textContent = getTotalQuantity(cart);
    }

    function showToast(message, type = 'info') {
        const container = document.getElementById('toastContainer');
        if (!container || !window.bootstrap) {
            return;
        }

        const bgClass = {
            success: 'text-bg-success',
            danger: 'text-bg-danger',
            warning: 'text-bg-warning',
            info: 'text-bg-dark'
        }[type] || 'text-bg-dark';

        const toastElement = document.createElement('div');
        toastElement.className = `toast align-items-center border-0 ${bgClass}`;
        toastElement.setAttribute('role', 'alert');
        toastElement.setAttribute('aria-live', 'assertive');
        toastElement.setAttribute('aria-atomic', 'true');
        toastElement.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>`;

        container.appendChild(toastElement);

        const toast = new window.bootstrap.Toast(toastElement, { delay: 2500 });
        toastElement.addEventListener('hidden.bs.toast', function () {
            toastElement.remove();
        });
        toast.show();
    }

    function isValidPhoneNumber(phoneNumber) {
        return /^(0|\+84)\d{9,10}$/.test((phoneNumber || '').replace(/\s+/g, ''));
    }

    window.winterCart = {
        getCart,
        saveCart,
        addItem,
        updateQuantity,
        removeItem,
        clear,
        getTotalQuantity,
        getSubtotal,
        formatCurrency,
        updateCartBadge,
        normalizeQuantity,
        showToast,
        isValidPhoneNumber
    };

    document.addEventListener('DOMContentLoaded', function () {
        updateCartBadge();
    });

    window.addEventListener('winterCartChanged', function (event) {
        updateCartBadge(event.detail);
    });
})();
