document.addEventListener('DOMContentLoaded', function () {
    // Thumbnail image switching
    const thumbnails = document.querySelectorAll('.thumbnail');
    const mainImage = document.getElementById('mainImage');

    if (thumbnails.length && mainImage) {
        thumbnails.forEach(thumbnail => {
            thumbnail.addEventListener('click', function (e) {
                e.preventDefault();

                // Remove active class from all thumbnails
                thumbnails.forEach(t => t.classList.remove('active'));

                // Add active class to clicked thumbnail
                this.classList.add('active');

                // Update main image with fade effect
                mainImage.style.opacity = '0.5';

                setTimeout(() => {
                    mainImage.src = this.src;
                    mainImage.alt = this.alt;
                    mainImage.style.opacity = '1';
                }, 150);
            });
        });
    }

    // Quantity selector for purchase
    const quantityInput = document.getElementById('quantity');
    const decreaseBtn = document.querySelector('.quantity-decrease');
    const increaseBtn = document.querySelector('.quantity-increase');

    if (quantityInput && decreaseBtn && increaseBtn) {
        decreaseBtn.addEventListener('click', function () {
            let currentValue = parseInt(quantityInput.value);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
            }
        });

        increaseBtn.addEventListener('click', function () {
            let currentValue = parseInt(quantityInput.value);
            const maxValue = parseInt(quantityInput.getAttribute('max')) || 10;
            if (currentValue < maxValue) {
                quantityInput.value = currentValue + 1;
            }
        });
    }

    // Add to cart button feedback
    const addToCartBtn = document.getElementById('addToCartBtn');
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', function () {
            const originalText = this.innerHTML;
            this.innerHTML = '<i class="bi bi-check-circle"></i> Added!';
            this.classList.add('btn-success');
            this.classList.remove('btn-dark');

            setTimeout(() => {
                this.innerHTML = originalText;
                this.classList.remove('btn-success');
                this.classList.add('btn-dark');
            }, 2000);
        });
    }

    // Image zoom on hover (optional enhancement)
    if (mainImage) {
        mainImage.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.05)';
            this.style.transition = 'transform 0.3s ease';
        });

        mainImage.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
        });
    }

    // Share functionality (optional)
    const shareBtn = document.getElementById('shareBtn');
    if (shareBtn) {
        shareBtn.addEventListener('click', function (e) {
            e.preventDefault();

            if (navigator.share) {
                navigator.share({
                    title: document.title,
                    url: window.location.href
                }).catch(err => console.log('Error sharing:', err));
            } else {
                // Fallback: copy to clipboard
                navigator.clipboard.writeText(window.location.href).then(() => {
                    alert('Link copied to clipboard!');
                });
            }
        });
    }
});
