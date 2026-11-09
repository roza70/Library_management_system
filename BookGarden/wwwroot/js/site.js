
document.addEventListener('DOMContentLoaded', () => {

    // 1. Handle confirmation messages
    document.querySelectorAll('[data-confirm]').forEach((element) => {
        element.addEventListener('click', (event) => {
            const message = element.getAttribute('data-confirm');

            if (message && !window.confirm(message)) {
                event.preventDefault();
            }
        });
    });

    // 2. Load products from API
    const productPage = document.querySelector('[data-product-list]');
    const productsBody = document.querySelector('#products-table-body');

    if (productPage && productsBody) {
        loadProducts(productPage.dataset.productsApiUrl, productsBody);
    }
});


// 3. Fetch products from the API
async function loadProducts(apiUrl, tableBody) {
    try {
        const response = await fetch(apiUrl, {
            headers: {
                Accept: 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Products request failed: ${response.status}`);
        }

        const products = await response.json();

        // Clear existing server-rendered rows
        tableBody.replaceChildren();

       
        // Display each product
        products.forEach((product) => {
            tableBody.appendChild(createProductRow(product));
        });
        // Initialize DataTables
        new DataTable('#productTable', {
            pageLength: 10,
            lengthMenu: [5, 10, 25, 50],
            order: [[0, 'asc']],
            columnDefs: [
                {
                    targets: 5,
                    orderable: false,
                    searchable: false
                }
            ],
            language: {
                emptyTable: 'No products found.'
            }
        });

    } catch (error) {
        console.error('Unable to load products from the API.', error);
    }
}


// 4. Create a product table row
function createProductRow(product) {
    const row = document.createElement('tr');

    const values = [
        product.title,
        product.author,
        product.categoryName || 'Uncategorized',
        product.isbn,
        new Intl.NumberFormat(undefined, {
            style: 'currency',
            currency: 'USD'
        }).format(product.listPrice)
    ];

    // Create product information cells
    values.forEach((value, index) => {
        const cell = document.createElement('td');

        cell.textContent = value ?? '';

        if (index === 0) {
            cell.className = 'fairy-name';
        }

        row.appendChild(cell);
    });

    // Create action cell
    const actions = document.createElement('td');
    actions.className = 'text-end';

    const actionContainer = document.createElement('div');
    actionContainer.className = 'd-inline-flex gap-2';

    // Edit button
    const editLink = document.createElement('a');
    editLink.href = `/Admin/Product/Update/${product.id}`;
    editLink.className = 'btn btn-sm btn-fairy-edit';
    editLink.textContent = 'Edit';

    // Delete button
    const deleteLink = document.createElement('a');
    deleteLink.href = `/Admin/Product/Delete/${product.id}`;
    deleteLink.className = 'btn btn-sm btn-fairy-delete';
    deleteLink.textContent = 'Delete';

    actionContainer.append(editLink, deleteLink);
    actions.appendChild(actionContainer);
    row.appendChild(actions);

    return row;
}


// 5. Create a message row
function createCellRow(message) {
    const row = document.createElement('tr');

    const cell = document.createElement('td');
    cell.colSpan = 6;
    cell.className = 'text-center py-4';
    cell.textContent = message;

    row.appendChild(cell);

    return row;
}
