(function () {
    const storageKey = "winterCart";
    const productMetaMap = {
        SP0001: { collection: "Arctic Edit", category: "Outerwear", badge: "Best seller", tagline: "Giữ ấm sâu, phom hiện đại", toneA: "#c8d5df", toneB: "#6c8295", style: "jacket" },
        SP0002: { collection: "City Luxe", category: "Leather", badge: "New drop", tagline: "Thanh lịch cho những buổi tối lạnh", toneA: "#d8c0ad", toneB: "#6d4639", style: "jacket" },
        SP0003: { collection: "Soft Layer", category: "Knitwear", badge: "Cozy fit", tagline: "Mềm nhẹ, dễ layer mỗi ngày", toneA: "#efe4d5", toneB: "#a68d76", style: "sweater" },
        SP0004: { collection: "Tailored Warmth", category: "Coat", badge: "Premium", tagline: "Phom coat sang và tối giản", toneA: "#d7d1cb", toneB: "#5d6873", style: "coat" },
        SP0005: { collection: "Street Comfort", category: "Hoodie", badge: "Daily wear", tagline: "Ấm áp, trẻ và dễ phối", toneA: "#cad8c4", toneB: "#5f7757", style: "hoodie" },
        SP0006: { collection: "Urban Layer", category: "Vest", badge: "Hot pick", tagline: "Gọn nhẹ cho nhịp sống thành phố", toneA: "#ece2d0", toneB: "#84745f", style: "vest" }
    };

    function escapeHtml(value) {
        return String(value ?? "").replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#39;");
    }

    function getFallbackMeta(productId) {
        const palette = [["#d2d9e5", "#6f7f98"], ["#eadfcf", "#93765c"], ["#d7e2d5", "#627d67"], ["#e4d5cb", "#7e6b63"]];
        const styles = ["jacket", "coat", "sweater", "vest"];
        const total = [...String(productId || "SP0000")].reduce((sum, char) => sum + char.charCodeAt(0), 0);
        const picked = palette[total % palette.length];
        return { collection: "Winter Capsule", category: "Seasonal", badge: "Featured", tagline: "Thiết kế mùa đông tối giản", toneA: picked[0], toneB: picked[1], style: styles[total % styles.length] };
    }

    function getProductMeta(productOrId) {
        const productId = typeof productOrId === "string" ? productOrId : productOrId?.sanPhamID;
        return productMetaMap[productId] || getFallbackMeta(productId);
    }

    function enrichProduct(product) {
        return { ...product, meta: getProductMeta(product) };
    }

    function buildArtwork(product, mode) {
        const meta = getProductMeta(product);
        const hoodMarkup = meta.style === "hoodie" ? '<span class="hood"></span>' : "";
        return `
            <div class="product-visual${mode === "large" ? " large" : ""}" style="--tone-a:${meta.toneA};--tone-b:${meta.toneB};">
                <span class="product-badge">${escapeHtml(meta.badge)}</span>
                <span class="visual-shape shape-one"></span>
                <span class="visual-shape shape-two"></span>
                <div class="garment" data-style="${escapeHtml(meta.style)}">
                    <span class="garment-body"></span>
                    ${hoodMarkup}
                    <span class="garment-accent"></span>
                </div>
            </div>`;
    }

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
        window.dispatchEvent(new CustomEvent("winterCartChanged", { detail: cart }));
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
            cart.push({ ...item, soLuong: safeQuantity });
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
        return cart.reduce((sum, item) => sum + Number(item.soLuong || 0), 0);
    }

    function getSubtotal(cart = getCart()) {
        return cart.reduce((sum, item) => sum + (Number(item.donGia || 0) * Number(item.soLuong || 0)), 0);
    }

    function formatCurrency(value) {
        return Number(value || 0).toLocaleString("vi-VN") + " đ";
    }

    function updateCartBadge(cart = getCart()) {
        const badge = document.getElementById("cartBadge");
        if (badge) {
            badge.textContent = getTotalQuantity(cart);
        }
    }

    function showToast(message, type = "info") {
        const container = document.getElementById("toastContainer");
        if (!container || !window.bootstrap) {
            return;
        }
        const bgClass = { success: "text-bg-success", danger: "text-bg-danger", warning: "text-bg-warning", info: "text-bg-dark" }[type] || "text-bg-dark";
        const toastElement = document.createElement("div");
        toastElement.className = `toast align-items-center border-0 ${bgClass}`;
        toastElement.setAttribute("role", "alert");
        toastElement.setAttribute("aria-live", "assertive");
        toastElement.setAttribute("aria-atomic", "true");
        toastElement.innerHTML = `<div class="d-flex"><div class="toast-body">${escapeHtml(message)}</div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button></div>`;
        container.appendChild(toastElement);
        const toast = new window.bootstrap.Toast(toastElement, { delay: 2600 });
        toastElement.addEventListener("hidden.bs.toast", function () { toastElement.remove(); });
        toast.show();
    }

    function isValidPhoneNumber(phoneNumber) {
        return /^(0|\+84)\d{9,10}$/.test((phoneNumber || "").replace(/\s+/g, ""));
    }

    async function fetchJson(url) {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Request failed: ${response.status}`);
        }
        return response.json();
    }

    async function fetchProducts() {
        const products = await fetchJson("/api/products");
        return Array.isArray(products) ? products.map(enrichProduct) : [];
    }

    async function fetchProduct(productId) {
        return enrichProduct(await fetchJson(`/api/products/${encodeURIComponent(productId)}`));
    }

    function getStockStatus(totalStock) {
        return totalStock <= 5 ? { label: "Sắp hết hàng", className: "status-pill low" } : { label: "Còn hàng", className: "status-pill" };
    }

    function renderProductCard(product) {
        const stock = getStockStatus(product.tongSoLuongTon);
        return `
            <article class="product-card">
                ${buildArtwork(product, "card")}
                <div class="product-info">
                    <div class="meta-row">
                        <span class="meta-tag">${escapeHtml(product.meta.collection)}</span>
                        <span class="${stock.className}">${escapeHtml(stock.label)}</span>
                    </div>
                    <h4>${escapeHtml(product.ten)}</h4>
                    <p class="meta-copy">${escapeHtml(product.meta.tagline)}</p>
                    <div class="price-line">
                        <div>
                            <div class="price-amount">${formatCurrency(product.giaThapNhat)}</div>
                            <div class="price-note">${escapeHtml(product.meta.category)} · ${product.tongSoLuongTon} sản phẩm khả dụng</div>
                        </div>
                    </div>
                    <div class="product-actions">
                        <a class="btn btn-dark-soft" href="/Home/Details?id=${encodeURIComponent(product.sanPhamID)}">Xem chi tiết</a>
                        <a class="btn btn-ghost-dark" href="/Home/Cart">Giỏ hàng</a>
                    </div>
                </div>
            </article>`;
    }

    window.winterCart = { getCart, saveCart, addItem, updateQuantity, removeItem, clear, getTotalQuantity, getSubtotal, formatCurrency, updateCartBadge, normalizeQuantity, showToast, isValidPhoneNumber };
    window.winterStore = { escapeHtml, getProductMeta, enrichProduct, buildArtwork, fetchProducts, fetchProduct, getStockStatus, renderProductCard };

    document.addEventListener("DOMContentLoaded", function () { updateCartBadge(); });
    window.addEventListener("winterCartChanged", function (event) { updateCartBadge(event.detail); });
})();
