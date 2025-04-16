    document.addEventListener('DOMContentLoaded', () => {
    // Efecto hover para los cuadros
    const cuadros = document.querySelectorAll('.cuadro-enlace');

    cuadros.forEach(cuadro => {
    // Efecto hover
    cuadro.addEventListener('mouseenter', () => {
    cuadro.style.transform = 'scale(1.05)';
    cuadro.style.boxShadow = '0 10px 20px rgba(0,0,0,0.2)';
});

    cuadro.addEventListener('mouseleave', () => {
    cuadro.style.transform = 'scale(1)';
    cuadro.style.boxShadow = '0 5px 15px rgba(0,0,0,0.1)';
});

    // Redirección al hacer clic
    cuadro.addEventListener('click', () => {
    if (cuadro.dataset.url) {
    window.location.href = cuadro.dataset.url;
}
});
});

    // Efecto hover para iconos sociales
    const socialIcons = document.querySelectorAll('.social-icon');

    socialIcons.forEach(icon => {
    icon.addEventListener('mouseenter', () => {
    icon.style.transform = 'scale(1.2)';
});

    icon.addEventListener('mouseleave', () => {
    icon.style.transform = 'scale(1)';
});
});

    // Efecto hover para botones CTA
    const ctaButtons = document.querySelectorAll('.cta-button');

    ctaButtons.forEach(button => {
    button.addEventListener('mouseenter', () => {
    button.style.transform = 'translateY(-5px)';
    button.style.boxShadow = '0 10px 20px rgba(0,0,0,0.2)';
});

    button.addEventListener('mouseleave', () => {
    button.style.transform = 'translateY(0)';
    button.style.boxShadow = '0 5px 15px rgba(0,0,0,0.1)';
});
});

    // Slider de anuncios
    const sliderTexts = [
    "¡La detección temprana salva vidas! Infórmate sobre prevención del cáncer hoy.",
    "Participa en nuestra carrera solidaria el próximo 15 de junio.",
    "Más de 40 años brindando apoyo a pacientes con la mejor tecnología."
    ];

    let currentSlide = 0;
    const sliderContent = document.querySelector('.announcement-slider p');
    const nextBtn = document.querySelector('.slider-next');
    const prevBtn = document.querySelector('.slider-prev');

    function showSlide(index) {
    sliderContent.textContent = sliderTexts[index];
}

    nextBtn.addEventListener('click', () => {
    currentSlide = (currentSlide + 1) % sliderTexts.length;
    showSlide(currentSlide);
});

    prevBtn.addEventListener('click', () => {
    currentSlide = (currentSlide - 1 + sliderTexts.length) % sliderTexts.length;
    showSlide(currentSlide);
});

    // Cambiar slide automáticamente cada 5 segundos
    setInterval(() => {
    currentSlide = (currentSlide + 1) % sliderTexts.length;
    showSlide(currentSlide);
}, 5000);
});
    document.addEventListener('DOMContentLoaded', () => {
        // Efecto hover para los iconos sociales
        const socialIcons = document.querySelectorAll('.social-icon');

        socialIcons.forEach(icon => {
            icon.addEventListener('mouseenter', () => {
                icon.style.transform = 'scale(1.2)';
            });

            icon.addEventListener('mouseleave', () => {
                icon.style.transform = 'scale(1)';
            });
        });

        // Calculador de días restantes
        function actualizarContador() {
            const fechaObjetivo = new Date('June 13, 2025');
            const ahora = new Date();

            // Diferencia en milisegundos
            const diferencia = fechaObjetivo - ahora;

            // Convertir a días
            const diasRestantes = Math.ceil(diferencia / (1000 * 60 * 60 * 24));

            // Actualizar el contador
            const contador = document.getElementById('countdown');
            contador.textContent = `${diasRestantes} días`;
        }

        // Actualizar contador al cargar
        actualizarContador();

        // Actualizar contador cada día
        setInterval(actualizarContador, 86400000); // 24 horas en milisegundos
    });
