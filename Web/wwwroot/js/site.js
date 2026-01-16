// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Normaliza campos de nombre con mayuscula inicial por palabra.
const normalizarNombre = (valor) => {
    const texto = valor.trim();
    if (texto.length === 0) {
        return valor;
    }

    return texto
        .split(/\s+/)
        .map((palabra) => palabra.charAt(0).toUpperCase() + palabra.slice(1).toLowerCase())
        .join(' ');
};

document.addEventListener('blur', (evento) => {
    const objetivo = evento.target;
    if (!(objetivo instanceof HTMLInputElement)) {
        return;
    }

    if (objetivo.name !== 'nombre') {
        return;
    }

    objetivo.value = normalizarNombre(objetivo.value);
}, true);

document.addEventListener('submit', (evento) => {
    const formulario = evento.target;
    if (!(formulario instanceof HTMLFormElement)) {
        return;
    }

    const camposNombre = formulario.querySelectorAll('input[name="nombre"]');
    camposNombre.forEach((campo) => {
        campo.value = normalizarNombre(campo.value);
    });
}, true);
