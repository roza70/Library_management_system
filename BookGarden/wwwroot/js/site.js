// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('[data-confirm]').forEach((element) => {
        element.addEventListener('click', (event) => {
            const message = element.getAttribute('data-confirm');
            if (message && !window.confirm(message)) {
                event.preventDefault();
            }
        });

    const productPage = document.querySelector('[data-product-list]');
    const productsBody = document.querySelector('#products-table-body');

    if (productPage && productsBody) {
        loadProducts(productPage.dataset.productsApiUrl, productsBody);
    }
    });

async function loadProducts(apiUrl, tableBody) {
    try {
        const response = await fetch(apiUrl, {
            headers: { Accept: 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`Products request failed: ${response.status}`);
        }

        const products = await response.json();
        tableBody.replaceChildren();

        if (products.length === 0) {
            tableBody.appendChild(createCellRow('No products found.'));
            return;
        }

        products.forEach(product => tableBody.appendChild(createProductRow(product)));
    } catch (error) {
        console.error('Unable to load products from the API.', error);
    }
}

function createProductRow(product) {
    const row = document.createElement('tr');
    const values = [
        product.title,
        product.author,
        product.categoryName || 'Uncategorized',
        product.isbn,
        new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' })
            .format(product.listPrice)
    ];

    values.forEach((value, index) => {
        const cell = document.createElement('td');
        cell.textContent = value;
        if (index === 0) cell.className = 'fairy-name';
        row.appendChild(cell);
    });

    const actions = document.createElement('td');
    actions.className = 'text-end';
    actions.innerHTML = `
        <div class="d-inline-flex gap-2">
            <a href="/Admin/Product/Update/${product.id}" class="btn btn-sm btn-fairy-edit">Edit</a>
            <a href="/Admin/Product/Delete/${product.id}" class="btn btn-sm btn-fairy-delete">Delete</a>
        </div>`;
    row.appendChild(actions);
    return row;
}

function createCellRow(message) {
    const row = document.createElement('tr');
    const cell = document.createElement('td');
    cell.colSpan = 6;
    cell.className = 'text-center py-4';
    cell.textContent = message;
    row.appendChild(cell);
    return row;
}
});
