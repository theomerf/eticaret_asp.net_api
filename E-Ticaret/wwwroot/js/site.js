// Cookie yönetimi için geliştirilmiş fonksiyonlar
function getFavoritesFromCookie() {
    const favoriteCookie = decodeURIComponent(getCookie("FavouriteProducts"));

    if (!favoriteCookie || favoriteCookie.trim() === "") {
        return [];
    }

    // Virgülle ayrılmış değerleri diziye çevir, boş değerleri filtrele
    const favorites = favoriteCookie
        .split('|')
        .map(id => {
            const trimmedId = id.trim();
            return trimmedId === "" ? NaN : parseInt(trimmedId);
        })
        .filter(id => !isNaN(id) && id > 0);

    return favorites;
}

function saveFavoritesToCookie(favorites) {
    // Tekrarlanan değerleri kaldır ve sırala
    const uniqueFavorites = [...new Set(favorites)].filter(id => id > 0).sort((a, b) => a - b);
    const favoritesString = uniqueFavorites.join('|');


    // Cookie'yi güvenli ayarlarla kaydet
    const expiryDate = new Date();
    expiryDate.setMonth(expiryDate.getMonth() + 1);

    document.cookie = `FavouriteProducts=${favoritesString};expires=${expiryDate.toUTCString()};path=/;SameSite=Lax`;

    // Cookie doğru şekilde kaydedildi mi kontrol et
    setTimeout(() => {
        const savedValue = getCookie("FavouriteProducts");

        if (savedValue !== favoritesString) {
            console.warn("Cookie may not have been saved correctly!");
        }
    }, 100);
}

function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            const value = cookie.substring(name.length + 1);
            return value;
        }
    }
    console.log(`Cookie ${name} not found`);
    return null;
}

// Favori sayısını güncelleme fonksiyonu
function updateFavouritesCount() {
    const favorites = getFavoritesFromCookie();
    const favCountElement = $('.favourites-count');
    if (favCountElement.length > 0) {
        favCountElement.text(favorites.length);
    }
}

// Toast gösterme fonksiyonu
let toastInstance = null;

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

function updateFavoriteButtons() {
    const favorites = getFavoritesFromCookie();

    // Tüm favori butonlarını kontrol et
    $('.action-btn.favorite-btn').each(function () {
        const button = $(this);
        const productId = parseInt(button.data("product-id"));

        if (favorites.includes(productId)) {
            // Ürün favorilerdeyse
            button.find("i").removeClass("far").addClass("fas");
            button.removeClass("add-btn").addClass("remove-btn");
            button.attr("title", "Favorilerden Kaldır");
        } else {
            // Ürün favorilerde değilse
            button.find("i").removeClass("fas").addClass("far");
            button.removeClass("remove-btn").addClass("add-btn");
            button.attr("title", "Favorilere Ekle");
        }
    });
}

// AJAX yükleme başarılı olduğunda favori butonlarını güncelleyen kod değişikliği
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
                window.scrollTo(0, 0);

                // AJAX ile içerik yüklendikten sonra favori butonlarını güncelle
                updateFavoriteButtons();
            },
            error: function () {
                alert('Sayfa yüklenemedi.');
            }
        });
    });

    $('body').on('submit', '#loginForm', function (e) {
        e.preventDefault();

        clearFormErrors();

        var token = $('input[name="__RequestVerificationToken"]').val();
        if (!token) {
            console.error("CSRF token bulunamadı!");
            return;
        }

        var formData = $(this).serialize();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            },
            success: function (response) {
                if (response.success) {
                    showMessage(response.message || "Giriş başarılı!", "success");

                    // Favori butonlarını güncelle
                    updateFavoriteButtons();

                    // Giriş başarılı olduktan sonra returnUrl'e AJAX ile git
                    $.ajax({
                        url: response.redirectUrl || "/",
                        type: 'GET',
                        success: function (data) {
                            let $data = $(data);

                            // Navbar elementlerini bul ve güncelle
                            let newNavbar = $data.filter('.custom-navbar, .navbar.custom-navbar').first();
                            let newCategoryNavbar = $data.filter('.category-navbar, .navbar.category-navbar').first();

                            // Navbar yoksa HTML içinden bul
                            if (newNavbar.length === 0) {
                                newNavbar = $data.find('.custom-navbar, .navbar.custom-navbar').first();
                            }

                            if (newCategoryNavbar.length === 0) {
                                newCategoryNavbar = $data.find('.category-navbar, .navbar.category-navbar').first();
                            }

                            if (newNavbar.length > 0) {
                                $('.custom-navbar').replaceWith(newNavbar);
                            }

                            if (newCategoryNavbar.length > 0) {
                                $('.category-navbar').replaceWith(newCategoryNavbar);
                            }

                            // Ana içeriği güncelle
                            let newContent = $data.find('#main-content').html();
                            if (newContent) {
                                $('#main-content').html(newContent);
                            }
                            window.history.pushState({}, '', response.redirectUrl || "/");
                            window.scrollTo(0, 0);

                            // Yeni sayfadaki favori butonlarını güncelle
                            updateFavoriteButtons();
                            // Cart sayısını güncelle
                            updateCartItemCount();

                            reinitializeNavbarScripts();
                        },
                        error: function () {
                            window.location.href = response.redirectUrl || "/";
                        }
                    });
                } else {
                    showMessage(response.message || "Giriş başarısız!", "error");

                    if (response.errors && response.errors.length > 0) {
                        displayFormErrors(response.errors);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("Login error:", error);
                showMessage("Giriş sırasında bir hata oluştu. Lütfen tekrar deneyin.", "error");
            }
        });
    });


    $("#registerForm").submit(function (e) {
        e.preventDefault();

        clearFormErrors();

        var formData = $(this).serialize();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            },
            success: function (response) {
                if (response.success) {
                    showMessage(response.message || "Kayıt başarılı!", "success");

                    // Login tab'ına geç
                    $('#loginTabBtn').tab('show');

                    // Kayıt formunu temizle
                    $('#registerForm')[0].reset();
                } else {
                    showMessage(response.message || "Kayıt başarısız!", "error");

                    if (response.errors && response.errors.length > 0) {
                        displayFormErrors(response.errors);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("Register error:", error);
                showMessage("Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.", "error");
            }
        });
    });

    $('body').on('click', '.logout-item', function (e) {
        e.preventDefault();

        var logoutUrl = $(this).attr("href");
        var returnUrl = window.location.pathname + window.location.search;

        $.ajax({
            url: logoutUrl,
            type: "GET",
            data: { ReturnUrl: returnUrl },
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            },
            success: function (response) {
                // Navbar'ı AJAX ile güncelle, tam sayfa şeklinde alıp ilgili kısımları yerleştiriyoruz
                $.ajax({
                    url: returnUrl,
                    type: 'GET',
                    success: function (data) {
                        // HTML yanıtını bir jQuery nesnesine dönüştür
                        let $data = $(data);

                        // Navbar elementlerini bul ve güncelle
                        let newNavbar = $data.filter('.custom-navbar, .navbar.custom-navbar').first();
                        let newCategoryNavbar = $data.filter('.category-navbar, .navbar.category-navbar').first();

                        // Navbar yoksa HTML içinden bul
                        if (newNavbar.length === 0) {
                            newNavbar = $data.find('.custom-navbar, .navbar.custom-navbar').first();
                        }

                        if (newCategoryNavbar.length === 0) {
                            newCategoryNavbar = $data.find('.category-navbar, .navbar.category-navbar').first();
                        }

                        if (newNavbar.length > 0) {
                            $('.custom-navbar').replaceWith(newNavbar);
                        }

                        if (newCategoryNavbar.length > 0) {
                            $('.category-navbar').replaceWith(newCategoryNavbar);
                        }

                        // Ana içeriği güncelle
                        let newContent = $data.find('#main-content').html();
                        if (newContent) {
                            $('#main-content').html(newContent);
                        }

                        // Favori butonlarını ve sepet sayısını güncelle
                        updateFavoriteButtons();
                        updateCartItemCount();

                        // Navbardaki scriptleri yeniden çalıştır
                        reinitializeNavbarScripts();

                        showToast("Başarıyla çıkış yapıldı", "success");
                    },
                    error: function () {
                        // Hata durumunda sayfayı yenilemeye geri dön
                        window.location.reload();
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error("Logout error:", error);
                window.location = logoutUrl;
            }
        });
    });

    // Navbar scriptlerini yeniden başlatan yardımcı fonksiyon
    function reinitializeNavbarScripts() {
        var topNavbar = document.querySelector('.custom-navbar');
        var categoryNavbar = document.querySelector('.category-navbar');

        if (topNavbar && categoryNavbar) {
            // Dropdown menüleri yeniden başlat
            var dropdownElementList = [].slice.call(document.querySelectorAll('.user-dropdown .dropdown-toggle'));

            dropdownElementList.forEach(function (dropdownToggleEl) {
                var dropdownMenu = dropdownToggleEl.nextElementSibling;

                if (dropdownMenu) {
                    dropdownMenu.classList.add('animate__animated');

                    // Event listener'ları kaldırıp yeniden ekle
                    dropdownToggleEl.removeEventListener('shown.bs.dropdown', dropdownAnimateIn);
                    dropdownToggleEl.removeEventListener('hide.bs.dropdown', dropdownAnimateOut);

                    dropdownToggleEl.addEventListener('shown.bs.dropdown', dropdownAnimateIn);
                    dropdownToggleEl.addEventListener('hide.bs.dropdown', dropdownAnimateOut);
                }
            });

            // Bootstrap bileşenlerini yeniden başlat
            var dropdowns = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'));
            dropdowns.map(function (dropdownToggleEl) {
                return new bootstrap.Dropdown(dropdownToggleEl);
            });
        }
    }

    // Dropdown animasyon fonksiyonları
    function dropdownAnimateIn() {
        var dropdownMenu = this.nextElementSibling;
        dropdownMenu.classList.remove('animate__fadeOutDown');
        dropdownMenu.classList.add('animate__fadeInUp');
    }

    function dropdownAnimateOut(e) {
        var dropdownMenu = this.nextElementSibling;
        if (!dropdownMenu.classList.contains('animate__fadeOutDown')) {
            e.preventDefault();

            dropdownMenu.classList.remove('animate__fadeInUp');
            dropdownMenu.classList.add('animate__fadeOutDown');

            setTimeout(function () {
                this.click();
                dropdownMenu.classList.remove('animate__fadeOutDown');
            }.bind(this), 300);
        }
    }

    function clearFormErrors() {
        $(".text-danger").html(""); // Tüm hata mesajlarını temizle
        $(".login-message").remove(); // Varolan uyarı mesajlarını temizle
    }

    // Hata mesajını gösteren yardımcı fonksiyon
    function showMessage(message, type) {
        // Mevcut mesajları temizle
        $(".login-message").remove();

        // Mesaj stilini belirle
        var alertClass = type === "success" ? "alert alert-success" : "alert alert-danger";

        // Mesajı form üstüne ekle
        var alertHtml = '<div class="' + alertClass + ' login-message">' + message + '</div>';
        $("#loginForm").prepend(alertHtml);
    }

    // Form hatalarını gösteren yardımcı fonksiyon
    function displayFormErrors(errors) {
        errors.forEach(function (error) {
            var fieldName = error.Key;
            var errorMessages = error.Errors.join("<br>");

            // Hata mesajını ilgili span'a ekle
            $("span[data-valmsg-for='" + fieldName + "']").html(errorMessages);
        });
    }

    window.addEventListener('popstate', function () {
        $.get(location.href, function (data) {
            let newContent = $(data).find('#main-content').html();
            $('#main-content').html(newContent);
            window.scrollTo(0, 0);

            // Tarayıcı geçmişinde gezinildiğinde de favori butonlarını güncelle
            updateFavoriteButtons();
        });
    });

    // Sayfa yüklendiğinde favori sayısını güncelle
    updateFavouritesCount();

    // Favorilere ekle butonuna tıklandığında - BUTTON için event handler
    $(document).on("click", ".favorite-btn.add-btn", function (e) {
        e.preventDefault(); // Sayfa yenilenmesini engeller
        const button = $(this);
        const productId = parseInt(button.data("product-id"));

        // Eğer kullanıcı giriş yapmamışsa login sayfasına yönlendir
        if (document.cookie.indexOf("FavouriteProducts") == -1) {
            const currentUrl = window.location.pathname + window.location.search;
            window.location.href = "/Account/Login?returnUrl=" + encodeURIComponent(currentUrl);
            return;
        }

        // Çift tıklama ve birden fazla istek göndermeyi engelle
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        // Cookie'den mevcut favorileri al
        const favorites = getFavoritesFromCookie();

        // Ürün zaten favorilerde mi kontrol et
        if (favorites.includes(productId)) {
            showToast("Bu ürün zaten favorilerinizde.", "info");
            button.removeClass('processing');
            return;
        }

        // Favorilere ekle
        favorites.push(productId);
        saveFavoritesToCookie(favorites);

        // UI güncelleme
        button.find("i").removeClass("far").addClass("fas");
        button.removeClass("add-btn").addClass("remove-btn");
        button.attr("title", "Favorilerden Kaldır");

        // Favori sayısını güncelle
        updateFavouritesCount();

        // Toast mesajı göster
        showToast("Ürün favorilerinize eklendi.", "success");
        button.removeClass('processing');
    });


    // Favorilerden kaldır butonuna tıklandığında - BUTTON için event handler
    $(document).on("click", ".favorite-btn.remove-btn", function (e) {
        e.preventDefault();
        const button = $(this);
        const productId = parseInt(button.data("product-id"));

        // Çift tıklama ve birden fazla istek göndermeyi engelle
        if (button.hasClass('processing')) return;
        button.addClass('processing');

        // Cookie'den mevcut favorileri al
        let favorites = getFavoritesFromCookie();

        // Ürünü favorilerden kaldır
        favorites = favorites.filter(id => id !== productId);
        saveFavoritesToCookie(favorites);

        // Favori sayısını güncelle
        updateFavouritesCount();

        // Favoriler sayfasındaysak, ürün kartını animasyonlu bir şekilde kaldır
        if ($('.favorites-container').length > 0) {
            // Ürün kartının parent elementi
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

        // Toast mesajı göster
        showToast("Ürün favorilerinizden kaldırıldı.", "success");
        button.removeClass('processing');
    });

    // Sayfa yüklendiğinde butonları güncelle
    updateFavoriteButtons();
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
