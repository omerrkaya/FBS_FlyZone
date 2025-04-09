const modalBg = document.querySelector('.modal-bg');
const detailsBtn = document.querySelector('.details-btn');
const closeBtn = document.querySelector('.close-btn');

// "Detayları Göster" butonuna tıklanınca modalı aç
detailsBtn.addEventListener('click', function () {
    modalBg.style.display = 'flex';
});

// Kapatma butonuna tıklanınca modalı kapat
closeBtn.addEventListener('click', function () {
    modalBg.style.display = 'none';
});

// Modal arka planına tıklanınca modalı kapat
window.addEventListener('click', function (e) {
    if (e.target === modalBg) {
        modalBg.style.display = 'none';
    }
});
