document.addEventListener("DOMContentLoaded", function () {
    fetchProducts();
});
document.getElementById("CategoryId").addEventListener("change", (e) => {
    document.getElementById('product_rows').dataset['id'] = e.target.value;
    fetchProducts();
});
document.getElementById('Discontinued').addEventListener("change", (e) => {
    fetchProducts();
});
// delegated event listener
document.getElementById('product_rows').addEventListener("click", (e) => {
    p = e.target.parentElement;
    if (p.classList.contains('product')) {
        e.preventDefault()
        // console.log(p.dataset['id']);
        const cart = new bootstrap.Modal('#cartModal', {}).show();
    }
});
// update total when cart quantity is changed
document.getElementById('Quantity').addEventListener("change", (e) => {
    // console.log(e.target.value);
    const total = parseInt(e.target.value) * Number(document.getElementById('UnitPrice').innerHTML);
    document.getElementById('Total').innerHTML = numberWithCommas(total.toFixed(2));
});
// function to display commas in number
const numberWithCommas = x => x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
async function fetchProducts() {
    const id = document.getElementById('product_rows').dataset['id'];
    const discontinued = document.getElementById('Discontinued').checked ? "" : "/discontinued/false";
    const { data: fetchedProducts } = await axios.get(`../../api/category/${id}/product${discontinued}`);
    // console.log(fetchedProducts);
    let product_rows = "";
    fetchedProducts.map(product => {
        const css = product.discontinued ? " discontinued" : "";
        product_rows +=
            `<tr class="product${css}" data-id="${product.productId}">
        <td>${product.productName}</td>
        <td class="text-end">${product.unitPrice.toFixed(2)}</td>
        <td class="text-end">${product.unitsInStock}</td>
      </tr>`;
    });
    document.getElementById('product_rows').innerHTML = product_rows;
}