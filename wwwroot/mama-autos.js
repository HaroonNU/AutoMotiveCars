// Enhanced UI interactions for Mama Autos
document.addEventListener('DOMContentLoaded', function() {
    
    // Smooth scroll animation for navigation
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 150);
        });
    });

    // Info cards hover animation
    const infoCards = document.querySelectorAll('.info-card');
    infoCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-8px) scale(1.02)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    // Company name typing effect
    const companyName = document.querySelector('.company-name');
    if (companyName) {
        const text = companyName.textContent;
        companyName.textContent = '';
        let i = 0;
        
        function typeWriter() {
            if (i < text.length) {
                companyName.textContent += text.charAt(i);
                i++;
                setTimeout(typeWriter, 100);
            }
        }
        
        setTimeout(typeWriter, 500);
    }

    // Welcome message fade in
    const welcomeMessage = document.querySelector('.welcome-message');
    if (welcomeMessage) {
        welcomeMessage.style.opacity = '0';
        welcomeMessage.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            welcomeMessage.style.transition = 'all 1s ease';
            welcomeMessage.style.opacity = '1';
            welcomeMessage.style.transform = 'translateY(0)';
        }, 2000);
    }

    // Contact info stagger animation
    const contactCards = document.querySelectorAll('.info-card');
    contactCards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        
        setTimeout(() => {
            card.style.transition = 'all 0.6s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, 1200 + (index * 200));
    });
});