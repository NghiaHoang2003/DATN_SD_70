(function () {
    const cartKey = "winterStoreCartV3";
    const profileKey = "winterStoreProfileV3";
    const stylePhotos = {
        parka: { url: "https://images.pexels.com/photos/15246022/pexels-photo-15246022.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 14%" },
        jacket: { url: "https://images.pexels.com/photos/35295619/pexels-photo-35295619.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 12%" },
        coat: { url: "https://images.pexels.com/photos/36211191/pexels-photo-36211191.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 10%" },
        hoodie: { url: "https://images.pexels.com/photos/8346216/pexels-photo-8346216.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 10%" },
        sweater: { url: "https://images.pexels.com/photos/29491953/pexels-photo-29491953.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 10%" },
        vest: { url: "https://images.pexels.com/photos/5303811/pexels-photo-5303811.jpeg?auto=compress&cs=tinysrgb&w=1200", position: "center 12%" }
    };
    const metaMap = {
        SP0001: { collection: "Arctic Edit", parentCategory: "outerwear", parentLabel: "Áo khoác ngoài", category: "ao-phao", categoryLabel: "Áo phao", badge: "Sale", tagline: "Giữ ấm tốt, phom hiện đại", toneA: "#d8e1ea", toneB: "#7d94a9", style: "parka", popularity: 97, originalFactor: 1.22, icon: "bi-box-seam" },
        SP0002: { collection: "City Luxe", parentCategory: "outerwear", parentLabel: "Áo khoác ngoài", category: "ao-da", categoryLabel: "Áo da", badge: "Mới", tagline: "Thanh lịch cho ngày lạnh", toneA: "#eadbcf", toneB: "#6d4b3f", style: "jacket", popularity: 94, originalFactor: 1.18, icon: "bi-stars" },
        SP0003: { collection: "Soft Layer", parentCategory: "layering", parentLabel: "Áo len layer", category: "ao-len", categoryLabel: "Áo len", badge: "Best seller", tagline: "Mềm nhẹ, dễ phối hằng ngày", toneA: "#f0e5d8", toneB: "#a18b75", style: "sweater", popularity: 99, originalFactor: 1.15, icon: "bi-grid" },
        SP0004: { collection: "Tailored Warmth", parentCategory: "outerwear", parentLabel: "Áo khoác ngoài", category: "mang-to", categoryLabel: "Măng tô", badge: "Premium", tagline: "Phom coat sang và tối giản", toneA: "#e6dfd8", toneB: "#727d88", style: "coat", popularity: 90, originalFactor: 1.2, icon: "bi-bag" },
        SP0005: { collection: "Street Comfort", parentCategory: "heattech", parentLabel: "Áo giữ nhiệt", category: "giu-nhiet", categoryLabel: "Áo giữ nhiệt", badge: "Hot", tagline: "Ấm áp, trẻ và dễ phối", toneA: "#d4dfd1", toneB: "#667b5f", style: "hoodie", popularity: 93, originalFactor: 1.14, icon: "bi-shield-check" },
        SP0006: { collection: "Urban Layer", parentCategory: "short-jacket", parentLabel: "Áo khoác ngắn", category: "gile", categoryLabel: "Gile phao", badge: "Sale", tagline: "Gọn nhẹ cho nhịp sống thành phố", toneA: "#efe4d2", toneB: "#84755f", style: "vest", popularity: 91, originalFactor: 1.16, icon: "bi-handbag" },
        SP0007: { collection: "Metro Ease", parentCategory: "short-jacket", parentLabel: "Áo khoác ngắn", category: "ao-khoac-ngan", categoryLabel: "Áo khoác ngắn", badge: "Mới", tagline: "Dáng gọn, mặc đẹp mỗi ngày", toneA: "#d9dee7", toneB: "#69788b", style: "jacket", popularity: 88, originalFactor: 1.15, icon: "bi-stars" },
        SP0008: { collection: "Soft Layer", parentCategory: "layering", parentLabel: "Áo len layer", category: "ao-len", categoryLabel: "Áo len", badge: "Premium", tagline: "Len mịn, layer nhẹ và ấm", toneA: "#ede2d7", toneB: "#a78974", style: "sweater", popularity: 92, originalFactor: 1.14, icon: "bi-grid" },
        SP0009: { collection: "Core Base", parentCategory: "heattech", parentLabel: "Áo giữ nhiệt", category: "giu-nhiet", categoryLabel: "Áo giữ nhiệt", badge: "Sale", tagline: "Lớp trong gọn nhẹ, co giãn tốt", toneA: "#dfe3de", toneB: "#808a7f", style: "hoodie", popularity: 95, originalFactor: 1.13, icon: "bi-shield-check" },
        SP0010: { collection: "Snow Field", parentCategory: "outerwear", parentLabel: "Áo khoác ngoài", category: "ao-phao", categoryLabel: "Áo phao", badge: "Hot", tagline: "Parka chống gió cho ngày rất lạnh", toneA: "#d5deea", toneB: "#6f86a2", style: "parka", popularity: 96, originalFactor: 1.19, icon: "bi-box-seam" },
        SP0011: { collection: "Layer Studio", parentCategory: "layering", parentLabel: "Áo len layer", category: "ao-len", categoryLabel: "Áo len", badge: "Mới", tagline: "Cardigan mềm cho set layer linh hoạt", toneA: "#eadfda", toneB: "#8d7469", style: "sweater", popularity: 87, originalFactor: 1.14, icon: "bi-grid" },
        SP0012: { collection: "Street Frost", parentCategory: "short-jacket", parentLabel: "Áo khoác ngắn", category: "ao-khoac-ngan", categoryLabel: "Áo khoác ngắn", badge: "Best seller", tagline: "Bomber trẻ, gọn và dễ phối", toneA: "#dddfe4", toneB: "#70757f", style: "jacket", popularity: 94, originalFactor: 1.17, icon: "bi-stars" }
    };
    const categories = [
        { key: "outerwear", label: "Áo khoác ngoài", icon: "bi-box-seam", description: "Tủ đồ mùa lạnh" },
        { key: "short-jacket", label: "Áo khoác ngắn", icon: "bi-stars", description: "Dễ mặc hằng ngày" },
        { key: "layering", label: "Áo len layer", icon: "bi-grid", description: "Mềm và dễ phối" },
        { key: "heattech", label: "Áo giữ nhiệt", icon: "bi-shield-check", description: "Giữ ấm nhẹ" }
    ];
    const provinces = [
        { value: "ha-noi", label: "Hà Nội", districts: [{ value: "cau-giay", label: "Cầu Giấy", wards: ["Dịch Vọng", "Quan Hoa", "Nghĩa Tân"] }, { value: "dong-da", label: "Đống Đa", wards: ["Láng Hạ", "Ô Chợ Dừa", "Văn Chương"] }] },
        { value: "hai-phong", label: "Hải Phòng", districts: [{ value: "ngo-quyen", label: "Ngô Quyền", wards: ["Máy Chai", "Lạch Tray", "Đông Khê"] }, { value: "le-chan", label: "Lê Chân", wards: ["An Biên", "Dư Hàng", "Kênh Dương"] }] },
        { value: "tp-hcm", label: "TP. Hồ Chí Minh", districts: [{ value: "quan-1", label: "Quận 1", wards: ["Bến Nghé", "Đa Kao", "Nguyễn Cư Trinh"] }, { value: "binh-thanh", label: "Bình Thạnh", wards: ["Phường 1", "Phường 13", "Phường 25"] }] }
    ];
    const shippingMethods = [
        { code: "vnpost", label: "VietNam Post", fee: 20000, note: "Giao tiêu chuẩn toàn quốc." },
        { code: "express", label: "Giao nhanh tiết kiệm", fee: 30000, note: "Ưu tiên khu vực nội thành." },
        { code: "freeship", label: "Freeship đơn trên 500.000đ", fee: 0, note: "Tự động áp dụng nếu đủ điều kiện.", minimum: 500000 }
    ];
    const paymentMethods = [
        { code: "vnpay", label: "Ví điện tử/VNPAY", note: "Thanh toán online qua cổng mô phỏng." },
        { code: "atm", label: "Online banking", note: "Chuyển khoản ngân hàng nội địa." },
        { code: "cod", label: "Thanh toán khi nhận hàng (COD)", note: "Thanh toán cho đơn vị giao hàng khi nhận." }
    ];
    const coupons = [
        { code: "WINTER10", label: "Giảm 10%", type: "percent", value: 10, minSubtotal: 600000 },
        { code: "NEW50", label: "Giảm 50.000đ", type: "amount", value: 50000, minSubtotal: 500000 },
        { code: "FREESHIP", label: "Miễn phí vận chuyển", type: "shipping", value: 0, minSubtotal: 300000 }
    ];

    const escapeHtml = value => String(value ?? "").replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\"/g, "&quot;").replace(/'/g, "&#39;");
    const formatCurrency = value => Number(value || 0).toLocaleString("vi-VN") + " đ";
    const clamp = (value, min, max) => Math.min(Math.max(value, min), max);
    const getMeta = productOrId => metaMap[typeof productOrId === "string" ? productOrId : productOrId?.sanPhamID] || { collection: "Winter Capsule", parentCategory: "outerwear", parentLabel: "Áo khoác ngoài", category: "ao-phao", categoryLabel: "Áo phao", badge: "Hot", tagline: "Thiết kế tối giản cho mùa lạnh", toneA: "#ddd8cd", toneB: "#887a69", style: "jacket", popularity: 80, originalFactor: 1.12, icon: "bi-box-seam" };
    const getCoupon = code => coupons.find(item => item.code.toLowerCase() === String(code || "").trim().toLowerCase()) || null;
    const isValidPhoneNumber = phone => /^(0|\+84)\d{9,10}$/.test((phone || "").replace(/\s+/g, ""));
    const getCartState = () => {
        try {
            const parsed = JSON.parse(localStorage.getItem(cartKey) || "{}");
            return { items: Array.isArray(parsed.items) ? parsed.items : [], note: parsed.note || "", couponCode: parsed.couponCode || "", shippingCode: parsed.shippingCode || "vnpost", paymentCode: parsed.paymentCode || "cod" };
        } catch {
            return { items: [], note: "", couponCode: "", shippingCode: "vnpost", paymentCode: "cod" };
        }
    };
    const saveCartState = state => {
        localStorage.setItem(cartKey, JSON.stringify(state));
        document.dispatchEvent(new CustomEvent("winterCartChanged", { detail: state }));
    };
    const getProfile = () => { try { return JSON.parse(localStorage.getItem(profileKey) || "{}"); } catch { return {}; } };
    const saveProfile = profile => localStorage.setItem(profileKey, JSON.stringify(profile));
    const getTotalQuantity = (state = getCartState()) => state.items.reduce((sum, item) => sum + Number(item.soLuong || 0), 0);
    const getSubtotal = (state = getCartState()) => state.items.reduce((sum, item) => sum + Number(item.donGia || 0) * Number(item.soLuong || 0), 0);

    function buildArtwork(product, mode) {
        const meta = getMeta(product);
        const photo = stylePhotos[meta.style];
        const modeClass = mode === "large" ? " large" : mode === "compact" ? " compact" : "";
        if (photo) {
            return `<div class="product-visual photo-mode${modeClass}" style="--tone-a:${meta.toneA};--tone-b:${meta.toneB};"><span class="product-badge">${escapeHtml(meta.badge)}</span><span class="visual-shape shape-one"></span><span class="visual-shape shape-two"></span><div class="product-photo-layer"><img class="product-photo" src="${photo.url}" alt="${escapeHtml(product?.ten || meta.categoryLabel)}" loading="lazy" style="object-position:${photo.position};" /></div></div>`;
        }
        const hoodMarkup = meta.style === "hoodie" ? '<span class="hood"></span>' : "";
        const collarMarkup = meta.style === "coat" || meta.style === "jacket" || meta.style === "parka" ? '<span class="garment-collar collar-left"></span><span class="garment-collar collar-right"></span>' : "";
        const pocketMarkup = meta.style === "sweater"
            ? ""
            : '<span class="garment-pocket pocket-left"></span><span class="garment-pocket pocket-right"></span>';
        const seamMarkup = meta.style === "vest"
            ? '<span class="garment-placket short"></span>'
            : '<span class="garment-placket"></span><span class="garment-hem"></span>';
        return `<div class="product-visual${modeClass}" style="--tone-a:${meta.toneA};--tone-b:${meta.toneB};"><span class="product-badge">${escapeHtml(meta.badge)}</span><span class="visual-shape shape-one"></span><span class="visual-shape shape-two"></span><div class="garment" data-style="${escapeHtml(meta.style)}"><span class="garment-body"></span>${hoodMarkup}${collarMarkup}${seamMarkup}${pocketMarkup}<span class="garment-accent"></span></div></div>`;
    }

    function enrichProduct(product) {
        const meta = getMeta(product);
        const giaThapNhat = Number(product.giaThapNhat || 0);
        const giaGoc = Math.round(giaThapNhat * meta.originalFactor);
        return { ...product, meta, giaGoc, phanTramGiam: giaGoc > giaThapNhat ? Math.round((1 - giaThapNhat / giaGoc) * 100) : 0 };
    }

    async function fetchJson(url) {
        const response = await fetch(url);
        if (!response.ok) throw new Error(`Request failed: ${response.status}`);
        return response.json();
    }

    async function fetchProducts() {
        const products = await fetchJson("/api/products");
        return Array.isArray(products) ? products.map(enrichProduct) : [];
    }

    async function fetchProduct(productId) {
        const product = enrichProduct(await fetchJson(`/api/products/${encodeURIComponent(productId)}`));
        product.bienThe = Array.isArray(product.bienThe) ? product.bienThe.map(item => ({ ...item, sizeLabel: String(item.tenKichCo || "").replace(/^Size\s*/i, "").trim() })) : [];
        if (product.bienThe.length) {
            product.giaThapNhat = Math.min(...product.bienThe.map(item => Number(item.giaNiemYet || 0)));
            product.tongSoLuongTon = product.bienThe.reduce((sum, item) => sum + Number(item.soLuongTon || 0), 0);
            product.giaGoc = Math.round(product.giaThapNhat * product.meta.originalFactor);
            product.phanTramGiam = product.giaGoc > product.giaThapNhat ? Math.round((1 - product.giaThapNhat / product.giaGoc) * 100) : 0;
        }
        return product;
    }

    function renderProductCard(product) {
        const detailUrl = `/Home/Details?id=${encodeURIComponent(product.sanPhamID)}`;
        return `<article class="product-card"><a class="product-card-link" href="${detailUrl}" aria-label="Xem chi tiết ${escapeHtml(product.ten)}">${buildArtwork(product, "card")}<div class="product-info"><div class="price-stack"><span class="sale-badge">-${product.phanTramGiam}%</span><span class="muted-copy">${escapeHtml(product.meta.categoryLabel)}</span></div><span class="product-name">${escapeHtml(product.ten)}</span><div class="product-copy">${escapeHtml(product.meta.tagline)}</div><div class="price-stack"><strong class="price-sale">${formatCurrency(product.giaThapNhat)}</strong><span class="price-original">${formatCurrency(product.giaGoc)}</span></div></div></a></article>`;
    }

    function showToast(message, type = "info") {
        const container = document.getElementById("toastContainer");
        if (!container || !window.bootstrap) return;
        const bgClass = { success: "text-bg-success", danger: "text-bg-danger", warning: "text-bg-warning", info: "text-bg-dark" }[type] || "text-bg-dark";
        const toastElement = document.createElement("div");
        toastElement.className = `toast align-items-center border-0 ${bgClass}`;
        toastElement.innerHTML = `<div class="d-flex"><div class="toast-body">${escapeHtml(message)}</div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button></div>`;
        container.appendChild(toastElement);
        const toast = new window.bootstrap.Toast(toastElement, { delay: 2600 });
        toastElement.addEventListener("hidden.bs.toast", () => toastElement.remove());
        toast.show();
    }
    function getShipping(state, subtotal) {
        const picked = shippingMethods.find(item => item.code === state.shippingCode) || shippingMethods[0];
        return picked.minimum && subtotal < picked.minimum ? shippingMethods[0] : picked;
    }

    function calculateTotals(state = getCartState()) {
        const subtotal = getSubtotal(state);
        const shipping = getShipping(state, subtotal);
        const coupon = getCoupon(state.couponCode);
        let discount = 0;
        if (coupon && subtotal >= coupon.minSubtotal) {
            if (coupon.type === "percent") discount = Math.round(subtotal * coupon.value / 100);
            if (coupon.type === "amount") discount = coupon.value;
            if (coupon.type === "shipping") discount = shipping.fee;
        }
        discount = Math.min(discount, subtotal + shipping.fee);
        return { subtotal, shipping, coupon, discount, total: Math.max(subtotal + shipping.fee - discount, 0) };
    }

    function updateCartPreview(state = getCartState()) {
        const badge = document.getElementById("cartBadge");
        const count = document.getElementById("cartPreviewCount");
        const itemsNode = document.getElementById("cartPreviewItems");
        if (badge) badge.textContent = getTotalQuantity(state);
        if (count) count.textContent = `${getTotalQuantity(state)} sản phẩm`;
        if (!itemsNode) return;
        itemsNode.innerHTML = state.items.length ? state.items.slice(0, 4).map(item => `<article class="cart-preview-item"><div class="thumb-wrap">${buildArtwork({ sanPhamID: item.sanPhamID }, "compact")}</div><div><strong>${escapeHtml(item.tenSanPham)}</strong><div class="item-meta">${escapeHtml(item.phanLoai)} · SL ${item.soLuong}</div><div class="price-sale">${formatCurrency(item.donGia * item.soLuong)}</div></div></article>`).join("") : '<div class="cart-preview-empty">Giỏ hàng đang trống.</div>';
    }

    function setSearchResults(products, keyword) {
        const node = document.getElementById("headerSearchResults");
        if (!node) return;
        const term = String(keyword || "").trim().toLowerCase();
        if (!term) {
            node.innerHTML = '<div class="header-search-empty">Nhập tên sản phẩm để tìm.</div>';
            return;
        }
        const matches = products.filter(product => product.ten.toLowerCase().includes(term) || product.meta.categoryLabel.toLowerCase().includes(term)).slice(0, 5);
        node.innerHTML = matches.length ? matches.map(product => `<a class="header-search-item" href="/Home/Details?id=${encodeURIComponent(product.sanPhamID)}"><div class="thumb-wrap">${buildArtwork(product, "compact")}</div><div><strong>${escapeHtml(product.ten)}</strong><div class="item-meta">${escapeHtml(product.meta.parentLabel)}</div><div class="price-sale">${formatCurrency(product.giaThapNhat)}</div></div></a>`).join("") : '<div class="header-search-empty">Không tìm thấy sản phẩm phù hợp.</div>';
    }

    function addItemToCart(product, variant, quantity) {
        const state = getCartState();
        const safeQuantity = clamp(Number.parseInt(quantity, 10) || 1, 1, Math.min(20, Number(variant.soLuongTon || 1)));
        const existing = state.items.find(item => item.chiTietSanPhamID === variant.chiTietSanPhamID);
        if (existing) existing.soLuong = clamp(existing.soLuong + safeQuantity, 1, Math.min(20, existing.tonKho || 20));
        else state.items.push({ sanPhamID: product.sanPhamID, chiTietSanPhamID: variant.chiTietSanPhamID, tenSanPham: product.ten, phanLoai: `${variant.tenMau} / ${variant.sizeLabel}`, soLuong: safeQuantity, donGia: Number(variant.giaNiemYet || 0), tonKho: Number(variant.soLuongTon || 0) });
        saveCartState(state);
    }

    function initHeader(productsPromise) {
        const bindHoverDropdown = selector => {
            document.querySelectorAll(selector).forEach(node => {
                let closeTimer = null;
                const open = () => {
                    if (closeTimer) window.clearTimeout(closeTimer);
                    node.classList.add("is-open");
                };
                const close = () => {
                    if (closeTimer) window.clearTimeout(closeTimer);
                    closeTimer = window.setTimeout(() => node.classList.remove("is-open"), 140);
                };
                node.addEventListener("mouseenter", open);
                node.addEventListener("mouseleave", close);
            });
        };

        bindHoverDropdown(".has-submenu");
        bindHoverDropdown(".account-menu");
        bindHoverDropdown(".cart-menu");

        const wrapper = document.querySelector(".header-search");
        const toggle = document.getElementById("headerSearchToggle");
        const input = document.getElementById("headerSearchInput");
        if (!wrapper || !toggle || !input) return;
        let searchCloseTimer = null;
        const openSearch = () => {
            if (searchCloseTimer) window.clearTimeout(searchCloseTimer);
            wrapper.classList.add("is-open");
        };
        const closeSearch = () => {
            if (searchCloseTimer) window.clearTimeout(searchCloseTimer);
            searchCloseTimer = window.setTimeout(() => wrapper.classList.remove("is-open"), 140);
        };
        wrapper.addEventListener("mouseenter", openSearch);
        wrapper.addEventListener("mouseleave", closeSearch);
        toggle.addEventListener("click", () => {
            wrapper.classList.toggle("is-open");
            if (wrapper.classList.contains("is-open")) input.focus();
        });
        document.addEventListener("click", event => { if (!wrapper.contains(event.target)) wrapper.classList.remove("is-open"); });
        input.addEventListener("input", async () => setSearchResults(await productsPromise, input.value));
    }

    function initHome(productsPromise) {
        const page = document.getElementById("homePage");
        if (!page) return;
        productsPromise.then(products => {
            page.querySelectorAll(".mini-art").forEach(node => { node.innerHTML = buildArtwork({ sanPhamID: node.dataset.product }, "compact"); });
            document.getElementById("homeCategories").innerHTML = categories.map(category => `<a class="category-card" href="/Home/Products?category=${category.key}"><div class="category-icon"><i class="bi ${escapeHtml(category.icon)}"></i></div><strong>${escapeHtml(category.label)}</strong><span>${escapeHtml(category.description)}</span></a>`).join("");
            document.getElementById("homeFeaturedProducts").innerHTML = products.slice().sort((a, b) => b.phanTramGiam - a.phanTramGiam).slice(0, 4).map(renderProductCard).join("");
            const tabsNode = document.getElementById("homeTabs");
            const listNode = document.getElementById("homeTabProducts");
            let active = categories[0].key;
            const render = () => {
                tabsNode.innerHTML = categories.map(category => `<button type="button" class="chip-button ${active === category.key ? "active" : ""}" data-key="${category.key}">${escapeHtml(category.label)}</button>`).join("");
                listNode.innerHTML = products.filter(item => item.meta.parentCategory === active).slice(0, 8).map(renderProductCard).join("");
            };
            tabsNode.addEventListener("click", event => {
                const button = event.target.closest("[data-key]");
                if (!button) return;
                active = button.dataset.key;
                render();
            });
            render();
        }).catch(() => {
            document.getElementById("homeFeaturedProducts").innerHTML = '<div class="empty-state">Không thể tải sản phẩm.</div>';
            document.getElementById("homeTabProducts").innerHTML = '<div class="empty-state">Không thể tải sản phẩm.</div>';
        });
    }
    function initProducts(productsPromise) {
        const page = document.getElementById("productsPage");
        if (!page) return;
        const query = new URLSearchParams(window.location.search);
        const state = { parent: query.get("category") || "all", sub: query.get("subcategory") || "all", keyword: "", sort: "featured", min: 0, max: 3000000, size: "all", page: 1, pageSize: 8 };
        productsPromise.then(async products => {
            const details = await Promise.all(products.map(item => fetchProduct(item.sanPhamID).catch(() => item)));
            const parentSelect = document.getElementById("filterParentCategory");
            const subSelect = document.getElementById("filterSubCategory");
            const searchInput = document.getElementById("catalogSearchInput");
            const sortSelect = document.getElementById("catalogSortSelect");
            const minRange = document.getElementById("priceRangeMin");
            const maxRange = document.getElementById("priceRangeMax");
            const sizeFilters = document.getElementById("sizeFilters");
            const subTabs = document.getElementById("catalogSubTabs");
            const grid = document.getElementById("catalogGrid");
            const countNode = document.getElementById("catalogCount");
            const summaryNode = document.getElementById("catalogSummary");
            const paginationNode = document.getElementById("catalogPagination");
            const setRangeText = () => {
                document.getElementById("priceRangeMinLabel").textContent = formatCurrency(state.min);
                document.getElementById("priceRangeMaxLabel").textContent = formatCurrency(state.max);
            };
            const getSubs = () => [...new Set(details.filter(item => state.parent === "all" || item.meta.parentCategory === state.parent).map(item => item.meta.category))];
            const renderSelects = () => {
                parentSelect.innerHTML = ['<option value="all">Tất cả</option>'].concat(categories.map(item => `<option value="${item.key}">${escapeHtml(item.label)}</option>`)).join("");
                parentSelect.value = state.parent;
                const subs = getSubs();
                if (!subs.includes(state.sub)) state.sub = "all";
                subSelect.innerHTML = ['<option value="all">Tất cả</option>'].concat(subs.map(key => `<option value="${key}">${escapeHtml((details.find(item => item.meta.category === key) || {}).meta?.categoryLabel || key)}</option>`)).join("");
                subSelect.value = state.sub;
                subTabs.innerHTML = ['<button type="button" class="chip-button ' + (state.sub === "all" ? "active" : "") + '" data-sub="all">Tất cả</button>'].concat(subs.map(key => `<button type="button" class="chip-button ${state.sub === key ? "active" : ""}" data-sub="${key}">${escapeHtml((details.find(item => item.meta.category === key) || {}).meta?.categoryLabel || key)}</button>`)).join("");
                sizeFilters.innerHTML = ["all", "S", "M", "L", "XL"].map(size => `<button type="button" class="chip-button ${state.size === size ? "active" : ""}" data-size="${size}">${size === "all" ? "Tất cả" : size}</button>`).join("");
            };
            const getFiltered = () => details.filter(item => {
                const sizeMatch = state.size === "all" || (item.bienThe || []).some(variant => variant.sizeLabel === state.size);
                return (state.parent === "all" || item.meta.parentCategory === state.parent) && (state.sub === "all" || item.meta.category === state.sub) && (!state.keyword || item.ten.toLowerCase().includes(state.keyword) || item.meta.categoryLabel.toLowerCase().includes(state.keyword)) && item.giaThapNhat >= state.min && item.giaThapNhat <= state.max && sizeMatch;
            }).sort((a, b) => {
                if (state.sort === "name-asc") return a.ten.localeCompare(b.ten, "vi");
                if (state.sort === "name-desc") return b.ten.localeCompare(a.ten, "vi");
                if (state.sort === "price-asc") return a.giaThapNhat - b.giaThapNhat;
                if (state.sort === "price-desc") return b.giaThapNhat - a.giaThapNhat;
                return b.meta.popularity - a.meta.popularity;
            });
            const render = () => {
                renderSelects();
                setRangeText();
                const filtered = getFiltered();
                const pageCount = filtered.length ? Math.ceil(filtered.length / state.pageSize) : 0;
                state.page = pageCount ? clamp(state.page, 1, pageCount) : 1;
                const pageItems = filtered.slice((state.page - 1) * state.pageSize, state.page * state.pageSize);
                countNode.textContent = filtered.length;
                summaryNode.textContent = filtered.length ? `Đang xem ${state.page}/${pageCount}` : "Không có sản phẩm phù hợp.";
                grid.innerHTML = pageItems.length ? pageItems.map(renderProductCard).join("") : '<div class="empty-state">Không có sản phẩm phù hợp.</div>';
                paginationNode.innerHTML = pageCount > 1
                    ? Array.from({ length: pageCount }, (_, i) => `<button type="button" class="pagination-chip ${state.page === i + 1 ? "active" : ""}" data-page="${i + 1}">${i + 1}</button>`).join("")
                    : "";
            };
            parentSelect.addEventListener("change", () => { state.parent = parentSelect.value; state.sub = "all"; state.page = 1; render(); });
            subSelect.addEventListener("change", () => { state.sub = subSelect.value; state.page = 1; render(); });
            searchInput.addEventListener("input", () => { state.keyword = searchInput.value.trim().toLowerCase(); state.page = 1; render(); });
            sortSelect.addEventListener("change", () => { state.sort = sortSelect.value; render(); });
            minRange.addEventListener("input", () => { state.min = Math.min(Number(minRange.value), state.max); minRange.value = state.min; render(); });
            maxRange.addEventListener("input", () => { state.max = Math.max(Number(maxRange.value), state.min); maxRange.value = state.max; render(); });
            sizeFilters.addEventListener("click", event => { const button = event.target.closest("[data-size]"); if (!button) return; state.size = button.dataset.size; state.page = 1; render(); });
            subTabs.addEventListener("click", event => { const button = event.target.closest("[data-sub]"); if (!button) return; state.sub = button.dataset.sub; state.page = 1; render(); });
            paginationNode.addEventListener("click", event => { const button = event.target.closest("[data-page]"); if (!button) return; state.page = Number(button.dataset.page); render(); });
            sortSelect.value = state.sort;
            render();
        }).catch(() => {
            document.getElementById("catalogGrid").innerHTML = '<div class="empty-state">Không thể tải danh sách sản phẩm.</div>';
            document.getElementById("catalogSummary").textContent = "Không thể tải dữ liệu.";
        });
    }

    function getDefaultVariant(product) {
        const whiteS = product.bienThe.find(item => /kem|trắng|white/i.test(item.tenMau) && item.sizeLabel === "S");
        return whiteS || product.bienThe.slice().sort((a, b) => a.giaNiemYet - b.giaNiemYet)[0] || null;
    }

    function initDetail(productsPromise) {
        const page = document.getElementById("productDetailPage");
        if (!page) return;
        const productId = page.dataset.productId;
        let product;
        let selectedColor = "";
        let selectedSize = "";
        let activeTab = "description";
        const tabContent = {
            description: current => `<p>${escapeHtml(current.moTa || "Sản phẩm được tối ưu cho nhu cầu mặc mùa đông hằng ngày.")}</p><ul><li>Giá hiển thị đổi theo màu và size.</li><li>Tối đa 20 sản phẩm cho mỗi biến thể trong một đơn hàng.</li><li>Chỉ thay đổi số lượng ở giỏ hàng, muốn đổi màu hoặc size cần quay lại trang chi tiết.</li></ul>`,
            returns: () => `<ul><li>Đổi size trong 7 ngày nếu sản phẩm còn nguyên tem.</li><li>Không đổi trực tiếp màu và size trong giỏ hàng.</li><li>Đơn đổi trả phụ thuộc tồn kho thời điểm xử lý.</li></ul>`,
            warranty: () => `<ul><li>Thông tin khách hàng chỉ dùng để xử lý đơn hàng.</li><li>Dữ liệu checkout gần nhất được lưu cục bộ để tự điền lại.</li><li>Thanh toán online trong bản demo là mô phỏng giao diện.</li></ul>`
        };
        const getSelected = () => product.bienThe.find(item => item.tenMau === selectedColor && item.sizeLabel === selectedSize) || null;
        const renderTabs = () => {
            page.querySelectorAll(".detail-tab").forEach(button => button.classList.toggle("active", button.dataset.tab === activeTab));
            document.getElementById("detailTabContent").innerHTML = tabContent[activeTab](product);
        };
        const renderVariant = () => {
            const variant = getSelected();
            const colors = [...new Set(product.bienThe.map(item => item.tenMau))];
            const sizes = [...new Set(product.bienThe.filter(item => item.tenMau === selectedColor).map(item => item.sizeLabel))];
            document.getElementById("detailColorOptions").innerHTML = colors.map(color => `<button type="button" class="option-button ${selectedColor === color ? "active" : ""}" data-color="${escapeHtml(color)}">${escapeHtml(color)}</button>`).join("");
            document.getElementById("detailSizeOptions").innerHTML = sizes.map(size => `<button type="button" class="option-button ${selectedSize === size ? "active" : ""}" data-size="${size}">${escapeHtml(size)}</button>`).join("");
            const qty = document.getElementById("detailQuantityInput");
            const add = document.getElementById("detailAddToCart");
            const buy = document.getElementById("detailBuyNow");
            if (!variant) {
                document.getElementById("detailPrice").textContent = "Liên hệ";
                document.getElementById("detailOriginalPrice").textContent = "";
                document.getElementById("detailDiscountBadge").textContent = "";
                document.getElementById("detailStockNote").textContent = "Biến thể đã chọn hiện không khả dụng.";
                add.disabled = true;
                buy.disabled = true;
                return;
            }
            const giaGoc = Math.round(variant.giaNiemYet * product.meta.originalFactor);
            document.getElementById("detailPrice").textContent = formatCurrency(variant.giaNiemYet);
            document.getElementById("detailOriginalPrice").textContent = formatCurrency(giaGoc);
            document.getElementById("detailDiscountBadge").textContent = `-${Math.round((1 - variant.giaNiemYet / giaGoc) * 100)}%`;
            document.getElementById("detailVariantSummary").textContent = `Đã chọn ${variant.tenMau} / ${variant.sizeLabel}. Giá thay đổi theo biến thể này.`;
            document.getElementById("detailStockNote").textContent = `Còn ${variant.soLuongTon} sản phẩm · Tối đa mỗi đơn 20 sản phẩm.`;
            qty.max = Math.min(20, variant.soLuongTon);
            qty.value = clamp(Number(qty.value) || 1, 1, Math.max(1, Math.min(20, variant.soLuongTon)));
            add.disabled = variant.soLuongTon < 1;
            buy.disabled = variant.soLuongTon < 1;
        };
        fetchProduct(productId).then(detail => {
            product = detail;
            const variant = getDefaultVariant(product);
            if (!variant) {
                document.getElementById("detailArtwork").innerHTML = '<div class="empty-state">Sản phẩm hiện chưa có biến thể để bán.</div>';
                return;
            }
            selectedColor = variant.tenMau;
            selectedSize = variant.sizeLabel;
            document.getElementById("detailArtwork").innerHTML = buildArtwork(product, "large");
            document.getElementById("detailGalleryThumbs").innerHTML = [1, 2, 3, 4].map(() => `<div class="mini-art">${buildArtwork(product, "compact")}</div>`).join("");
            document.getElementById("detailTitle").textContent = product.ten.toUpperCase();
            document.getElementById("detailBreadcrumb").textContent = `Winter Shop / ${product.meta.parentLabel} / ${product.meta.categoryLabel}`;
            renderVariant();
            renderTabs();
            productsPromise.then(products => {
                const sameCategory = products.filter(item => item.sanPhamID !== product.sanPhamID && item.meta.category === product.meta.category);
                const sameParent = products.filter(item => item.sanPhamID !== product.sanPhamID && item.meta.parentCategory === product.meta.parentCategory && !sameCategory.some(x => x.sanPhamID === item.sanPhamID));
                document.getElementById("relatedProducts").innerHTML = [...sameCategory, ...sameParent].slice(0, 4).map(renderProductCard).join("");
            });
        }).catch(() => {
            document.getElementById("detailArtwork").innerHTML = '<div class="empty-state">Không thể tải sản phẩm.</div>';
        });
        document.getElementById("detailColorOptions").addEventListener("click", event => {
            const button = event.target.closest("[data-color]");
            if (!button || !product) return;
            selectedColor = button.dataset.color;
            const sizes = [...new Set(product.bienThe.filter(item => item.tenMau === selectedColor).map(item => item.sizeLabel))];
            if (!sizes.includes(selectedSize)) selectedSize = sizes[0];
            renderVariant();
        });
        document.getElementById("detailSizeOptions").addEventListener("click", event => {
            const button = event.target.closest("[data-size]");
            if (!button) return;
            selectedSize = button.dataset.size;
            renderVariant();
        });
        page.querySelector(".detail-tabs").addEventListener("click", event => {
            const button = event.target.closest("[data-tab]");
            if (!button || !product) return;
            activeTab = button.dataset.tab;
            renderTabs();
        });
        document.getElementById("detailDecreaseQty").addEventListener("click", () => {
            const input = document.getElementById("detailQuantityInput");
            input.value = clamp((Number(input.value) || 1) - 1, 1, Number(input.max) || 20);
        });
        document.getElementById("detailIncreaseQty").addEventListener("click", () => {
            const input = document.getElementById("detailQuantityInput");
            input.value = clamp((Number(input.value) || 1) + 1, 1, Number(input.max) || 20);
        });
        document.getElementById("detailAddToCart").addEventListener("click", () => {
            const variant = product ? getSelected() : null;
            if (!variant) return showToast("Vui lòng chọn màu và kích cỡ khả dụng.", "warning");
            addItemToCart(product, variant, document.getElementById("detailQuantityInput").value);
            showToast("Đã thêm sản phẩm vào giỏ hàng.", "success");
        });
        document.getElementById("detailBuyNow").addEventListener("click", () => {
            const variant = product ? getSelected() : null;
            if (!variant) return showToast("Vui lòng chọn màu và kích cỡ khả dụng.", "warning");
            addItemToCart(product, variant, document.getElementById("detailQuantityInput").value);
            window.location.href = "/Home/Cart";
        });
    }
    function initCart() {
        const page = document.getElementById("cartPage");
        if (!page) return;
        const container = document.getElementById("cartItemsContainer");
        const noteInput = document.getElementById("cartOrderNote");
        const checkoutLink = document.getElementById("cartCheckoutLink");
        const render = () => {
            const state = getCartState();
            const totals = calculateTotals(state);
            document.getElementById("cartHeadCount").textContent = getTotalQuantity(state);
            document.getElementById("cartSubtotalAmount").textContent = formatCurrency(totals.subtotal);
            document.getElementById("cartDiscountAmount").textContent = `- ${formatCurrency(totals.discount)}`;
            document.getElementById("cartTotalAmount").textContent = formatCurrency(totals.total);
            noteInput.value = state.note;
            checkoutLink.style.pointerEvents = state.items.length ? "auto" : "none";
            checkoutLink.style.opacity = state.items.length ? "1" : "0.6";
            container.innerHTML = state.items.length ? state.items.map(item => `<article class="cart-item"><div class="thumb-wrap">${buildArtwork({ sanPhamID: item.sanPhamID }, "compact")}</div><div><a class="product-name" href="/Home/Details?id=${encodeURIComponent(item.sanPhamID)}">${escapeHtml(item.tenSanPham)}</a><div class="item-meta">${escapeHtml(item.phanLoai)}</div><div class="cart-qty-actions"><input class="store-input mini-qty cart-qty-input" type="number" min="0" max="${Math.min(20, item.tonKho || 20)}" value="${item.soLuong}" data-id="${item.chiTietSanPhamID}" /><button type="button" class="link-button cart-remove-btn" data-id="${item.chiTietSanPhamID}">Xóa</button></div></div><strong class="price-sale">${formatCurrency(item.donGia * item.soLuong)}</strong></article>`).join("") : '<div class="empty-state">Giỏ hàng đang trống.</div>';
        };
        container.addEventListener("change", event => {
            const input = event.target.closest(".cart-qty-input");
            if (!input) return;
            const quantity = Number(input.value);
            const state = getCartState();
            if (quantity <= 0) {
                if (window.confirm("Bạn có muốn xóa sản phẩm này khỏi giỏ hàng không?")) {
                    state.items = state.items.filter(item => item.chiTietSanPhamID !== input.dataset.id);
                    saveCartState(state);
                    showToast("Đã xóa sản phẩm khỏi giỏ hàng.", "success");
                } else render();
                return;
            }
            const item = state.items.find(entry => entry.chiTietSanPhamID === input.dataset.id);
            if (!item) return;
            item.soLuong = clamp(quantity, 1, Math.min(20, item.tonKho || 20));
            saveCartState(state);
        });
        container.addEventListener("click", event => {
            const button = event.target.closest(".cart-remove-btn");
            if (!button) return;
            const state = getCartState();
            state.items = state.items.filter(item => item.chiTietSanPhamID !== button.dataset.id);
            saveCartState(state);
            showToast("Đã xóa sản phẩm khỏi giỏ hàng.", "success");
        });
        noteInput.addEventListener("input", () => {
            const state = getCartState();
            state.note = noteInput.value;
            saveCartState(state);
        });
        checkoutLink.addEventListener("click", event => {
            if (!getCartState().items.length) {
                event.preventDefault();
                showToast("Giỏ hàng đang trống.", "warning");
            }
        });
        document.addEventListener("winterCartChanged", render);
        render();
    }

    function initCheckout() {
        const page = document.getElementById("checkoutPage");
        if (!page) return;
        if (!getCartState().items.length) {
            window.location.href = "/Home/Cart";
            return;
        }
        const profile = getProfile();
        const fullName = document.getElementById("checkoutFullName");
        const email = document.getElementById("checkoutEmail");
        const phone = document.getElementById("checkoutPhone");
        const street = document.getElementById("checkoutStreet");
        const province = document.getElementById("checkoutProvince");
        const district = document.getElementById("checkoutDistrict");
        const ward = document.getElementById("checkoutWard");
        const shippingNode = document.getElementById("shippingOptions");
        const paymentNode = document.getElementById("paymentOptions");
        const couponInput = document.getElementById("checkoutCouponInput");
        const couponSuggestions = document.getElementById("couponSuggestions");
        const fillProvince = () => {
            province.innerHTML = provinces.map(item => `<option value="${item.value}">${escapeHtml(item.label)}</option>`).join("");
            province.value = profile.province || provinces[0].value;
            fillDistrict();
        };
        const fillDistrict = () => {
            const currentProvince = provinces.find(item => item.value === province.value) || provinces[0];
            district.innerHTML = currentProvince.districts.map(item => `<option value="${item.value}">${escapeHtml(item.label)}</option>`).join("");
            district.value = profile.district || currentProvince.districts[0].value;
            fillWard();
        };
        const fillWard = () => {
            const currentProvince = provinces.find(item => item.value === province.value) || provinces[0];
            const currentDistrict = currentProvince.districts.find(item => item.value === district.value) || currentProvince.districts[0];
            ward.innerHTML = currentDistrict.wards.map(item => `<option value="${item}">${escapeHtml(item)}</option>`).join("");
            ward.value = profile.ward || currentDistrict.wards[0];
        };
        const render = () => {
            const cart = getCartState();
            const totals = calculateTotals(cart);
            document.getElementById("checkoutItems").innerHTML = cart.items.map(item => `<article class="checkout-item"><div class="thumb-wrap">${buildArtwork({ sanPhamID: item.sanPhamID }, "compact")}</div><div><div class="product-name">${escapeHtml(item.tenSanPham)}</div><div class="item-meta">${escapeHtml(item.phanLoai)} · SL ${item.soLuong}</div></div><strong class="price-sale">${formatCurrency(item.donGia * item.soLuong)}</strong></article>`).join("");
            document.getElementById("checkoutSubtotal").textContent = formatCurrency(totals.subtotal);
            document.getElementById("checkoutShipping").textContent = formatCurrency(totals.shipping.fee);
            document.getElementById("checkoutDiscount").textContent = `- ${formatCurrency(totals.discount)}`;
            document.getElementById("checkoutGrandTotal").textContent = formatCurrency(totals.total);
            document.getElementById("checkoutSummaryMessage").textContent = totals.coupon ? `Đã áp dụng mã ${totals.coupon.code}.` : "Bạn có thể chọn mã giảm giá phù hợp trước khi hoàn tất.";
            couponInput.value = cart.couponCode;
            shippingNode.innerHTML = shippingMethods.map(item => {
                const disabled = item.minimum && totals.subtotal < item.minimum;
                const active = getShipping(cart, totals.subtotal).code === item.code;
                return `<label class="stack-option ${active ? "active" : ""} ${disabled ? "opacity-50" : ""}"><div class="stack-option-copy"><strong>${escapeHtml(item.label)}</strong><span>${escapeHtml(item.note)}</span></div><div><input type="radio" name="shippingMethod" value="${item.code}" ${active ? "checked" : ""} ${disabled ? "disabled" : ""} /><strong>${formatCurrency(item.fee)}</strong></div></label>`;
            }).join("");
            paymentNode.innerHTML = paymentMethods.map(item => `<label class="stack-option ${cart.paymentCode === item.code ? "active" : ""}"><div class="stack-option-copy"><strong>${escapeHtml(item.label)}</strong><span>${escapeHtml(item.note)}</span></div><input type="radio" name="paymentMethod" value="${item.code}" ${cart.paymentCode === item.code ? "checked" : ""} /></label>`).join("");
        };
        province.addEventListener("change", fillDistrict);
        district.addEventListener("change", fillWard);
        shippingNode.addEventListener("change", event => { const input = event.target.closest("[name='shippingMethod']"); if (!input) return; const cart = getCartState(); cart.shippingCode = input.value; saveCartState(cart); });
        paymentNode.addEventListener("change", event => { const input = event.target.closest("[name='paymentMethod']"); if (!input) return; const cart = getCartState(); cart.paymentCode = input.value; saveCartState(cart); });
        couponSuggestions.addEventListener("click", event => { const button = event.target.closest("[data-code]"); if (!button) return; couponInput.value = button.dataset.code; const cart = getCartState(); cart.couponCode = button.dataset.code; saveCartState(cart); });
        document.getElementById("applyCouponButton").addEventListener("click", () => {
            const coupon = getCoupon(couponInput.value);
            if (!coupon) return showToast("Mã giảm giá không hợp lệ.", "warning");
            const cart = getCartState();
            cart.couponCode = coupon.code;
            saveCartState(cart);
            showToast(`Đã áp dụng mã ${coupon.code}.`, "success");
        });
        document.getElementById("placeOrderButton").addEventListener("click", async () => {
            [fullName, phone, street].forEach(input => input.classList.toggle("is-invalid", !input.value.trim()));
            phone.classList.toggle("is-invalid", !isValidPhoneNumber(phone.value));
            email.classList.toggle("is-invalid", !!email.value.trim() && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value.trim()));
            if (!fullName.value.trim() || !street.value.trim() || !isValidPhoneNumber(phone.value) || (email.value.trim() && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value.trim()))) return showToast("Vui lòng kiểm tra lại thông tin khách hàng.", "warning");
            const currentProvince = provinces.find(item => item.value === province.value) || provinces[0];
            const currentDistrict = currentProvince.districts.find(item => item.value === district.value) || currentProvince.districts[0];
            const requestData = {
                tenKhachHang: fullName.value.trim(),
                soDienThoai: phone.value.replace(/\s+/g, ""),
                diaChiGiaoHang: `${street.value.trim()}, ${ward.value}, ${currentDistrict.label}, ${currentProvince.label}`,
                items: getCartState().items.map(item => ({ chiTietSanPhamID: item.chiTietSanPhamID, soLuong: item.soLuong }))
            };
            saveProfile({ fullName: fullName.value.trim(), email: email.value.trim(), phone: phone.value.trim(), street: street.value.trim(), province: province.value, district: district.value, ward: ward.value });
            const button = document.getElementById("placeOrderButton");
            try {
                button.disabled = true;
                button.textContent = "Đang xử lý...";
                const response = await fetch("/api/orders", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(requestData) });
                const result = await response.json();
                if (!response.ok) return showToast(result.message || "Không thể tạo đơn hàng.", "danger");
                showToast(`Đặt hàng thành công. Mã đơn: ${result.hoaDonID}`, "success");
                saveCartState({ items: [], note: "", couponCode: "", shippingCode: "vnpost", paymentCode: "cod" });
                setTimeout(() => { window.location.href = "/"; }, 1200);
            } catch (error) {
                console.error(error);
                showToast("Không thể kết nối tới máy chủ.", "danger");
            } finally {
                button.disabled = false;
                button.textContent = "Hoàn tất đơn hàng";
            }
        });
        fullName.value = profile.fullName || "";
        email.value = profile.email || "";
        phone.value = profile.phone || "";
        street.value = profile.street || "";
        couponSuggestions.innerHTML = coupons.map(item => `<button type="button" class="coupon-chip" data-code="${item.code}">${item.code} · ${item.label}</button>`).join("");
        fillProvince();
        document.addEventListener("winterCartChanged", render);
        render();
    }

    const productsPromise = fetchProducts().catch(error => {
        console.error(error);
        return [];
    });

    initHeader(productsPromise);
    initHome(productsPromise);
    initProducts(productsPromise);
    initDetail(productsPromise);
    initCart();
    initCheckout();
    updateCartPreview();
    document.addEventListener("winterCartChanged", event => updateCartPreview(event.detail));

    window.winterStore = { escapeHtml, formatCurrency, fetchProducts, fetchProduct, buildArtwork, getProductMeta: getMeta, renderProductCard };
    window.winterCart = { getCartState, getTotalQuantity, getSubtotal, calculateTotals, addItemToCart, formatCurrency, showToast, isValidPhoneNumber };
})();
