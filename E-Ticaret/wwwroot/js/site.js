$(document).ready(function () {
    $('body').on('click', 'a.ajax-link', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');

        if (!url || url.startsWith('#') || url.startsWith('javascript:')) return;

        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
                let newContent = $(data).find('#main-content').html();
                $('#main-content').html(newContent);
                window.history.pushState({}, '', url);
                window.scrollTo(0, 0); // 🚀 Sayfa başına scroll
            },
            error: function () {
                alert('Sayfa yüklenemedi.');
            }
        });
    });

    window.addEventListener('popstate', function () {
        $.get(location.href, function (data) {
            let newContent = $(data).find('#main-content').html();
            $('#main-content').html(newContent);
            window.scrollTo(0, 0); // 👈 Geri-ileri hareketlerde de scroll sıfırlansın
        });
    });
});


// site.js dosyasına eklemeniz gereken düzeltilmiş kod
let toastInstance = null; // Global değişken olarak tanımla

function showToast(message, type) {
    // Eğer zaten aktif bir toast varsa, önce onu kaldır
    if (toastInstance) {
        toastInstance.hide();
    }

    // Toast tipine göre arkaplan rengini ayarla
    const toast = $('#ajaxToast');
    toast.removeClass('bg-success bg-danger bg-warning bg-info');
    switch (type) {
        case 'success':
            toast.addClass('bg-success');
            break;
        case 'danger':
            toast.addClass('bg-danger');
            break;
        case 'warning':
            toast.addClass('bg-warning');
            break;
        case 'info':
            toast.addClass('bg-info');
            break;
        default:
            toast.addClass('bg-success');
    }

    // Toast mesajını ayarla
    $('#ajaxToastBody').text(message);

    // Bootstrap toast nesnesini oluştur
    toastInstance = new bootstrap.Toast(toast, {
        autohide: true,
        delay: 3000 // Toast görüntülenme süresi (ms)
    });

    // Toast'u göster
    toastInstance.show();

    // Toast kapandığında event listener
    toast.on('hidden.bs.toast', function () {
        toastInstance = null; // Toast kapandığında referansı temizle
    });
}

// Favori sayısını AJAX ile güncelleyen fonksiyon
function updateFavouritesCount() {
    $.ajax({
        type: "GET",
        url: "/Account/Summary",
        headers: {
            "X-Requested-With": "XMLHttpRequest" // AJAX isteği olduğunu belirt
        },
        success: function (count) {
            // Favori sayısını gösteren tüm elementleri güncelle
            $(".favourites-count").text(count);
        },
        error: function (error) {
            console.error("Favori sayısı güncellenemedi.", error);
        }
    });
}

$(document).ready(function () {
    // Favorilere ekle butonuna tıklandığında
    $(document).on("click", ".add-btn", function (e) {
        e.preventDefault(); // Sayfa yenilenmesini engeller
        const button = $(this);
        const productId = button.data("product-id");

        // Çift tıklama ve birden fazla istek göndermeyi engelle
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        $.ajax({
            type: "POST",
            url: "/Account/AddToFavourites",
            data: { id: productId },
            success: function (response) {
                showToast(response.message, response.type);
                if (response.success) {
                    // Favori butonunun ikonunu dolu kalp yap
                    button.find("i").removeClass("far").addClass("fas");
                    // Butonun class'ını güncelle
                    button.removeClass("add-btn").addClass("remove-btn");
                    // Tooltip güncelleme
                    button.attr("title", "Favorilerden Kaldır");

                    // Favori sayısını güncelle
                    updateFavouritesCount();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                showToast("Bir hata oluştu. Lütfen tekrar deneyin.", "danger");
            },
            complete: function () {
                button.removeClass('processing');
            }
        });
    });

    // Favorilerden kaldır butonuna tıklandığında
    $(document).on("click", ".remove-btn", function (e) {
        e.preventDefault(); // Sayfa yenilenmesini engeller
        const button = $(this);
        const productId = button.data("product-id");

        // Çift tıklama ve birden fazla istek göndermeyi engelle
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        $.ajax({
            type: "POST",
            url: "/Account/RemoveFromFavourites",
            data: { id: productId },
            success: function (response) {
                showToast(response.message, response.type);
                if (response.success) {
                    // Favori sayısını güncelle
                    updateFavouritesCount();

                    // Favori sayfasındaysak, ürün kartını animasyonlu bir şekilde kaldır
                    if ($('.favorites-container').length > 0) {
                        // Ürün kartının parent elementi (col div)
                        const productCard = button.closest('.product-wrapper');

                        // Animasyon ile kartı kaldır
                        productCard.addClass('animate__animated animate__fadeOut');

                        // Animasyon tamamlandıktan sonra DOM'dan kaldır
                        setTimeout(function () {
                            productCard.remove();

                            // Eğer tüm ürünler kaldırıldıysa "favori ürün yok" mesajını göster
                            if ($('.product-wrapper').length === 0) {
                                const emptyFavoritesHtml = `
                                    <div class="favorites-empty">
                                        <div class="text-center py-5">
                                            <i class="fa fa-heart-broken fa-3x mb-3"></i>
                                            <h4>Henüz favori ürününüz bulunmuyor</h4>
                                            <p>Beğendiğiniz ürünleri favorilerinize ekleyerek daha sonra kolayca ulaşabilirsiniz.</p>
                                            <a href="/Product/Index" class="btn favorites-shop-btn mt-3">
                                                <i class="fa fa-shopping-bag"></i> Alışverişe Başla
                                            </a>
                                        </div>
                                    </div>
                                `;

                                // Konteyneri temizle ve boş favoriler mesajını ekle
                                $('.favorites-products-wrapper').html(emptyFavoritesHtml);
                            }
                        }, 500);
                    } else {
                        // Normal sayfadaysak sadece ikonu değiştir
                        button.find("i").removeClass("fas").addClass("far");
                        // Button sınıfını güncelle
                        button.removeClass("remove-btn").addClass("add-btn");
                        // Tooltip güncelleme
                        button.attr("title", "Favorilere Ekle");
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                showToast("Bir hata oluştu. Lütfen tekrar deneyin.", "danger");
            },
            complete: function () {
                button.removeClass('processing');
            }
        });
    });

    // Toast element başlangıçta varsa, event listener'ı ayarla
    $('#ajaxToast').on('hidden.bs.toast', function () {
        toastInstance = null;
    });
});

// AJAX Cart Implementation - Add to site.js

// Cart item counter update function
function updateCartItemCount() {
    $.ajax({
        type: "GET",
        url: "/Cart/GetCartItemCount",
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (count) {
            $(".cart-item-count").text(count);
        },
        error: function (error) {
            console.error("Sepet sayısı güncellenemedi.", error);
        }
    });
}

// Modify existing document.ready function or add this to it
$(document).ready(function () {
    // Handle AJAX Add to Cart
    $(document).on("click", ".add-to-cart-btn", function (e) {
        e.preventDefault();
        const button = $(this);
        const productId = button.data("product-id");

        // Prevent double clicks
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        $.ajax({
            type: "POST",
            url: "/Cart/AddToCartAjax",
            data: { productId: productId },
            success: function (response) {
                // Show toast notification
                showToast(response.message, response.type);

                if (response.success) {
                    // Update cart item count
                    updateCartItemCount();

                    // Change button to "Remove from Cart"
                    const cartActionDiv = button.closest(".cart-action");
                    cartActionDiv.html(`
                        <button type="button" class="remove-from-cart-btn" data-product-id="${productId}">
                            <span class="btn-text">Sepetten Çıkar</span>
                            <span class="btn-icon"><i class="fas fa-trash-alt"></i></span>
                        </button>
                    `);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                showToast("Sepete eklenirken bir hata oluştu.", "danger");
            },
            complete: function () {
                button.removeClass('processing');
            }
        });
    });

    // Handle Remove from Cart
    $(document).on("click", ".remove-from-cart-btn", function (e) {
        e.preventDefault();
        const button = $(this);
        const productId = button.data("product-id");

        // Prevent double clicks
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        $.ajax({
            type: "POST",
            url: "/Cart/RemoveFromCartAjax",
            data: { productId: productId },
            success: function (response) {
                // Show toast notification
                showToast(response.message, response.type);

                if (response.success) {
                    // Update cart item count
                    updateCartItemCount();

                    // Change button back to "Add to Cart"
                    const cartActionDiv = button.closest(".cart-action");
                    cartActionDiv.html(`
                       <button type="button" class="add-to-cart-btn" data-product-id="${productId}">
                           <span class="btn-text">Sepete Ekle</span>
                           <span class="btn-icon"><i class="fas fa-cart-plus"></i></span>
                        </button>
                    `);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                showToast("Sepetten çıkarılırken bir hata oluştu.", "danger");
            },
            complete: function () {
                button.removeClass('processing');
            }
        });
    });
});
