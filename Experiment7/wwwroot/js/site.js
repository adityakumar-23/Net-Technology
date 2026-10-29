// Feedback Management System - Interactive Logic & Validation Enhancements

document.addEventListener('DOMContentLoaded', () => {
    // 1. Interactive Star Rating Selector
    const ratingLabels = document.querySelectorAll('.star-rating-group label');
    const ratingRadios = document.querySelectorAll('.star-rating-group input[type="radio"]');
    const ratingValueDisplay = document.getElementById('selectedRatingText');

    const ratingDescriptions = {
        1: '1 Star - Poor / Unsatisfactory',
        2: '2 Stars - Fair / Needs Improvement',
        3: '3 Stars - Average / Satisfactory',
        4: '4 Stars - Good / Exceeded Expectations',
        5: '5 Stars - Excellent / Outstanding'
    };

    function updateRatingLabel(val) {
        if (ratingValueDisplay && ratingDescriptions[val]) {
            ratingValueDisplay.textContent = ratingDescriptions[val];
        }
    }

    ratingRadios.forEach(radio => {
        if (radio.checked) {
            updateRatingLabel(radio.value);
        }
        radio.addEventListener('change', (e) => {
            updateRatingLabel(e.target.value);
        });
    });

    ratingLabels.forEach(label => {
        label.addEventListener('mouseenter', () => {
            const forId = label.getAttribute('for');
            const match = forId ? forId.match(/\d+/) : null;
            if (match && ratingDescriptions[match[0]]) {
                if (ratingValueDisplay) ratingValueDisplay.textContent = ratingDescriptions[match[0]];
            }
        });
        label.addEventListener('mouseleave', () => {
            const checked = document.querySelector('.star-rating-group input[type="radio"]:checked');
            if (checked) {
                updateRatingLabel(checked.value);
            } else if (ratingValueDisplay) {
                ratingValueDisplay.textContent = 'Please select a star rating';
            }
        });
    });

    // 2. Sync aria-invalid with modern web guidance
    const syncAria = (el) => {
        if (el && el.matches) {
            const isInvalid = el.matches(':user-invalid') || el.classList.contains('input-validation-error');
            el.setAttribute('aria-invalid', isInvalid ? 'true' : 'false');
        }
    };

    document.addEventListener('blur', (e) => {
        if (e.target && (e.target.tagName === 'INPUT' || e.target.tagName === 'SELECT' || e.target.tagName === 'TEXTAREA')) {
            syncAria(e.target);
        }
    }, true);

    document.addEventListener('input', (e) => {
        if (e.target && e.target.hasAttribute && e.target.hasAttribute('aria-invalid')) {
            syncAria(e.target);
        }
    });

    // 3. Auto-dismiss alerts after 6 seconds
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            const closeBtn = alert.querySelector('.btn-close');
            if (closeBtn) {
                closeBtn.click();
            }
        }, 6000);
    });

    // 4. Word / Character count for Feedback Textarea
    const commentsArea = document.querySelector('textarea[name="Comments"]');
    const charCounter = document.getElementById('charCountDisplay');
    if (commentsArea && charCounter) {
        const updateCount = () => {
            const len = commentsArea.value.length;
            charCounter.textContent = `${len} / 1000 characters`;
            if (len < 15) {
                charCounter.classList.add('text-danger');
                charCounter.classList.remove('text-muted');
            } else {
                charCounter.classList.remove('text-danger');
                charCounter.classList.add('text-muted');
            }
        };
        commentsArea.addEventListener('input', updateCount);
        updateCount();
    }
});
